using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoomManegerApp.Users
{
    public partial class FormAddUser : Form
    {
        public FormAddUser()
        {
            InitializeComponent();
        }

        private void FormAddUser_Load(object sender, EventArgs e)
        {
            
        }

        private void buttonTao_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;
            string role = comboBoxRole.Text;

            try
            {
                string sql = @"insert into users (username, password, role) values (@username, @password, @role)";
                int rowAffected = Convert.ToInt32(Database_connect.ExecuteNonQuery(sql, new Dictionary<string, object>
                {
                    {"@username", username },
                    { "@password", password },
                    {"@role", role }
                }));
                if (rowAffected > 0)
                {
                    MessageBox.Show("Thêm người dùng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đã xảy ra " + ex.Message,"Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
    }
}
