namespace QlyCafe
{
    partial class TableDetail
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
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.btnDatBan = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblBan
            // 
            this.lblBan.Location = new System.Drawing.Point(106, 49);
            this.lblBan.Name = "lblBan";
            this.lblBan.Size = new System.Drawing.Size(100, 23);
            this.lblBan.TabIndex = 0;
            this.lblBan.Text = "label1";
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Location = new System.Drawing.Point(106, 161);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(35, 13);
            this.lblTrangThai.TabIndex = 1;
            this.lblTrangThai.Text = "label2";
            // 
            // btnDatBan
            // 
            this.btnDatBan.Location = new System.Drawing.Point(313, 103);
            this.btnDatBan.Name = "btnDatBan";
            this.btnDatBan.Size = new System.Drawing.Size(75, 23);
            this.btnDatBan.TabIndex = 2;
            this.btnDatBan.Text = "Dat Ban";
            this.btnDatBan.UseVisualStyleBackColor = true;
            // 
            // TableDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 234);
            this.Controls.Add(this.btnDatBan);
            this.Controls.Add(this.lblTrangThai);
            this.Controls.Add(this.lblBan);
            this.Name = "TableDetail";
            this.Text = "TableDetail";
            this.Load += new System.EventHandler(this.TableDetail_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBan;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.Button btnDatBan;
    }
}