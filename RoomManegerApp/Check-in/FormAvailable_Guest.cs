using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RoomManegerApp.Contracts;
using RoomManegerApp.Tetants;

namespace RoomManegerApp.Check_in
{
    public partial class FormAvailable_Guest : Form
    {
        private string nameRoom;
        private string type;
        private string size;
        private Action _callback;
        private string checkin;
        private string checkout;
        public FormAvailable_Guest(string roomName, string roomType, string size, Action callback, string checkin, string checkout)
        {
            InitializeComponent();
            nameRoom = roomName;
            type = roomType;
            this.size = size;
            _callback = callback;
            this.checkin = checkin;
            this.checkout = checkout;
        }

        private int currentPage = 1;
        private int pageSize = 18;
        private int totalRecords = 0;
        private int totalPages = 0;

        private async void FormAvailable_Guest_Load(object sender, EventArgs e)
        {
            await load_form();
        }

        private async Task load_form()
        {
            UpdatePaginationInfo();
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Rows.Clear();

            try
            {
                int offset = (currentPage - 1) * pageSize;
                string sql = @"select tenants.id as id, name, phone, email, id_card, gender, address
                    from tenants
                    left join checkins on checkins.tenant_id = tenants.id 
                    limit @pageSize offset @offset";
                var data = await Task.Run(() => Database_connect.ExecuteReader(sql, new Dictionary<string, object>
                {
                    { "@pageSize", pageSize },
                    { "@offset", offset}
                }));

                dataGridView1.Invoke(new Action(() =>
                {
                    dataGridView1.Rows.Clear();
                    foreach (var row in data)
                    {
                        dataGridView1.Rows.Add(row["id"], row["name"], row["phone"], row["email"], row["id_card"], row["gender"], row["address"]);
                    }
                }));
                labelPageInfo.Text = $"Trang {currentPage}/{totalPages}";

            }
            catch (Exception ex)
            {
                MessageBox.Show("lỗi khi tải dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdatePaginationInfo()
        {
            string sql = "SELECT COUNT(*) FROM tenants";
            totalRecords = Convert.ToInt32(Database_connect.ExecuteScalar(sql));
            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            string sql = @"select tenants.id, name, phone, id_card, gender, address from tenants
                    left join checkins on checkins.tenant_id = tenants.id
                    where tenants.name like '%'|| @find ||'%' or id_card like '%'|| @find ||'%'";
            var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object> { { "@find", textBox1.Text} });
            foreach (var row in data)
            {
                dataGridView1.Rows.Add(row["id"], row["name"], row["phone"], row["id_card"], row["gender"], row["address"]);
            }
        }

        private void chọnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string nameGuest = get_name();
            FormAdd_check_in f = new FormAdd_check_in(nameRoom, nameGuest, type, size, _callback, checkin, checkout);
            f.ShowDialog();
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hit = dataGridView1.HitTest(e.X, e.Y);
                if (hit.RowIndex >= 0)
                {
                    // Chọn dòng chuột phải vào
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[hit.RowIndex].Selected = true;

                    // Hiển thị menu tại vị trí chuột
                    contextMenuStrip1.Show(dataGridView1, e.Location);
                }
            }
        }

        public string get_name()
        {
            string name = "";
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var row = dataGridView1.SelectedRows[0];
                name = row.Cells["name"].Value.ToString();
            }
            return name;
        }

        private void buttonCreat_Click(object sender, EventArgs e)
        {
            string Tenant = textBox1.Text;
            FormAdd_one_tenant f = new FormAdd_one_tenant(Tenant);
            f.Show();
        }

        private async void btnFirst_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            await load_form();
        }

        private async void btnPrev_Click(object sender, EventArgs e)
        {
            if(currentPage > 0)
            {
                currentPage--;
                await load_form();
            }
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                await load_form();
            }
        }

        private async void btnLast_Click(object sender, EventArgs e)
        {
            currentPage = totalPages;
            await load_form();
        }
    }
}
