namespace xServer.Forms
{
    partial class FrmOnJoin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOnJoin));
            this.ctxMainMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadExecuteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dropToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nativeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selfInjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visitWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hiddenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.removeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lstCommands = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ctxMainMenu.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctxMainMenu
            // 
            this.ctxMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addItemToolStripMenuItem,
            this.removeItemToolStripMenuItem,
            this.toolStripSeparator1,
            this.removeAllToolStripMenuItem});
            this.ctxMainMenu.Name = "ctxMainMenu";
            this.ctxMainMenu.Size = new System.Drawing.Size(153, 98);
            // 
            // addItemToolStripMenuItem
            // 
            this.addItemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadExecuteToolStripMenuItem,
            this.visitWebsiteToolStripMenuItem});
            this.addItemToolStripMenuItem.Image = global::xServer.Properties.Resources.application_add;
            this.addItemToolStripMenuItem.Name = "addItemToolStripMenuItem";
            this.addItemToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addItemToolStripMenuItem.Text = "Add Item";
            // 
            // downloadExecuteToolStripMenuItem
            // 
            this.downloadExecuteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dropToolStripMenuItem,
            this.nativeToolStripMenuItem,
            this.selfInjectToolStripMenuItem});
            this.downloadExecuteToolStripMenuItem.Image = global::xServer.Properties.Resources.download;
            this.downloadExecuteToolStripMenuItem.Name = "downloadExecuteToolStripMenuItem";
            this.downloadExecuteToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.downloadExecuteToolStripMenuItem.Text = "Download && Execute";
            // 
            // dropToolStripMenuItem
            // 
            this.dropToolStripMenuItem.Name = "dropToolStripMenuItem";
            this.dropToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.dropToolStripMenuItem.Text = "Drop";
            this.dropToolStripMenuItem.Click += new System.EventHandler(this.dropToolStripMenuItem_Click);
            // 
            // nativeToolStripMenuItem
            // 
            this.nativeToolStripMenuItem.Name = "nativeToolStripMenuItem";
            this.nativeToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.nativeToolStripMenuItem.Text = "Native";
            this.nativeToolStripMenuItem.Click += new System.EventHandler(this.nativeToolStripMenuItem_Click);
            // 
            // selfInjectToolStripMenuItem
            // 
            this.selfInjectToolStripMenuItem.Name = "selfInjectToolStripMenuItem";
            this.selfInjectToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.selfInjectToolStripMenuItem.Text = "Self Inject";
            this.selfInjectToolStripMenuItem.Click += new System.EventHandler(this.selfInjectToolStripMenuItem_Click);
            // 
            // visitWebsiteToolStripMenuItem
            // 
            this.visitWebsiteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.normalToolStripMenuItem,
            this.hiddenToolStripMenuItem});
            this.visitWebsiteToolStripMenuItem.Image = global::xServer.Properties.Resources.world_link;
            this.visitWebsiteToolStripMenuItem.Name = "visitWebsiteToolStripMenuItem";
            this.visitWebsiteToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.visitWebsiteToolStripMenuItem.Text = "Visit Website";
            // 
            // normalToolStripMenuItem
            // 
            this.normalToolStripMenuItem.Name = "normalToolStripMenuItem";
            this.normalToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.normalToolStripMenuItem.Text = "Normal";
            this.normalToolStripMenuItem.Click += new System.EventHandler(this.normalToolStripMenuItem_Click);
            // 
            // hiddenToolStripMenuItem
            // 
            this.hiddenToolStripMenuItem.Name = "hiddenToolStripMenuItem";
            this.hiddenToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.hiddenToolStripMenuItem.Text = "Hidden";
            this.hiddenToolStripMenuItem.Click += new System.EventHandler(this.hiddenToolStripMenuItem_Click);
            // 
            // removeItemToolStripMenuItem
            // 
            this.removeItemToolStripMenuItem.Image = global::xServer.Properties.Resources.delete;
            this.removeItemToolStripMenuItem.Name = "removeItemToolStripMenuItem";
            this.removeItemToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.removeItemToolStripMenuItem.Text = "Remove Item";
            this.removeItemToolStripMenuItem.Click += new System.EventHandler(this.removeItemToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // removeAllToolStripMenuItem
            // 
            this.removeAllToolStripMenuItem.Image = global::xServer.Properties.Resources.cancel;
            this.removeAllToolStripMenuItem.Name = "removeAllToolStripMenuItem";
            this.removeAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.removeAllToolStripMenuItem.Text = "Remove All";
            this.removeAllToolStripMenuItem.Click += new System.EventHandler(this.removeAllToolStripMenuItem_Click);
            // 
            // lstCommands
            // 
            this.lstCommands.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lstCommands.ContextMenuStrip = this.ctxMainMenu;
            this.lstCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstCommands.FullRowSelect = true;
            this.lstCommands.GridLines = true;
            this.lstCommands.Location = new System.Drawing.Point(3, 16);
            this.lstCommands.Name = "lstCommands";
            this.lstCommands.Size = new System.Drawing.Size(504, 263);
            this.lstCommands.TabIndex = 1;
            this.lstCommands.UseCompatibleStateImageBehavior = false;
            this.lstCommands.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Action";
            this.columnHeader1.Width = 153;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            this.columnHeader2.Width = 343;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lstCommands);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(510, 282);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Main";
            // 
            // FrmOnJoin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 306);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmOnJoin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "xRAT 2.0 - Join Commands";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmOnJoin_FormClosing);
            this.Load += new System.EventHandler(this.FrmOnJoin_Load);
            this.ctxMainMenu.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip ctxMainMenu;
        private System.Windows.Forms.ToolStripMenuItem addItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeItemToolStripMenuItem;
        private System.Windows.Forms.ListView lstCommands;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripMenuItem downloadExecuteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dropToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nativeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selfInjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visitWebsiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hiddenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem removeAllToolStripMenuItem;
    }
}