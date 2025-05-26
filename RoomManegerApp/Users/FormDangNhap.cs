using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
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
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void FormDangNhap_Load(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = true;
            this.AcceptButton = button1;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = !checkBox1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = @"select id, username, role from users where username = @username and password = @password";
            string username = textBoxUsername.Text.Trim();
            string password = textBoxPassword.Text.Trim();
            var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object>
            {
                {"@username", username},
                {"@password", password}
            });
            foreach(var row in data)
            {
                Session.UserId = Convert.ToInt32(row["id"]);
                Session.UserName = row["username"].ToString();
                Session.Role = row["role"].ToString();

                Forms.FormDashboard f = new Forms.FormDashboard();
                f.WindowState = FormWindowState.Maximized;
                this.Hide();
                f.ShowDialog();
                this.Close();
            }
            MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }
}
