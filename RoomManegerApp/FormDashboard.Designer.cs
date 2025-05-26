namespace RoomManegerApp.Forms
{
    partial class FormDashboard
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelDangXuat = new System.Windows.Forms.Label();
            this.labelRole = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.QLDatPhong = new System.Windows.Forms.Button();
            this.booking = new System.Windows.Forms.Button();
            this.ThanhToan = new System.Windows.Forms.Button();
            this.BaoCao = new System.Windows.Forms.Button();
            this.QLKhachHang = new System.Windows.Forms.Button();
            this.QLPhong = new System.Windows.Forms.Button();
            this.QLUser = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.timerNow = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1067, 562);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 2);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel3, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1067, 56);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.flowLayoutPanel3.Controls.Add(this.labelDangXuat);
            this.flowLayoutPanel3.Controls.Add(this.labelRole);
            this.flowLayoutPanel3.Controls.Add(this.labelTime);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(710, 0);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(357, 56);
            this.flowLayoutPanel3.TabIndex = 1;
            // 
            // labelDangXuat
            // 
            this.labelDangXuat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDangXuat.AutoSize = true;
            this.labelDangXuat.Location = new System.Drawing.Point(282, 0);
            this.labelDangXuat.Name = "labelDangXuat";
            this.labelDangXuat.Size = new System.Drawing.Size(72, 17);
            this.labelDangXuat.TabIndex = 3;
            this.labelDangXuat.Text = "Đăng xuất";
            this.labelDangXuat.Click += new System.EventHandler(this.label4_Click);
            // 
            // labelRole
            // 
            this.labelRole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelRole.AutoSize = true;
            this.labelRole.Location = new System.Drawing.Point(230, 0);
            this.labelRole.Name = "labelRole";
            this.labelRole.Size = new System.Drawing.Size(46, 17);
            this.labelRole.TabIndex = 2;
            this.labelRole.Text = "label3";
            // 
            // labelTime
            // 
            this.labelTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(224, 0);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(0, 17);
            this.labelTime.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(385, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(294, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nesta Hotel Hanoi";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 56);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 67.56757F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 32.43243F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(150, 506);
            this.tableLayoutPanel3.TabIndex = 6;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoScroll = true;
            this.flowLayoutPanel2.BackColor = System.Drawing.Color.SkyBlue;
            this.flowLayoutPanel2.Controls.Add(this.QLDatPhong);
            this.flowLayoutPanel2.Controls.Add(this.booking);
            this.flowLayoutPanel2.Controls.Add(this.ThanhToan);
            this.flowLayoutPanel2.Controls.Add(this.BaoCao);
            this.flowLayoutPanel2.Controls.Add(this.QLKhachHang);
            this.flowLayoutPanel2.Controls.Add(this.QLPhong);
            this.flowLayoutPanel2.Controls.Add(this.QLUser);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.flowLayoutPanel2.Size = new System.Drawing.Size(150, 341);
            this.flowLayoutPanel2.TabIndex = 1;
            // 
            // QLDatPhong
            // 
            this.QLDatPhong.Location = new System.Drawing.Point(13, 20);
            this.QLDatPhong.Margin = new System.Windows.Forms.Padding(3, 20, 3, 20);
            this.QLDatPhong.Name = "QLDatPhong";
            this.QLDatPhong.Size = new System.Drawing.Size(125, 43);
            this.QLDatPhong.TabIndex = 1;
            this.QLDatPhong.Text = "Quản lí đặt phòng";
            this.QLDatPhong.UseVisualStyleBackColor = true;
            this.QLDatPhong.Click += new System.EventHandler(this.QLDatPhong_Click);
            // 
            // booking
            // 
            this.booking.Location = new System.Drawing.Point(13, 103);
            this.booking.Margin = new System.Windows.Forms.Padding(3, 20, 3, 20);
            this.booking.Name = "booking";
            this.booking.Size = new System.Drawing.Size(125, 43);
            this.booking.TabIndex = 5;
            this.booking.UseVisualStyleBackColor = true;
            this.booking.Click += new System.EventHandler(this.booking_Click);
            // 
            // ThanhToan
            // 
            this.ThanhToan.Location = new System.Drawing.Point(13, 186);
            this.ThanhToan.Margin = new System.Windows.Forms.Padding(3, 20, 3, 20);
            this.ThanhToan.Name = "ThanhToan";
            this.ThanhToan.Size = new System.Drawing.Size(125, 43);
            this.ThanhToan.TabIndex = 3;
            this.ThanhToan.Text = "Thanh toán";
            this.ThanhToan.UseVisualStyleBackColor = true;
            this.ThanhToan.Click += new System.EventHandler(this.ThanhToan_Click);
            // 
            // BaoCao
            // 
            this.BaoCao.Location = new System.Drawing.Point(13, 269);
            this.BaoCao.Margin = new System.Windows.Forms.Padding(3, 20, 3, 20);
            this.BaoCao.Name = "BaoCao";
            this.BaoCao.Size = new System.Drawing.Size(125, 43);
            this.BaoCao.TabIndex = 4;
            this.BaoCao.Text = "Báo cáo";
            this.BaoCao.UseVisualStyleBackColor = true;
            this.BaoCao.Click += new System.EventHandler(this.BaoCao_Click);
            // 
            // QLKhachHang
            // 
            this.QLKhachHang.Location = new System.Drawing.Point(13, 352);
            this.QLKhachHang.Margin = new System.Windows.Forms.Padding(3, 20, 3, 20);
            this.QLKhachHang.Name = "QLKhachHang";
            this.QLKhachHang.Size = new System.Drawing.Size(125, 43);
            this.QLKhachHang.TabIndex = 2;
            this.QLKhachHang.Text = "Quản lí khách hàng";
            this.QLKhachHang.UseVisualStyleBackColor = true;
            this.QLKhachHang.Click += new System.EventHandler(this.QLKhachHang_Click);
            // 
            // QLPhong
            // 
            this.QLPhong.BackColor = System.Drawing.SystemColors.ControlLight;
            this.QLPhong.Location = new System.Drawing.Point(13, 435);
            this.QLPhong.Margin = new System.Windows.Forms.Padding(3, 20, 3, 20);
            this.QLPhong.Name = "QLPhong";
            this.QLPhong.Size = new System.Drawing.Size(125, 43);
            this.QLPhong.TabIndex = 0;
            this.QLPhong.Text = "Quản lí phòng";
            this.QLPhong.UseVisualStyleBackColor = false;
            this.QLPhong.Click += new System.EventHandler(this.QLPhong_Click);
            // 
            // QLUser
            // 
            this.QLUser.Location = new System.Drawing.Point(13, 518);
            this.QLUser.Margin = new System.Windows.Forms.Padding(3, 20, 3, 20);
            this.QLUser.Name = "QLUser";
            this.QLUser.Size = new System.Drawing.Size(125, 43);
            this.QLUser.TabIndex = 6;
            this.QLUser.Text = "Quản lí người dùng";
            this.QLUser.UseVisualStyleBackColor = true;
            this.QLUser.Click += new System.EventHandler(this.QLUser_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Controls.Add(this.label4);
            this.flowLayoutPanel1.Controls.Add(this.label5);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 341);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(150, 165);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 46);
            this.label3.Margin = new System.Windows.Forms.Padding(10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(10, 82);
            this.label4.Margin = new System.Windows.Forms.Padding(10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "label4";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(10, 118);
            this.label5.Margin = new System.Windows.Forms.Padding(10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 16);
            this.label5.TabIndex = 3;
            this.label5.Text = "label5";
            // 
            // timerNow
            // 
            this.timerNow.Tick += new System.EventHandler(this.timerNow_Tick);
            // 
            // FormDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 562);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormDashboard";
            this.Text = "FormDashboard";
            this.Load += new System.EventHandler(this.FormDashboard_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button QLPhong;
        private System.Windows.Forms.Button QLDatPhong;
        private System.Windows.Forms.Button QLKhachHang;
        private System.Windows.Forms.Button ThanhToan;
        private System.Windows.Forms.Button BaoCao;
        private System.Windows.Forms.Timer timerNow;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Label labelDangXuat;
        private System.Windows.Forms.Label labelRole;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button booking;
        private System.Windows.Forms.Button QLUser;
    }
}