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
            try
            {
                dataGridView1.Rows.Clear();
                string sql = @"select bills.id as b_id, bills.checkins_id as b_c_id, bills.status as b_status, end_date, start_date , rooms.price as r_price, time
                        from bills
                        inner join checkins on bills.checkins_id = checkins.id
                        inner join rooms on checkins.room_id = rooms.id";
                var data = await Task.Run(() => Database_connect.ExecuteReader(sql));
                foreach (var row in data)
                {
                    FillDataGridView(row);
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillDataGridView(Dictionary<string, object> row)
        {
            int startDate = Convert.ToInt32(row["start_date"].ToString());
            int endDate = Convert.ToInt32(row["end_date"].ToString());
            DateTime start = DateTime.ParseExact(startDate.ToString(), "yyyyMMdd", null);
            DateTime end = DateTime.ParseExact(endDate.ToString(), "yyyyMMdd", null);

            int totalDays = (end - start).Days;
            double price = Convert.ToDouble(row["r_price"].ToString());

            double time = Convert.ToInt64(row["time"].ToString());
            DateTime checkout = DateTime.ParseExact(time.ToString(), "yyyyMMddHHmmss", null);

            dataGridView1.Rows.Add(row["b_id"], row["b_c_id"], totalDays, price, totalDays * price, row["b_status"], checkout);
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
    }
}
