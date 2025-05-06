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
        }

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

        private void FormDangNhap_Load(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = true;
            this.AcceptButton = button1;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = !checkBox1.Checked;
        }

        public enum Role { Admin, Manager, Staff}
        public Role UserRole { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            sql = @"select role from users where username = @username and password = @password";
            string username = textBoxUsername.Text.Trim();
            string password = textBoxPassword.Text.Trim();
            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using(thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@username", username);
                    thuchien.Parameters.AddWithValue("@password", password);
                    using(doc = thuchien.ExecuteReader())
                    {
                        if (doc.Read())
                        {
                            string roleStr = doc["role"].ToString();
                            Role role = (Role)Enum.Parse(typeof(Role), roleStr, ignoreCase: true);
                            doc.Close();

                            Forms.FormDashboard f = new Forms.FormDashboard();
                            f.UserRole = role;
                            f.WindowState = FormWindowState.Maximized;
                            this.Hide();
                            f.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            doc.Close();
                            MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
        }
    }
}
