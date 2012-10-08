namespace Beinet.cn.DataSync
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.txtDbSource = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chkErrContinue = new System.Windows.Forms.CheckBox();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.chkIdentifier = new System.Windows.Forms.CheckBox();
            this.chkClear = new System.Windows.Forms.CheckBox();
            this.btnDelRow = new System.Windows.Forms.Button();
            this.btnAddNewSql = new System.Windows.Forms.Button();
            this.btnGetSchma = new System.Windows.Forms.Button();
            this.btnSyncBegin = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDbTarget = new System.Windows.Forms.TextBox();
            this.lstTarget = new System.Windows.Forms.ComboBox();
            this.lstSource = new System.Windows.Forms.ComboBox();
            this.lvTables = new System.Windows.Forms.ListView();
            this.colSource = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTarget = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imglstForLvTables = new System.Windows.Forms.ImageList(this.components);
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据源:";
            // 
            // txtDbSource
            // 
            this.txtDbSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDbSource.Location = new System.Drawing.Point(77, 6);
            this.txtDbSource.Name = "txtDbSource";
            this.txtDbSource.Size = new System.Drawing.Size(465, 21);
            this.txtDbSource.TabIndex = 1;
            this.txtDbSource.Text = "server=192.168.19.63;database=newresourcedb;uid=mobileuser;pwd=mobileuserpws";
            this.txtDbSource.TextChanged += new System.EventHandler(this.txtDbSource_TextChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.CausesValidation = false;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chkErrContinue);
            this.splitContainer1.Panel1.Controls.Add(this.chkAll);
            this.splitContainer1.Panel1.Controls.Add(this.chkIdentifier);
            this.splitContainer1.Panel1.Controls.Add(this.chkClear);
            this.splitContainer1.Panel1.Controls.Add(this.btnDelRow);
            this.splitContainer1.Panel1.Controls.Add(this.btnAddNewSql);
            this.splitContainer1.Panel1.Controls.Add(this.btnGetSchma);
            this.splitContainer1.Panel1.Controls.Add(this.btnSaveConfig);
            this.splitContainer1.Panel1.Controls.Add(this.btnSyncBegin);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.txtDbTarget);
            this.splitContainer1.Panel1.Controls.Add(this.txtDbSource);
            this.splitContainer1.Panel1MinSize = 1;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lstTarget);
            this.splitContainer1.Panel2.Controls.Add(this.lstSource);
            this.splitContainer1.Panel2.Controls.Add(this.lvTables);
            this.splitContainer1.Panel2MinSize = 1;
            this.splitContainer1.Size = new System.Drawing.Size(676, 452);
            this.splitContainer1.SplitterDistance = 85;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // chkErrContinue
            // 
            this.chkErrContinue.AutoSize = true;
            this.chkErrContinue.Location = new System.Drawing.Point(528, 63);
            this.chkErrContinue.Name = "chkErrContinue";
            this.chkErrContinue.Size = new System.Drawing.Size(144, 16);
            this.chkErrContinue.TabIndex = 4;
            this.chkErrContinue.Text = "错误时继续同步后续表";
            this.chkErrContinue.UseVisualStyleBackColor = true;
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(112, 60);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(48, 16);
            this.chkAll.TabIndex = 4;
            this.chkAll.Text = "全选";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // chkIdentifier
            // 
            this.chkIdentifier.AutoSize = true;
            this.chkIdentifier.Location = new System.Drawing.Point(452, 63);
            this.chkIdentifier.Name = "chkIdentifier";
            this.chkIdentifier.Size = new System.Drawing.Size(72, 16);
            this.chkIdentifier.TabIndex = 4;
            this.chkIdentifier.Text = "标识插入";
            this.chkIdentifier.UseVisualStyleBackColor = true;
            // 
            // chkClear
            // 
            this.chkClear.AutoSize = true;
            this.chkClear.Location = new System.Drawing.Point(362, 63);
            this.chkClear.Name = "chkClear";
            this.chkClear.Size = new System.Drawing.Size(84, 16);
            this.chkClear.TabIndex = 4;
            this.chkClear.Text = "清空目标表";
            this.chkClear.UseVisualStyleBackColor = true;
            // 
            // btnDelRow
            // 
            this.btnDelRow.Enabled = false;
            this.btnDelRow.Location = new System.Drawing.Point(166, 56);
            this.btnDelRow.Name = "btnDelRow";
            this.btnDelRow.Size = new System.Drawing.Size(81, 23);
            this.btnDelRow.TabIndex = 3;
            this.btnDelRow.Text = "删除选定行";
            this.btnDelRow.UseVisualStyleBackColor = true;
            this.btnDelRow.Click += new System.EventHandler(this.btnDelRow_Click);
            // 
            // btnAddNewSql
            // 
            this.btnAddNewSql.Enabled = false;
            this.btnAddNewSql.Location = new System.Drawing.Point(250, 56);
            this.btnAddNewSql.Name = "btnAddNewSql";
            this.btnAddNewSql.Size = new System.Drawing.Size(87, 23);
            this.btnAddNewSql.TabIndex = 3;
            this.btnAddNewSql.Text = "新增查询同步";
            this.btnAddNewSql.UseVisualStyleBackColor = true;
            this.btnAddNewSql.Click += new System.EventHandler(this.btnAddNewSql_Click);
            // 
            // btnGetSchma
            // 
            this.btnGetSchma.Location = new System.Drawing.Point(14, 56);
            this.btnGetSchma.Name = "btnGetSchma";
            this.btnGetSchma.Size = new System.Drawing.Size(81, 23);
            this.btnGetSchma.TabIndex = 3;
            this.btnGetSchma.Text = "获取表结构";
            this.btnGetSchma.UseVisualStyleBackColor = true;
            this.btnGetSchma.Click += new System.EventHandler(this.btnGetSchma_Click);
            // 
            // btnSyncBegin
            // 
            this.btnSyncBegin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSyncBegin.Enabled = false;
            this.btnSyncBegin.Location = new System.Drawing.Point(616, 9);
            this.btnSyncBegin.Name = "btnSyncBegin";
            this.btnSyncBegin.Size = new System.Drawing.Size(56, 45);
            this.btnSyncBegin.TabIndex = 3;
            this.btnSyncBegin.Text = "直接开\r\n始同步";
            this.btnSyncBegin.UseVisualStyleBackColor = true;
            this.btnSyncBegin.Click += new System.EventHandler(this.btnSyncBegin_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "数据目标:";
            // 
            // txtDbTarget
            // 
            this.txtDbTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDbTarget.Location = new System.Drawing.Point(77, 33);
            this.txtDbTarget.Name = "txtDbTarget";
            this.txtDbTarget.Size = new System.Drawing.Size(465, 21);
            this.txtDbTarget.TabIndex = 2;
            this.txtDbTarget.Text = "server=192.168.19.63;database=sitepv;uid=mobileuser;pwd=mobileuserpws";
            this.txtDbTarget.TextChanged += new System.EventHandler(this.txtDbSource_TextChanged);
            // 
            // lstTarget
            // 
            this.lstTarget.DropDownHeight = 1060;
            this.lstTarget.FormattingEnabled = true;
            this.lstTarget.IntegralHeight = false;
            this.lstTarget.Location = new System.Drawing.Point(250, 185);
            this.lstTarget.Name = "lstTarget";
            this.lstTarget.Size = new System.Drawing.Size(121, 20);
            this.lstTarget.TabIndex = 2;
            this.lstTarget.Visible = false;
            this.lstTarget.Leave += new System.EventHandler(this.lstLeave);
            // 
            // lstSource
            // 
            this.lstSource.DropDownHeight = 1060;
            this.lstSource.FormattingEnabled = true;
            this.lstSource.IntegralHeight = false;
            this.lstSource.Location = new System.Drawing.Point(39, 107);
            this.lstSource.Name = "lstSource";
            this.lstSource.Size = new System.Drawing.Size(121, 20);
            this.lstSource.TabIndex = 1;
            this.lstSource.Visible = false;
            // 
            // lvTables
            // 
            this.lvTables.CheckBoxes = true;
            this.lvTables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSource,
            this.colTarget});
            this.lvTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvTables.FullRowSelect = true;
            this.lvTables.GridLines = true;
            this.lvTables.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvTables.HideSelection = false;
            this.lvTables.Location = new System.Drawing.Point(0, 0);
            this.lvTables.Margin = new System.Windows.Forms.Padding(0);
            this.lvTables.MultiSelect = false;
            this.lvTables.Name = "lvTables";
            this.lvTables.Size = new System.Drawing.Size(676, 366);
            this.lvTables.SmallImageList = this.imglstForLvTables;
            this.lvTables.TabIndex = 0;
            this.lvTables.TabStop = false;
            this.lvTables.UseCompatibleStateImageBehavior = false;
            this.lvTables.View = System.Windows.Forms.View.Details;
            this.lvTables.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lvTables_DrawColumnHeader);
            this.lvTables.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvTables_ItemChecked);
            this.lvTables.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvTables_MouseUp);
            // 
            // colSource
            // 
            this.colSource.Text = "源表";
            this.colSource.Width = 300;
            // 
            // colTarget
            // 
            this.colTarget.Text = "目标表";
            this.colTarget.Width = 300;
            // 
            // imglstForLvTables
            // 
            this.imglstForLvTables.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imglstForLvTables.ImageSize = new System.Drawing.Size(1, 20);
            this.imglstForLvTables.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveConfig.Enabled = false;
            this.btnSaveConfig.Location = new System.Drawing.Point(548, 9);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(62, 45);
            this.btnSaveConfig.TabIndex = 3;
            this.btnSaveConfig.Text = "保存为\r\n配置文件";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 452);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Sql Server数据导入导出";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDbSource;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDbTarget;
        private System.Windows.Forms.Button btnGetSchma;
        private System.Windows.Forms.Button btnSyncBegin;
        private System.Windows.Forms.ListView lvTables;
        private System.Windows.Forms.ColumnHeader colSource;
        private System.Windows.Forms.ColumnHeader colTarget;
        private System.Windows.Forms.ComboBox lstTarget;
        private System.Windows.Forms.ComboBox lstSource;
        private System.Windows.Forms.ImageList imglstForLvTables;
        private System.Windows.Forms.Button btnAddNewSql;
        private System.Windows.Forms.Button btnDelRow;
        private System.Windows.Forms.CheckBox chkIdentifier;
        private System.Windows.Forms.CheckBox chkClear;
        private System.Windows.Forms.CheckBox chkErrContinue;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.Button btnSaveConfig;
    }
}

