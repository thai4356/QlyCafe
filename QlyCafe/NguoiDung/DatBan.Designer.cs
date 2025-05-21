namespace QlyCafe
{
    partial class DatBan
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
            this.ban = new System.Windows.Forms.FlowLayoutPanel();
            this.btnBan = new System.Windows.Forms.Button();
            this.lblBanDangChon = new System.Windows.Forms.Label();
            this.btnTraBan = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ban
            // 
            this.ban.Location = new System.Drawing.Point(63, 35);
            this.ban.Name = "ban";
            this.ban.Size = new System.Drawing.Size(444, 378);
            this.ban.TabIndex = 0;
            // 
            // btnBan
            // 
            this.btnBan.Location = new System.Drawing.Point(529, 388);
            this.btnBan.Name = "btnBan";
            this.btnBan.Size = new System.Drawing.Size(259, 25);
            this.btnBan.TabIndex = 1;
            this.btnBan.Text = "Dat ban";
            this.btnBan.UseVisualStyleBackColor = true;
            this.btnBan.Click += new System.EventHandler(this.btnBan_Click);
            // 
            // lblBanDangChon
            // 
            this.lblBanDangChon.AutoSize = true;
            this.lblBanDangChon.Location = new System.Drawing.Point(546, 108);
            this.lblBanDangChon.Name = "lblBanDangChon";
            this.lblBanDangChon.Size = new System.Drawing.Size(0, 13);
            this.lblBanDangChon.TabIndex = 2;
            // 
            // btnTraBan
            // 
            this.btnTraBan.Location = new System.Drawing.Point(529, 336);
            this.btnTraBan.Name = "btnTraBan";
            this.btnTraBan.Size = new System.Drawing.Size(259, 25);
            this.btnTraBan.TabIndex = 3;
            this.btnTraBan.Text = "Tra Ban";
            this.btnTraBan.UseVisualStyleBackColor = true;
            // 
            // DatBan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnTraBan);
            this.Controls.Add(this.lblBanDangChon);
            this.Controls.Add(this.btnBan);
            this.Controls.Add(this.ban);
            this.Name = "DatBan";
            this.Text = "DatBan";
            this.Load += new System.EventHandler(this.DatBan_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel ban;
        private System.Windows.Forms.Button btnBan;
        private System.Windows.Forms.Label lblBanDangChon;
        private System.Windows.Forms.Button btnTraBan;
    }
}