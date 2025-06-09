namespace RoomManegerApp.Check_in
{
    partial class FormEditCheckin
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
            this.labelNameRoom = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelGuestname = new System.Windows.Forms.Label();
            this.buttonCapnhat = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dateTimePickerCheckin = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerCheckout = new System.Windows.Forms.DateTimePicker();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelNameRoom
            // 
            this.labelNameRoom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelNameRoom.AutoSize = true;
            this.labelNameRoom.Location = new System.Drawing.Point(186, 65);
            this.labelNameRoom.Name = "labelNameRoom";
            this.labelNameRoom.Size = new System.Drawing.Size(35, 13);
            this.labelNameRoom.TabIndex = 41;
            this.labelNameRoom.Text = "label1";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(123, 65);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Phòng";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(102, 113);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Người thuê";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(89, 161);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "Ngày bắt đầu";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(87, 209);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 32;
            this.label6.Text = "Ngày kết thúc";
            // 
            // labelGuestname
            // 
            this.labelGuestname.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelGuestname.AutoSize = true;
            this.labelGuestname.Location = new System.Drawing.Point(186, 113);
            this.labelGuestname.Name = "labelGuestname";
            this.labelGuestname.Size = new System.Drawing.Size(35, 13);
            this.labelGuestname.TabIndex = 44;
            this.labelGuestname.Text = "label1";
            // 
            // buttonCapnhat
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.buttonCapnhat, 2);
            this.buttonCapnhat.Location = new System.Drawing.Point(165, 290);
            this.buttonCapnhat.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCapnhat.Name = "buttonCapnhat";
            this.buttonCapnhat.Size = new System.Drawing.Size(61, 24);
            this.buttonCapnhat.TabIndex = 34;
            this.buttonCapnhat.Text = "Cập nhật";
            this.buttonCapnhat.UseVisualStyleBackColor = true;
            this.buttonCapnhat.Click += new System.EventHandler(this.buttonCapnhat_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel1.Controls.Add(this.labelNameRoom, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelGuestname, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.dateTimePickerCheckin, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.dateTimePickerCheckout, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.buttonCapnhat, 1, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(409, 388);
            this.tableLayoutPanel1.TabIndex = 53;
            // 
            // dateTimePickerCheckin
            // 
            this.dateTimePickerCheckin.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dateTimePickerCheckin.Location = new System.Drawing.Point(186, 158);
            this.dateTimePickerCheckin.Name = "dateTimePickerCheckin";
            this.dateTimePickerCheckin.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerCheckin.TabIndex = 53;
            // 
            // dateTimePickerCheckout
            // 
            this.dateTimePickerCheckout.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dateTimePickerCheckout.Location = new System.Drawing.Point(186, 206);
            this.dateTimePickerCheckout.Name = "dateTimePickerCheckout";
            this.dateTimePickerCheckout.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerCheckout.TabIndex = 54;
            // 
            // FormEditCheckin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 388);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormEditCheckin";
            this.Text = "FormEditCheckin";
            this.Load += new System.EventHandler(this.FormEditCheckin_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelNameRoom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelGuestname;
        private System.Windows.Forms.Button buttonCapnhat;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DateTimePicker dateTimePickerCheckin;
        private System.Windows.Forms.DateTimePicker dateTimePickerCheckout;
    }
}