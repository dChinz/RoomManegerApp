namespace RoomManegerApp.Forms
{
    partial class FormBills
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.check_in_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_days = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonAdd_new = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.đãThanhToánToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inHóaĐơnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.check_in_id,
            this.total_days,
            this.rent,
            this.total,
            this.status,
            this.note});
            this.tableLayoutPanel1.SetColumnSpan(this.dataGridView1, 2);
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 48);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(794, 399);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDown);
            // 
            // id
            // 
            this.id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Width = 40;
            // 
            // check_in_id
            // 
            this.check_in_id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.check_in_id.HeaderText = "Mã Check In";
            this.check_in_id.Name = "check_in_id";
            this.check_in_id.ReadOnly = true;
            // 
            // total_days
            // 
            this.total_days.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.total_days.HeaderText = "Số ngày";
            this.total_days.Name = "total_days";
            this.total_days.ReadOnly = true;
            // 
            // rent
            // 
            this.rent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.rent.HeaderText = "Giá thuê";
            this.rent.Name = "rent";
            this.rent.ReadOnly = true;
            // 
            // total
            // 
            this.total.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.total.HeaderText = "Tổng tiền";
            this.total.Name = "total";
            this.total.ReadOnly = true;
            // 
            // status
            // 
            this.status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.status.HeaderText = "Tình trạng";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            // 
            // note
            // 
            this.note.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.note.HeaderText = "Ghi chú";
            this.note.Name = "note";
            this.note.ReadOnly = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.buttonAdd_new);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(394, 39);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // buttonAdd_new
            // 
            this.buttonAdd_new.Location = new System.Drawing.Point(3, 3);
            this.buttonAdd_new.Name = "buttonAdd_new";
            this.buttonAdd_new.Size = new System.Drawing.Size(83, 36);
            this.buttonAdd_new.TabIndex = 0;
            this.buttonAdd_new.Text = "Tạo mới";
            this.buttonAdd_new.UseVisualStyleBackColor = true;
            this.buttonAdd_new.Click += new System.EventHandler(this.buttonAdd_new_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.đãThanhToánToolStripMenuItem,
            this.inHóaĐơnToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 70);
            // 
            // đãThanhToánToolStripMenuItem
            // 
            this.đãThanhToánToolStripMenuItem.Name = "đãThanhToánToolStripMenuItem";
            this.đãThanhToánToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.đãThanhToánToolStripMenuItem.Text = "Đã thanh toán";
            this.đãThanhToánToolStripMenuItem.Click += new System.EventHandler(this.đãThanhToánToolStripMenuItem_Click);
            // 
            // inHóaĐơnToolStripMenuItem
            // 
            this.inHóaĐơnToolStripMenuItem.Name = "inHóaĐơnToolStripMenuItem";
            this.inHóaĐơnToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.inHóaĐơnToolStripMenuItem.Text = "In hóa đơn";
            this.inHóaĐơnToolStripMenuItem.Click += new System.EventHandler(this.inHóaĐơnToolStripMenuItem_Click);
            // 
            // FormBills
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormBills";
            this.Text = "FormBills";
            this.Load += new System.EventHandler(this.FormBills_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button buttonAdd_new;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem đãThanhToánToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn check_in_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_days;
        private System.Windows.Forms.DataGridViewTextBoxColumn rent;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn note;
        private System.Windows.Forms.ToolStripMenuItem inHóaĐơnToolStripMenuItem;
    }
}