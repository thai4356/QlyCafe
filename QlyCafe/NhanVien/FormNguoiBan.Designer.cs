namespace QlyCafe
{
    partial class FormNguoiBan
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnDuyetDon = new System.Windows.Forms.Button();
            this.btnTuChoiDon = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(362, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "nguoi ban";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(143, 90);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(502, 273);
            this.dataGridView1.TabIndex = 1;
            // 
            // btnDuyetDon
            // 
            this.btnDuyetDon.Location = new System.Drawing.Point(143, 394);
            this.btnDuyetDon.Name = "btnDuyetDon";
            this.btnDuyetDon.Size = new System.Drawing.Size(183, 23);
            this.btnDuyetDon.TabIndex = 2;
            this.btnDuyetDon.Text = "Dong y va luu";
            this.btnDuyetDon.UseVisualStyleBackColor = true;
            this.btnDuyetDon.Click += new System.EventHandler(this.btnDuyetDon_Click);
            // 
            // btnTuChoiDon
            // 
            this.btnTuChoiDon.Location = new System.Drawing.Point(462, 394);
            this.btnTuChoiDon.Name = "btnTuChoiDon";
            this.btnTuChoiDon.Size = new System.Drawing.Size(183, 23);
            this.btnTuChoiDon.TabIndex = 3;
            this.btnTuChoiDon.Text = "Huy don";
            this.btnTuChoiDon.UseVisualStyleBackColor = true;
            this.btnTuChoiDon.Click += new System.EventHandler(this.btnTuChoiDon_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Location = new System.Drawing.Point(683, 204);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnLamMoi.Size = new System.Drawing.Size(75, 23);
            this.btnLamMoi.TabIndex = 4;
            this.btnLamMoi.Text = "Lam Moi";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // FormNguoiBan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnLamMoi);
            this.Controls.Add(this.btnTuChoiDon);
            this.Controls.Add(this.btnDuyetDon);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Name = "FormNguoiBan";
            this.Text = "FormNguoiBan";
            this.Load += new System.EventHandler(this.FormNguoiBan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnDuyetDon;
        private System.Windows.Forms.Button btnTuChoiDon;
        private System.Windows.Forms.Button btnLamMoi;
    }
}