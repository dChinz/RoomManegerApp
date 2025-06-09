namespace RoomManegerApp.Report
{
    partial class FormServiceDetails
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTenantName = new System.Windows.Forms.Label();
            this.labelRoomName = new System.Windows.Forms.Label();
            this.labelTotal = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(39, 73);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(652, 313);
            this.panel1.TabIndex = 0;
            // 
            // labelTenantName
            // 
            this.labelTenantName.AutoSize = true;
            this.labelTenantName.Location = new System.Drawing.Point(36, 9);
            this.labelTenantName.Name = "labelTenantName";
            this.labelTenantName.Size = new System.Drawing.Size(35, 13);
            this.labelTenantName.TabIndex = 1;
            this.labelTenantName.Text = "label1";
            // 
            // labelRoomName
            // 
            this.labelRoomName.AutoSize = true;
            this.labelRoomName.Location = new System.Drawing.Point(36, 34);
            this.labelRoomName.Name = "labelRoomName";
            this.labelRoomName.Size = new System.Drawing.Size(35, 13);
            this.labelRoomName.TabIndex = 2;
            this.labelRoomName.Text = "label2";
            // 
            // labelTotal
            // 
            this.labelTotal.AutoSize = true;
            this.labelTotal.Location = new System.Drawing.Point(36, 57);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(35, 13);
            this.labelTotal.TabIndex = 3;
            this.labelTotal.Text = "label1";
            // 
            // FormServiceDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.labelRoomName);
            this.Controls.Add(this.labelTenantName);
            this.Controls.Add(this.panel1);
            this.Name = "FormServiceDetails";
            this.Text = "FormServiceDetails";
            this.Load += new System.EventHandler(this.FormServiceDetails_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTenantName;
        private System.Windows.Forms.Label labelRoomName;
        private System.Windows.Forms.Label labelTotal;
    }
}