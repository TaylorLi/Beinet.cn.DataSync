using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;

namespace Beinet.cn.DataSync
{
    public partial class MainForm : Form
    {
        // 总列数
        private const int COL_COUNT = 4;
        // 源表列号
        private const int COL_SOURCE = 0;
        // 目标表列号
        private const int COL_TARGET = 1;
        // 源表备份列号，用于标识这行是否自定义查询
        private const int COL_SOURCEBACK = 2;
        // 目标表备份列号，用于输入目标表，又取消勾选，再勾选时的恢复
        private const int COL_TARGETBACK = 3;

        private readonly string configPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "sync.xml");

        public MainForm()
        {
            InitializeComponent();

            // 用于后面可以给标题栏加复选框
            //lvTables.OwnerDraw = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if(File.Exists(configPath))
            {
                #region 配置文件存在时，从配置文件加载数据
                SyncTask task;
                try
                {
                    using (var xmlreader = new XmlTextReader(configPath))
                    {
                        // [\x0-\x8\x11\x12\x14-\x32]
                        // 默认为true，如果序列化的对象含有比如0x1e之类的非打印字符，反序列化就会出错，因此设置为false http://msdn.microsoft.com/en-us/library/aa302290.aspx
                        xmlreader.Normalization = false;
                        xmlreader.WhitespaceHandling = WhitespaceHandling.Significant;
                        xmlreader.XmlResolver = null;
                        var formatter = new DataContractSerializer(typeof(SyncTask));
                        task = formatter.ReadObject(xmlreader) as SyncTask;
                    }
                }
                catch
                {
                    task = null;
                }
                if(task != null)
                {
                    txtDbSource.Text = task.SourceConstr;
                    txtDbTarget.Text = task.TargetConstr;
                    chkClear.Checked = task.TruncateOld;
                    chkErrContinue.Checked = task.ErrContinue;
                    chkIdentifier.Checked = task.UseIdentifier;
                    if(task.Items != null)
                    {
                        bool haverow = false;
                        foreach (SyncItem item in task.Items)
                        {
                            string[] values = new string[COL_COUNT];
                            values[COL_SOURCE] = item.Source;
                            values[COL_TARGET] = item.Target;
                            if (item.IsSqlSource) 
                                values[COL_SOURCEBACK] = item.Target;

                            lvTables.Items.Add(new ListViewItem(values){Checked = true});
                            haverow = true;
                        }
                        if (haverow)
                        {
                            btnSyncBegin.Enabled = true;
                            btnAddNewSql.Enabled = true;
                            btnDelRow.Enabled = true;
                            btnSaveConfig.Enabled = true;
                        }
                    }
                }
                lstTarget.Items.Add("请点击按钮：获取表结构");

                #endregion
            }
        }

        private void lvTables_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            //if (e.ColumnIndex == 0)
            //{
            //    TextFormatFlags flags = TextFormatFlags.LeftAndRightPadding;
            //    e.DrawBackground();

            //    //CheckBoxRenderer.DrawCheckBox(e.Graphics, ClientRectangle.Location, System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal); 
            //    CheckBoxRenderer.DrawCheckBox(e.Graphics, ClientRectangle.Location, Rectangle.Empty, "", this.Font,
            //                                  TextFormatFlags.HorizontalCenter, false, CheckBoxState.CheckedHot);

