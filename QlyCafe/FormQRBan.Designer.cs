namespace QlyCafe
{
    partial class FormQRBan
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
            this.lblBan = new System.Windows.Forms.Label();
            this.QR = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.QR)).BeginInit();
            this.SuspendLayout();
            // 
            // lblBan
            // 
            this.lblBan.AutoSize = true;
            this.lblBan.Location = new System.Drawing.Point(100, 192);
            this.lblBan.Name = "lblBan";
            this.lblBan.Size = new System.Drawing.Size(35, 13);
            this.lblBan.TabIndex = 0;
            this.lblBan.Text = "label1";
            // 
            // QR
            // 
            this.QR.Location = new System.Drawing.Point(304, 94);
            this.QR.Name = "QR";
            this.QR.Size = new System.Drawing.Size(415, 254);
            this.QR.TabIndex = 1;
            this.QR.TabStop = false;
            // 
            // FormQRBan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.QR);
            this.Controls.Add(this.lblBan);
            this.Name = "FormQRBan";
            this.Text = "FormQRBan";
            this.Load += new System.EventHandler(this.FormQRBan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.QR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBan;
        private System.Windows.Forms.PictureBox QR;
    }
}