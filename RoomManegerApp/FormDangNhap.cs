using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoomManegerApp
{
    public partial class FormDangNhap : Form
    {
        public FormDangNhap()
        {
            InitializeComponent();
        }

        bool status = false;

        private void FormDangNhap_Load(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = true;
        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = !checkBox1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxUsername.Text == "admin" && textBoxPassword.Text == "123456")
            {
                status = true;
            }
            if (status)
            {
                Forms.FormDashboard f = new Forms.FormDashboard();
                f.WindowState = FormWindowState.Maximized;
                this.Hide();
                f.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxUsername.Focus();
            }
        }
    }
}
