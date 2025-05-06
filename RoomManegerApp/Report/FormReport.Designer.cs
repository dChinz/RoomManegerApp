namespace RoomManegerApp.Report
{
    partial class FormReport
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
            this.comboBoxReport = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.comboBoxTime = new System.Windows.Forms.ComboBox();
            this.comboBoxSelectTime = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxReport
            // 
            this.comboBoxReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxReport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxReport.FormattingEnabled = true;
            this.comboBoxReport.Items.AddRange(new object[] {
            "Báo cáo doanh thu",
            "Báo cáo công suất phòng",
            "Báo cáo đặt phòng",
            "Báo cáo khách hàng"});
            this.comboBoxReport.Location = new System.Drawing.Point(3, 10);
            this.comboBoxReport.Name = "comboBoxReport";
            this.comboBoxReport.Size = new System.Drawing.Size(108, 21);
            this.comboBoxReport.TabIndex = 0;
            this.comboBoxReport.SelectedIndexChanged += new System.EventHandler(this.comboBoxReport_SelectedIndexChanged);
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Báo cáo doanh thu",
            "",
            "",
            "Báo cáo công suất phòng",
            "",
            "",
            "Báo cáo đặt phòng",
            "",
            "",
            "Báo cáo khách hàng"});
            this.comboBox2.Location = new System.Drawing.Point(3, 37);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(108, 21);
            this.comboBox2.TabIndex = 2;
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "Báo cáo doanh thu",
            "",
            "",
            "Báo cáo công suất phòng",
            "",
            "",
            "Báo cáo đặt phòng",
            "",
            "",
            "Báo cáo khách hàng"});
            this.comboBox3.Location = new System.Drawing.Point(117, 37);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(108, 21);
            this.comboBox3.TabIndex = 3;
            // 
            // buttonCreate
            // 
            this.buttonCreate.Location = new System.Drawing.Point(345, 71);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(75, 23);
            this.buttonCreate.TabIndex = 4;
            this.buttonCreate.Text = "Tạo";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // reportViewer1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.reportViewer1, 10);
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportViewer1.DocumentMapWidth = 12;
            this.reportViewer1.Location = new System.Drawing.Point(3, 105);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(794, 342);
            this.reportViewer1.TabIndex = 5;
            // 
            // comboBoxTime
            // 
            this.comboBoxTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTime.FormattingEnabled = true;
            this.comboBoxTime.Items.AddRange(new object[] {
            "Tháng",
            "Quý",
            "Năm"});
            this.comboBoxTime.Location = new System.Drawing.Point(117, 10);
            this.comboBoxTime.Name = "comboBoxTime";
            this.comboBoxTime.Size = new System.Drawing.Size(108, 21);
            this.comboBoxTime.TabIndex = 6;
            this.comboBoxTime.SelectedIndexChanged += new System.EventHandler(this.ComboBoxTime_SelectedIndexChanged);
            // 
            // comboBoxSelectTime
            // 
            this.comboBoxSelectTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxSelectTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSelectTime.FormattingEnabled = true;
            this.comboBoxSelectTime.Location = new System.Drawing.Point(231, 10);
            this.comboBoxSelectTime.Name = "comboBoxSelectTime";
            this.comboBoxSelectTime.Size = new System.Drawing.Size(108, 21);
            this.comboBoxSelectTime.TabIndex = 7;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel1.Controls.Add(this.comboBoxReport, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxSelectTime, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxTime, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboBox3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBox2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.reportViewer1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonCreate, 3, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692307F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692307F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692307F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 76.92308F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // FormReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormReport";
            this.Text = "FormReport";
            this.Load += new System.EventHandler(this.FormReport_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxReport;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Button buttonCreate;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.ComboBox comboBoxTime;
        private System.Windows.Forms.ComboBox comboBoxSelectTime;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}