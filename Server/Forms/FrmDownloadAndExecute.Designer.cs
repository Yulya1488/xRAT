namespace xServer.Forms
{
    partial class FrmDownloadAndExecute
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDownloadAndExecute));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnDownloadAndExecute = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkRunHidden = new System.Windows.Forms.CheckBox();
            this.rbDrop = new System.Windows.Forms.RadioButton();
            this.rbNative = new System.Windows.Forms.RadioButton();
            this.rbSelfInjection = new System.Windows.Forms.RadioButton();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnDownloadAndExecute);
            this.groupBox3.Location = new System.Drawing.Point(12, 173);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(437, 60);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Controls";
            // 
            // btnDownloadAndExecute
            // 
            this.btnDownloadAndExecute.Location = new System.Drawing.Point(6, 21);
            this.btnDownloadAndExecute.Name = "btnDownloadAndExecute";
            this.btnDownloadAndExecute.Size = new System.Drawing.Size(125, 23);
            this.btnDownloadAndExecute.TabIndex = 4;
            this.btnDownloadAndExecute.Text = "Download && Execute";
            this.btnDownloadAndExecute.UseVisualStyleBackColor = true;
            this.btnDownloadAndExecute.Click += new System.EventHandler(this.btnDownloadAndExecute_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtURL);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(437, 52);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Direct URL";
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(6, 21);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(425, 22);
            this.txtURL.TabIndex = 4;
            this.txtURL.Text = "http://example.com/file.exe";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkRunHidden);
            this.groupBox1.Controls.Add(this.rbDrop);
            this.groupBox1.Controls.Add(this.rbNative);
            this.groupBox1.Controls.Add(this.rbSelfInjection);
            this.groupBox1.Location = new System.Drawing.Point(12, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(437, 97);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // chkRunHidden
            // 
            this.chkRunHidden.AutoSize = true;
            this.chkRunHidden.Location = new System.Drawing.Point(325, 21);
            this.chkRunHidden.Name = "chkRunHidden";
            this.chkRunHidden.Size = new System.Drawing.Size(106, 17);
            this.chkRunHidden.TabIndex = 6;
            this.chkRunHidden.Text = "Run file hidden";
            this.chkRunHidden.UseVisualStyleBackColor = true;
            // 
            // rbDrop
            // 
            this.rbDrop.AutoSize = true;
            this.rbDrop.Checked = true;
            this.rbDrop.Location = new System.Drawing.Point(6, 21);
            this.rbDrop.Name = "rbDrop";
            this.rbDrop.Size = new System.Drawing.Size(51, 17);
            this.rbDrop.TabIndex = 5;
            this.rbDrop.TabStop = true;
            this.rbDrop.Text = "Drop";
            this.rbDrop.UseVisualStyleBackColor = true;
            // 
            // rbNative
            // 
            this.rbNative.AutoSize = true;
            this.rbNative.Location = new System.Drawing.Point(6, 67);
            this.rbNative.Name = "rbNative";
            this.rbNative.Size = new System.Drawing.Size(57, 17);
            this.rbNative.TabIndex = 4;
            this.rbNative.Text = "Native";
            this.rbNative.UseVisualStyleBackColor = true;
            // 
            // rbSelfInjection
            // 
            this.rbSelfInjection.AutoSize = true;
            this.rbSelfInjection.Location = new System.Drawing.Point(6, 44);
            this.rbSelfInjection.Name = "rbSelfInjection";
            this.rbSelfInjection.Size = new System.Drawing.Size(92, 17);
            this.rbSelfInjection.TabIndex = 3;
            this.rbSelfInjection.Text = "Self Injection";
            this.rbSelfInjection.UseVisualStyleBackColor = true;
            // 
            // FrmDownloadAndExecute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 243);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDownloadAndExecute";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "xRAT 2.0 - Download & Execute []";
            this.Load += new System.EventHandler(this.FrmDownloadAndExecute_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnDownloadAndExecute;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkRunHidden;
        private System.Windows.Forms.RadioButton rbDrop;
        private System.Windows.Forms.RadioButton rbNative;
        private System.Windows.Forms.RadioButton rbSelfInjection;

    }
}