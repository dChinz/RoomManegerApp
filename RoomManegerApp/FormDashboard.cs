using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RoomManegerApp.FormDangNhap;

namespace RoomManegerApp.Forms
{
    public partial class FormDashboard : Form
    {
        public FormDashboard()
        {
            InitializeComponent();
        }
        public Role UserRole { get; set; }

        private void phòngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(UserRole == Role.Admin || UserRole == Role.Manager)
            {
                FormRooms rooms = new FormRooms();
                rooms.MdiParent = this;
                rooms.Dock = DockStyle.Fill;
                rooms.Show();
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập khu vực này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void kháchThuêToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserRole == Role.Admin || UserRole == Role.Manager)
            {
                FormTenant rooms = new FormTenant();
                rooms.MdiParent = this;
                rooms.Dock = DockStyle.Fill;
                rooms.Show();
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập khu vực này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void checkInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormBills form = new FormBills();
            form.MdiParent = this;
            form.Dock = DockStyle.Fill;
            form.Show();
        }

        private void checkOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCheck_in form = new FormCheck_in();
            form.MdiParent = this;
            form.Dock = DockStyle.Fill;
            form.Show();
        }
    }
}
