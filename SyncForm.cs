using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Forms;

namespace Beinet.cn.DataSync
{
    public partial class SyncForm : Form
    {
        public SyncForm(SyncTask task)
        {
            InitializeComponent();
            ShowInTaskbar = false;// 不能放到OnLoad里，会导致窗体消失

            this.arr = task.Items;
            this.sourceCon = task.SourceConstr;
            this.targetCon = task.TargetConstr;
            this.truncate = task.TruncateOld;
            this.useIdentifier = task.UseIdentifier;
            this.errContinue = task.ErrContinue;
        }

        private readonly IEnumerable<SyncItem> arr;
        private readonly string sourceCon;
        private readonly string targetCon;
        private readonly bool truncate;
        private readonly bool useIdentifier;
        private readonly bool errContinue;

        private bool canceled = false;
        private bool finished = false;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Owner != null)
            {
                Left = Owner.Left + 20;
                Top = Owner.Top + Owner.Height - Height - 20;
                if (Top < 0)
                    Top = 0;
            }

            ThreadPool.UnsafeQueueUserWorkItem(DoSync, null);

            //new Thread(DoSync).Start();
        }

        void DoSync(object state)
        {
            try
            {
                foreach (var item in arr)
                {
                    if (canceled)
                        break;

                    var row = new ListViewItem(new string[] { item.Source, item.Target, "处理中……" });
                    _viewItem = row;
                    ListViewAddRow(lvTables, row);
                    try
                    {
                        string sql = item.Source;
                        if (!item.IsSqlSource)
                        {
                            sql = "SELECT * FROM [" + sql + "] WITH(NOLOCK)";
                        }
                        using (SqlDataReader reader = Common.ExecuteReader(sourceCon, sql))
                        {
                            if (!reader.HasRows)
                            {
                                SetListViewText(_viewItem, 2, "源表无数据，未同步", Color.Red);
                                continue;
                            }

                            if (Common.ExistsTable(targetCon, item.Target))
                            {
                                if (truncate)
                                {
                                    Common.TruncateTable(targetCon, item.Target);
                                }
                            }
                            else
                            {
                                // 目标不存在，创建它
                                string createSql = Common.GetCreateSql(reader, item.Target);
                                Common.ExecuteNonQuery(targetCon, createSql);
                            }
                            var opn = useIdentifier ? SqlBulkCopyOptions.KeepIdentity : SqlBulkCopyOptions.Default;
                            Stopwatch sw = new Stopwatch();
                            sw.Start();
                            using (SqlBulkCopy bcp = new SqlBulkCopy(targetCon, opn))
                            {
                                bcp.SqlRowsCopied += bcp_SqlRowsCopied; // 用于进度显示
                                int batchSize = 2000;
                                bcp.BatchSize = batchSize;
                                bcp.NotifyAfter = batchSize;// 设置为1，状态栏提示比较准确，但是速度很慢

                                bcp.DestinationTableName = item.Target;
                                bcp.WriteToServer(reader);
                            }
                            //long totalrow = Common.GetTableRows(targetCon, item.Target);
                            sw.Stop();
                            string oldTxt = _viewItem.SubItems[2].Text;
                            SetListViewText(_viewItem, 2,
                                oldTxt + " 同步完成,耗时:" + sw.ElapsedMilliseconds.ToString("N0") + "毫秒", Color.Green);//，记录数:" + totalrow.ToString("N0")
                        }
                    }
                    catch (OperationAbortedException)
                    {
                        //bcp_SqlRowsCopied里调用Abort
                    }
                    catch (Exception exp)
                    {
                        SetListViewText(_viewItem, 2, "错误:" + exp, Color.Red);
                        if (!errContinue)
                            break;
                    }
                }
                finished = true;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp + "");
            }
        }

        private ListViewItem _viewItem;
        void bcp_SqlRowsCopied(object sender, SqlRowsCopiedEventArgs args)
        {
            if (canceled)
            {
                args.Abort = true;
                //var bcp = sender as SqlBulkCopy;
                //if (bcp != null)
                //    bcp.Close();// 不能在这里调用Close事件，会异常
            }
            if (_viewItem != null)
            {
                SetListViewText(_viewItem, 2,
                    "已同步:" + args.RowsCopied.ToString("N0") + "条" + (canceled ? " 已中止" : ""),
                    Color.Black);
            }
        }

        private delegate void SetListViewTextDele(ListViewItem item, int col, string text, Color color);
        void SetListViewText(ListViewItem item, int col, string text, Color color)
        {
            if (item == null || item.ListView == null)
                return;
            if (item.ListView.InvokeRequired)
            {
                SetListViewTextDele de = SetListViewText;
                Invoke(de, item, col, text, color);
            }
            else
            {
                item.SubItems[col].Text = text;
                item.SubItems[col].ForeColor = color;
            }
        }

        private delegate void ListViewAddRowDele(ListView lv, ListViewItem item);
        void ListViewAddRow(ListView lv, ListViewItem item)
        {
            if (lv == null || item == null)
                return;
            if (lv.InvokeRequired)
            {
                ListViewAddRowDele de = ListViewAddRow;
                Invoke(de, lv, item);
            }
            else
            {
                lv.Items.Add(item);
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            //base.OnClosing(e);
            if (!canceled && !finished &&
                MessageBox.Show("尚未同步完成，是否确认中止退出？", "未同步完成", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                canceled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (!canceled && !finished &&
                MessageBox.Show("尚未同步完成，是否确认中止退出？", "未同步完成", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                canceled = true;
            }
        }

        private void SyncForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)// 27表示ESC
            {
                if (!canceled && !finished)
                    btnCancel_Click(sender, e);
                else
                    btnClose_Click(sender, e);
            }
        }
    }

    [DataContract]
    public class SyncItem
    {
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 0)]
        public string Source { get; set; }
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 1)]
        public string Target { get; set; }
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 2, Name = "IsSql")]
        public bool IsSqlSource { get; set; }
    }

    [DataContract]
    public class SyncTask
    {
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 0)]
        public string SourceConstr { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 1)]
        public string TargetConstr { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 2)]
        public bool TruncateOld { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 3)]
        public bool UseIdentifier { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 4)]
        public bool ErrContinue { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 5)]
        public IEnumerable<SyncItem> Items { get; set; }
    }
}
