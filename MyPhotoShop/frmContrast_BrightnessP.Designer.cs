namespace MyPhotoShop
{
    partial class frmContrast_BrightnessP
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
            this.label1 = new System.Windows.Forms.Label();
            this.skContrast = new CCWin.SkinControl.SkinTrackBar();
            this.txtContrastValue = new System.Windows.Forms.TextBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.txtBrightnessPValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.skBrightnessP = new CCWin.SkinControl.SkinTrackBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAutomatic = new System.Windows.Forms.Button();
            this.chkPreview = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.skContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.skBrightnessP)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "亮度：";
            // 
            // skContrast
            // 
            this.skContrast.BackColor = System.Drawing.Color.Transparent;
            this.skContrast.Bar = global::MyPhotoShop.Properties.Resources.Bar1;
            this.skContrast.BarStyle = CCWin.SkinControl.HSLTrackBarStyle.Img;
            this.skContrast.Location = new System.Drawing.Point(15, 16);
            this.skContrast.Maximum = 150;
            this.skContrast.Minimum = -150;
            this.skContrast.Name = "skContrast";
            this.skContrast.Size = new System.Drawing.Size(191, 45);
            this.skContrast.TabIndex = 1;
            this.skContrast.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.skContrast.Track = global::MyPhotoShop.Properties.Resources.Track12;
            this.skContrast.Scroll += new System.EventHandler(this.skContrast_Scroll);
            // 
            // txtContrastValue
            // 
            this.txtContrastValue.Location = new System.Drawing.Point(147, 10);
            this.txtContrastValue.Name = "txtContrastValue";
            this.txtContrastValue.Size = new System.Drawing.Size(59, 21);
            this.txtContrastValue.TabIndex = 2;
            this.txtContrastValue.TextChanged += new System.EventHandler(this.txtContrastValue_TextChanged);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(226, 10);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(91, 21);
            this.btnConfirm.TabIndex = 3;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // txtBrightnessPValue
            // 
            this.txtBrightnessPValue.Location = new System.Drawing.Point(147, 59);
            this.txtBrightnessPValue.Name = "txtBrightnessPValue";
            this.txtBrightnessPValue.Size = new System.Drawing.Size(59, 21);
            this.txtBrightnessPValue.TabIndex = 6;
            this.txtBrightnessPValue.TextChanged += new System.EventHandler(this.txtBrightnessPValue_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "对比度：";
            // 
            // skBrightnessP
            // 
            this.skBrightnessP.BackColor = System.Drawing.Color.Transparent;
            this.skBrightnessP.Bar = global::MyPhotoShop.Properties.Resources.Bar1;
            this.skBrightnessP.BarStyle = CCWin.SkinControl.HSLTrackBarStyle.Img;
            this.skBrightnessP.Location = new System.Drawing.Point(15, 64);
            this.skBrightnessP.Minimum = -100;
            this.skBrightnessP.Name = "skBrightnessP";
            this.skBrightnessP.Size = new System.Drawing.Size(191, 45);
            this.skBrightnessP.TabIndex = 5;
            this.skBrightnessP.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.skBrightnessP.Track = global::MyPhotoShop.Properties.Resources.Track12;
            this.skBrightnessP.Scroll += new System.EventHandler(this.skBrightnessP_Scroll);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(226, 45);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(91, 21);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAutomatic
            // 
            this.btnAutomatic.Location = new System.Drawing.Point(226, 81);
            this.btnAutomatic.Name = "btnAutomatic";
            this.btnAutomatic.Size = new System.Drawing.Size(91, 21);
            this.btnAutomatic.TabIndex = 3;
            this.btnAutomatic.Text = "自动";
            this.btnAutomatic.UseVisualStyleBackColor = true;
            // 
            // chkPreview
            // 
            this.chkPreview.AutoSize = true;
            this.chkPreview.Location = new System.Drawing.Point(226, 108);
            this.chkPreview.Name = "chkPreview";
            this.chkPreview.Size = new System.Drawing.Size(54, 16);
            this.chkPreview.TabIndex = 7;
            this.chkPreview.Text = "预览&P";
            this.chkPreview.UseVisualStyleBackColor = true;
            this.chkPreview.CheckedChanged += new System.EventHandler(this.chkPreview_CheckedChanged);
            // 
            // frmContrast_BrightnessP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 128);
            this.Controls.Add(this.chkPreview);
            this.Controls.Add(this.txtBrightnessPValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.skBrightnessP);
            this.Controls.Add(this.btnAutomatic);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.txtContrastValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.skContrast);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmContrast_BrightnessP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "亮度对比度";
            this.Load += new System.EventHandler(this.frmContrast_BrightnessP_Load);
            ((System.ComponentModel.ISupportInitialize)(this.skContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.skBrightnessP)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private CCWin.SkinControl.SkinTrackBar skContrast;
        private System.Windows.Forms.TextBox txtContrastValue;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.TextBox txtBrightnessPValue;
        private System.Windows.Forms.Label label2;
        private CCWin.SkinControl.SkinTrackBar skBrightnessP;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAutomatic;
        private System.Windows.Forms.CheckBox chkPreview;
    }
}