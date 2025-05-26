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
using RoomManegerApp.Bills;
using RoomManegerApp.Report;

namespace RoomManegerApp.Forms
{
    public partial class FormBills : Form
    {
        private FormDashboard dashboard;
        public FormBills(FormDashboard dashboard)
        {
            InitializeComponent();
            this.dashboard = dashboard;
        }

        private int currentPage = 1;
        private int pageSize = 22;
        private int totalPages = 0;
        private int totalRecords = 0;

        private async void reloadData()
        {
            await load_bill();
            dashboard.reloadData();
        }

        private async void FormBills_Load(object sender, EventArgs e)
        {
            await load_bill();
        }
        private async Task load_bill()
        {
            UpdatePaginationInfo();
            int offset = (currentPage - 1) * pageSize;
            try
            {
                dataGridView1.Rows.Clear();
                string sql = @"select bills.id as billId, bills.checkins_id as checkinId, rooms.name as roomName, tenants.name as tenantName, start_date, end_date,
                        rooms.type as roomType, rooms.price as roomPrice, checkins.deposit as checkinDeposit, bills.status as billStatus, bills.userId as billUserId
                        from bills
                        inner join checkins on bills.checkins_id = checkins.id
                        inner join rooms on checkins.room_id = rooms.id
                        inner join tenants on checkins.tenant_id = tenants.id
                        limit @pageSize offset @offset";
                var data = await Task.Run(() => Database_connect.ExecuteReader(sql, new Dictionary<string, object>
                {
                    { "@pageSize", pageSize},
                    { "@offset", offset},
                }));
                foreach (var row in data)
                {
                    FillDataGridView(row);
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            labelPageInfo.Text = $"Trang {currentPage}/{totalPages}";
        }

        private void FillDataGridView(Dictionary<string, object> row)
        {
            int startDate = Convert.ToInt32(row["start_date"].ToString());
            int endDate = Convert.ToInt32(row["end_date"].ToString());
            DateTime start = DateTime.ParseExact(startDate.ToString(), "yyyyMMdd", null);
            DateTime end = DateTime.ParseExact(endDate.ToString(), "yyyyMMdd", null);
            string userName = "";

            string sql = @"select username from users where id = @id";
            var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object> { { "@id", row["billUserId"] } });
            foreach(var row2 in data)
            {
                userName = row2["username"].ToString();
            }

            int totalDays = (end - start).Days;
            double price = Convert.ToDouble(row["roomPrice"].ToString());
            double deposit = Convert.ToDouble(row["checkinDeposit"].ToString());

            dataGridView1.Rows.Add(row["billId"], row["checkinId"], row["roomName"], row["tenantName"], totalDays, row["roomType"], Convert.ToInt32(row["roomPrice"]).ToString("#,##0"), Convert.ToInt32(deposit).ToString("#,##0"), Convert.ToInt32(totalDays * price - deposit).ToString("#,##0"), row["billStatus"], userName);
        }

        private void UpdatePaginationInfo()
        {
            string sql = "SELECT COUNT(*) FROM bills";
            totalRecords = Convert.ToInt32(Database_connect.ExecuteScalar(sql));
            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        }

        private void buttonAdd_new_Click(object sender, EventArgs e)
        {
            FormRoom_checkined form = new FormRoom_checkined(reloadData);
            form.Show();
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

        private async void đãThanhToánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = @"update bills set status = 'Đã thanh toán' where id = @id";
                int id = get_id_bill();
                int rowAffected = Convert.ToInt16(Database_connect.ExecuteNonQuery(sql, new Dictionary<string, object> { { "@id", id } }));
                if (rowAffected > 0)
                {
                    MessageBox.Show("Update thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataGridView1.Rows.Clear();
                    await load_bill();
                }
                else
                {
                    MessageBox.Show("Cập nhật không thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public int get_id_bill()
        {
            if (dataGridView1.SelectedRows.Count > 0 && Int32.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int id))
            {
                return id;
            }
            return -1;
        }

        private void inHóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = get_id_bill();
            PrintBills form = new PrintBills(id);
            form.ShowDialog();
        }

        private async void btnFirst_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            await load_bill();
        }

        private async void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                await load_bill();
            }
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                await load_bill();
            }
        }

        private async void btnLast_Click(object sender, EventArgs e)
        {
            currentPage = totalPages;
            await load_bill();
        }
    }
}
