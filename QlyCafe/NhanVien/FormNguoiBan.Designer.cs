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
            this.btn_exit = new System.Windows.Forms.Button();
            this.txtCCCD = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(272, 39);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Thanh toan";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(43, 70);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(376, 222);
            this.dataGridView1.TabIndex = 1;
            // 
            // btnDuyetDon
            // 
            this.btnDuyetDon.Location = new System.Drawing.Point(43, 320);
            this.btnDuyetDon.Margin = new System.Windows.Forms.Padding(2);
            this.btnDuyetDon.Name = "btnDuyetDon";
            this.btnDuyetDon.Size = new System.Drawing.Size(148, 35);
            this.btnDuyetDon.TabIndex = 2;
            this.btnDuyetDon.Text = "Dong y va luu";
            this.btnDuyetDon.UseVisualStyleBackColor = true;
            this.btnDuyetDon.Click += new System.EventHandler(this.btnDuyetDon_Click);
            // 
            // btnTuChoiDon
            // 
            this.btnTuChoiDon.Location = new System.Drawing.Point(262, 320);
            this.btnTuChoiDon.Margin = new System.Windows.Forms.Padding(2);
            this.btnTuChoiDon.Name = "btnTuChoiDon";
            this.btnTuChoiDon.Size = new System.Drawing.Size(157, 35);
            this.btnTuChoiDon.TabIndex = 3;
            this.btnTuChoiDon.Text = "Huy don";
            this.btnTuChoiDon.UseVisualStyleBackColor = true;
            this.btnTuChoiDon.Click += new System.EventHandler(this.btnTuChoiDon_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Location = new System.Drawing.Point(471, 70);
            this.btnLamMoi.Margin = new System.Windows.Forms.Padding(2);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnLamMoi.Size = new System.Drawing.Size(76, 32);
            this.btnLamMoi.TabIndex = 4;
            this.btnLamMoi.Text = "Lam Moi";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // btn_exit
            // 
            this.btn_exit.Location = new System.Drawing.Point(16, 10);
            this.btn_exit.Margin = new System.Windows.Forms.Padding(2);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_exit.Size = new System.Drawing.Size(142, 42);
            this.btn_exit.TabIndex = 5;
            this.btn_exit.Text = "Dang xuat";
            this.btn_exit.UseVisualStyleBackColor = true;
            this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // txtCCCD
            // 
            this.txtCCCD.Location = new System.Drawing.Point(447, 235);
            this.txtCCCD.Name = "txtCCCD";
            this.txtCCCD.Size = new System.Drawing.Size(141, 20);
            this.txtCCCD.TabIndex = 6;
            this.txtCCCD.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(447, 201);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "nhap so hoi vien";
            // 
            // FormNguoiBan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 366);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCCCD);
            this.Controls.Add(this.btn_exit);
            this.Controls.Add(this.btnLamMoi);
            this.Controls.Add(this.btnTuChoiDon);
            this.Controls.Add(this.btnDuyetDon);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
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
        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.TextBox txtCCCD;
        private System.Windows.Forms.Label label2;
    }
}