﻿namespace RoomManegerApp.Contracts
{
    partial class FormAdd_check_in
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
            this.buttonCapnhat = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.labelNameRoom = new System.Windows.Forms.Label();
            this.labelTypeRoom = new System.Windows.Forms.Label();
            this.labelStart_date = new System.Windows.Forms.Label();
            this.labelGuestname = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonCapnhat
            // 
            this.buttonCapnhat.Location = new System.Drawing.Point(155, 248);
            this.buttonCapnhat.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCapnhat.Name = "buttonCapnhat";
            this.buttonCapnhat.Size = new System.Drawing.Size(61, 24);
            this.buttonCapnhat.TabIndex = 34;
            this.buttonCapnhat.Text = "Thêm";
            this.buttonCapnhat.UseVisualStyleBackColor = true;
            this.buttonCapnhat.Click += new System.EventHandler(this.buttonCapnhat_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(54, 147);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 32;
            this.label6.Text = "Ngày kết thúc";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(54, 112);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "Ngày bắt đầu";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(60, 79);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Người thuê";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(61, 46);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Phòng";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(133, 147);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker2.TabIndex = 38;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(60, 186);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 39;
            this.label7.Text = "Loại phòng";
            // 
            // labelNameRoom
            // 
            this.labelNameRoom.AutoSize = true;
            this.labelNameRoom.Location = new System.Drawing.Point(130, 46);
            this.labelNameRoom.Name = "labelNameRoom";
            this.labelNameRoom.Size = new System.Drawing.Size(35, 13);
            this.labelNameRoom.TabIndex = 41;
            this.labelNameRoom.Text = "label1";
            // 
            // labelTypeRoom
            // 
            this.labelTypeRoom.AutoSize = true;
            this.labelTypeRoom.Location = new System.Drawing.Point(130, 186);
            this.labelTypeRoom.Name = "labelTypeRoom";
            this.labelTypeRoom.Size = new System.Drawing.Size(35, 13);
            this.labelTypeRoom.TabIndex = 42;
            this.labelTypeRoom.Text = "label1";
            // 
            // labelStart_date
            // 
            this.labelStart_date.AutoSize = true;
            this.labelStart_date.Location = new System.Drawing.Point(131, 112);
            this.labelStart_date.Name = "labelStart_date";
            this.labelStart_date.Size = new System.Drawing.Size(35, 13);
            this.labelStart_date.TabIndex = 43;
            this.labelStart_date.Text = "label1";
            // 
            // labelGuestname
            // 
            this.labelGuestname.AutoSize = true;
            this.labelGuestname.Location = new System.Drawing.Point(131, 79);
            this.labelGuestname.Name = "labelGuestname";
            this.labelGuestname.Size = new System.Drawing.Size(35, 13);
            this.labelGuestname.TabIndex = 44;
            this.labelGuestname.Text = "label1";
            // 
            // FormAdd_check_in
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 331);
            this.Controls.Add(this.labelGuestname);
            this.Controls.Add(this.labelStart_date);
            this.Controls.Add(this.labelTypeRoom);
            this.Controls.Add(this.labelNameRoom);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.buttonCapnhat);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Name = "FormAdd_check_in";
            this.Text = "FormAdd_check_in";
            this.Load += new System.EventHandler(this.FormAdd_contract_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonCapnhat;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelNameRoom;
        private System.Windows.Forms.Label labelTypeRoom;
        private System.Windows.Forms.Label labelStart_date;
        private System.Windows.Forms.Label labelGuestname;
    }
}