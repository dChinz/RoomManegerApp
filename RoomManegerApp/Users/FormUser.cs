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
    public partial class FormUser : Form
    {
        public FormUser()
        {
            InitializeComponent();
        }
        private int pageCurrent = 1;
        private int pageSize = 25;
        private int totalPages = 0;
        private int totalRecords = 0;
        private void FormUser_Load(object sender, EventArgs e)
        {
            load_form();
        }

        private void load_form()
        {
            UpdatePaginationInfo();
            try
            {
                int offset = (pageCurrent - 1) * pageSize;
                string sql = @"select * from users limit @pageSize offset @offset";
                var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object>
                {
                    { "@pageSize", pageSize},
                    { "@offset", offset},
                });
                foreach (var row in data)
                {
                    dataGridView1.Rows.Add(row["id"], row["username"], row["password"], row["role"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra:\n" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            labelPageInfo.Text = $"Trang {pageCurrent}/{totalPages}";
        }

        private void UpdatePaginationInfo()
        {
            string sql = @"select count(*) from users";
            totalRecords = Convert.ToInt32(Database_connect.ExecuteScalar(sql));
            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            pageCurrent = 1;
            load_form();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if(pageCurrent > 1)
            {
                pageCurrent--;
                load_form();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(pageCurrent < totalPages)
            {
                pageCurrent++;
                load_form();
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            pageCurrent = totalPages;
            load_form();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormAddUser form = new FormAddUser();
            form.ShowDialog();
        }
    }
}