            //    e.DrawText(flags);
            //}
        }


        // 备份主调方法

        private void btnSyncBegin_Click(object sender, EventArgs e)
        {
            SyncTask task = GetTask();
            if (task == null)
                return;
            try
            {
                using (var sync = new SyncForm(task))
                {
                    sync.ShowDialog(this);
                }
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        // 保存为配置文件
        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            SyncTask task = GetTask();
            if (task == null)
                return;

            try
            {
                // 让输出的xml可读性好
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    Encoding = Encoding.UTF8,
                    IndentChars = "    "
                };
                using (FileStream fs = new FileStream(configPath, FileMode.Create))
                using (XmlWriter writer = XmlWriter.Create(fs, settings))
                {
                    var formatter = new DataContractSerializer(task.GetType());
                    formatter.WriteObject(writer, task);
                    //formatter.WriteObject(fs, task);
                }
                var diag = MessageBox.Show("成功保存到配置文件，下次启动时将自动载入\r\n是否打开文件所在目录？", "打开目录", MessageBoxButtons.YesNo);
                if(diag == DialogResult.Yes)
                {
                    Process.Start("explorer.exe", @" /select," + configPath);
                }
            }
            catch(Exception exp)
            {
                MessageBox.Show("保存到文件" + configPath + "失败:\r\n" + exp.ToString());
            }
        }

        SyncTask GetTask()
        {
            string strSourceConn, strTargetConn;
            if (!CheckConn(out strSourceConn, out strTargetConn))
            {
                return null;
            }
            if (lvTables.CheckedItems.Count <= 0)
            {
                MessageBox.Show("请选择要同步的源表");
                return null;
            }
            var arr = new Dictionary<string, SyncItem>();
            foreach (ListViewItem item in lvTables.CheckedItems)
            {
                string target = item.SubItems[COL_TARGET].Text.Trim();
                string key = target.ToLower();
                if (arr.ContainsKey(key))
                {
                    MessageBox.Show(target + " 目标表选择重复，请重新指定");
                    return null;
                }
                string source = item.SubItems[COL_SOURCE].Text.Trim();

                arr.Add(key, new SyncItem
                {
                    Source = source,
                    Target = target,
                    IsSqlSource = !string.IsNullOrEmpty(item.SubItems[COL_SOURCEBACK].Text),
                });
            }
            var task = new SyncTask
            {
                Items = arr.Values,
                ErrContinue = chkErrContinue.Checked,
                SourceConstr = strSourceConn,
                TargetConstr = strTargetConn,
                TruncateOld = chkClear.Checked,
                UseIdentifier = chkIdentifier.Checked,
            };
            return task;
        }


        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            var check = (((CheckBox)sender).Checked);
            foreach (ListViewItem item in lvTables.Items)
            {
                item.Checked = check;
            }
        }




        private void btnGetSchma_Click(object sender, EventArgs e)
        {
            lstSource.Visible = false;
            lstTarget.Visible = false;

            string strSourceConn, strTargetConn;
            if (!CheckConn(out strSourceConn, out strTargetConn))
            {
                return;
            }
            // 绑定源数据库的所有表和视图到ListView
            BindTables(strSourceConn, lvTables, true);
            // 绑定目标数据库的所有表到ComboBox
            BindTables(strTargetConn, lstTarget, false);

            btnSyncBegin.Enabled = true;
            btnAddNewSql.Enabled = true;
            btnDelRow.Enabled = true;
            btnSaveConfig.Enabled = true;
        }

        private void btnAddNewSql_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("请输入自定义SQL", "", MessageBoxButtons.OKCancel, )
            string sql = string.Empty;
            using (Prompt prompt = new Prompt())
            {
                prompt.ShowDialog(this);
                if (prompt.IsOk && !prompt.IsDisposed)
                    sql = prompt.Sql;
            }
            if (string.IsNullOrEmpty(sql))
                return;

            // 源数据库增加自定义查询行
            string[] values = new string[COL_COUNT];
            values[COL_SOURCE] = sql;
            string tbName = "Query_" + Guid.NewGuid().GetHashCode().ToString();
            values[COL_SOURCEBACK] = tbName;
            values[COL_TARGET] = tbName;


            var item = new ListViewItem(values) { Checked = true };
            lvTables.Items.Insert(0, item);
        }

        private void btnDelRow_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvTables.CheckedItems)
            {
                lvTables.Items.Remove(item);
            }
        }


        private void txtDbSource_TextChanged(object sender, EventArgs e)
        {
            btnSyncBegin.Enabled = false;
            //btnAddNewSql.Enabled = false;
            //btnDelRow.Enabled = false;
        }

        private bool CheckConn(out string sourceConn, out string targetConn)
        {
            sourceConn = txtDbSource.Text.Trim();
            targetConn = txtDbTarget.Text.Trim();
            if (sourceConn == string.Empty || targetConn == string.Empty)
            {
                MessageBox.Show("请输入数据源或目标连接字符串");
                return false;
            }
            return true;
        }

        private void lvTables_MouseUp(object sender, MouseEventArgs e)
        {
            ShowCombobox(lvTables, lstTarget, e.X, e.Y, COL_TARGET, COL_SOURCE);
        }

        private void lvTables_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            string targetTbName = e.Item.SubItems[COL_TARGET].Text;
            if (e.Item.Checked)
            {
                if (string.IsNullOrEmpty(targetTbName))
                    targetTbName = e.Item.SubItems[COL_TARGETBACK].Text;
                if (string.IsNullOrEmpty(targetTbName))
                    targetTbName = e.Item.SubItems[COL_SOURCE].Text;
                e.Item.SubItems[COL_TARGET].Text = targetTbName;
            }
            else
            {
                e.Item.SubItems[COL_TARGET].Text = string.Empty;
                e.Item.SubItems[COL_TARGETBACK].Text = targetTbName;
            }
        }

        void lstLeave(object sender, EventArgs e)
        {
            ComboBox lst = (ComboBox)sender;
            int col;
            if (!int.TryParse(lst.Name, out col))
                return;
            lst.Visible = false;
            if (comboboxItem != null)
            {
                string target = lst.Text.Trim();
                if (target != string.Empty)
                    comboboxItem.SubItems[col].Text = target;
            }
        }

        #region 静态方法集

        private static void BindTables(string connstr, Control ctl, bool getViews)
        {
            ComboBox lst = ctl as ComboBox;
            if (lst != null)
                lst.Items.Clear();
            ListView lv = ctl as ListView;
            if (lv != null)
                lv.Items.Clear();

            string sql = "select name from sys.objects where type='U'";
            if (getViews)
                sql += " or type='V'";
            sql += " order by name";

            using (SqlDataReader reader = Common.ExecuteReader(connstr, sql))
            {
                while (reader.Read())
                {
                    string name = Convert.ToString(reader[0]);
                    if (lst != null)
                        lst.Items.Add(name);
                    else if (lv != null)
                    {
                        string[] values = new string[COL_COUNT];
                        values[COL_SOURCE] = name;
                        lv.Items.Add(new ListViewItem(values));
                    }
                }
            }
        }

        private static ListViewItem comboboxItem = null;
        private static void ShowCombobox(ListView lv, ComboBox lst, int x, int y, int col, int defaultValueCol)
        {
            var item = lv.GetItemAt(x, y);
            if (item == null || !item.Checked)
                return;
            comboboxItem = item;

            int lWidth = 0, rWidth = 0;
            for (int i = 0; i <= col; i++)
            {
                int tmp = lv.Columns[i].Width;
                if (i < col)
                    lWidth += tmp;
                rWidth += tmp;
            }

            if (x > rWidth || x < lWidth)
            {
                lst.Visible = false;
                return;
            }

            //获取所在位置的行的Bounds            
            Rectangle rect = item.Bounds;
            //修改Rect的范围使其与第二列的单元格的大小相同，为了好看 ，左边缩进了2个单位                       
            rect.X += lv.Left + lWidth + 2;
            rect.Y += lv.Top + 2;
            rect.Width = rWidth - lWidth;
            lst.Bounds = rect;
            string val = item.SubItems[col].Text;
            if (string.IsNullOrEmpty(val))
                val = item.SubItems[defaultValueCol].Text;
            lst.Text = val;
            lst.Visible = true;
            lst.BringToFront();
            lst.Focus();
            lst.Name = col.ToString();

            //lst.SelectedIndexChanged -= lstSelectedIndexChanged;// (obj, args) => { MessageBox.Show("1"); };
        }

        #endregion
        //// 获取指定事件的绑定的全部委托
        //void ttt()
        //{
        //    PropertyInfo propertyInfo = (typeof(System.Windows.Forms.Button)).GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic);
        //    EventHandlerList eventHandlerList = (EventHandlerList)propertyInfo.GetValue(btn_Retrive, null);
        //    FieldInfo fieldInfo = (typeof(Control)).GetField("EventClick", BindingFlags.Static | BindingFlags.NonPublic);
        //    if (fieldInfo != null)
        //    {
        //        Delegate d = eventHandlerList[fieldInfo.GetValue(null)];
        //        if (d != null)
        //        {
        //            foreach (Delegate temp in d.GetInvocationList())
        //            {
        //               // btn_Retrive -= temp;
        //            }
        //        }
        //    }
        //}

    }
}