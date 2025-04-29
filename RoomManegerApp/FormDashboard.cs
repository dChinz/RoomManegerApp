using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoomManegerApp.Forms
{
    public partial class FormDashboard : Form
    {
        public FormDashboard()
        {
            InitializeComponent();
        }

        private void phòngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormRooms rooms = new FormRooms();
            rooms.MdiParent = this;
            rooms.Dock = DockStyle.Fill;
            rooms.Show();
        }

        private void kháchThuêToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormTenant rooms = new FormTenant();
            rooms.MdiParent = this;
            rooms.Dock = DockStyle.Fill;
            rooms.Show();
        }

        private void điệnNướcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormRoomDeatils form = new FormRoomDeatils();
            form.MdiParent = this;
            form.Dock = DockStyle.Fill;
            form.Show();
        }

        private void hóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormBills form = new FormBills();
            form.MdiParent = this;
            form.Dock = DockStyle.Fill;
            form.Show();
        }

        private void hợpĐồngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCheck_in form = new FormCheck_in();
            form.MdiParent = this;
            form.Dock = DockStyle.Fill;
            form.Show();
        }
    }
}
