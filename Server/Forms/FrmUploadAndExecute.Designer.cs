namespace xServer.Forms
{
    partial class FrmUploadAndExecute
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUploadAndExecute));
            this.btnUploadAndExecute = new System.Windows.Forms.Button();
            this.chkRunHidden = new System.Windows.Forms.CheckBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbSelfInjection = new System.Windows.Forms.RadioButton();
            this.rbNative = new System.Windows.Forms.RadioButton();
            this.rbDrop = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUploadAndExecute
            // 
            this.btnUploadAndExecute.Location = new System.Drawing.Point(6, 21);
            this.btnUploadAndExecute.Name = "btnUploadAndExecute";
            this.btnUploadAndExecute.Size = new System.Drawing.Size(111, 23);
            this.btnUploadAndExecute.TabIndex = 4;
            this.btnUploadAndExecute.Text = "Upload && Execute";
            this.btnUploadAndExecute.UseVisualStyleBackColor = true;
            this.btnUploadAndExecute.Click += new System.EventHandler(this.btnUploadAndExecute_Click);
            // 
            // chkRunHidden
            // 
            this.chkRunHidden.AutoSize = true;
            this.chkRunHidden.Location = new System.Drawing.Point(325, 21);
            this.chkRunHidden.Name = "chkRunHidden";
            this.chkRunHidden.Size = new System.Drawing.Size(106, 17);
            this.chkRunHidden.TabIndex = 2;
            this.chkRunHidden.Text = "Run file hidden";
            this.chkRunHidden.UseVisualStyleBackColor = true;
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(6, 21);
            this.txtPath.MaxLength = 300;
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(333, 22);
            this.txtPath.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(345, 19);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(83, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbDrop);
            this.groupBox1.Controls.Add(this.rbNative);
            this.groupBox1.Controls.Add(this.rbSelfInjection);
            this.groupBox1.Controls.Add(this.chkRunHidden);
            this.groupBox1.Location = new System.Drawing.Point(12, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(437, 97);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtPath);
            this.groupBox2.Controls.Add(this.btnBrowse);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(437, 52);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Path";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnUploadAndExecute);
            this.groupBox3.Location = new System.Drawing.Point(12, 173);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(437, 60);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Controls";
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
            // FrmUploadAndExecute
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
            this.Name = "FrmUploadAndExecute";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "xRAT 2.0 - Upload & Execute []";
            this.Load += new System.EventHandler(this.FrmUploadAndExecute_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUploadAndExecute;
        private System.Windows.Forms.CheckBox chkRunHidden;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbDrop;
        private System.Windows.Forms.RadioButton rbNative;
        private System.Windows.Forms.RadioButton rbSelfInjection;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}