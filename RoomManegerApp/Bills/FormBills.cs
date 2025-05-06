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
        public FormBills()
        {
            InitializeComponent();
        }

        private void reloadData()
        {
            dataGridView1.Rows.Clear();
            load_bill();
        }

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

        private void FormBills_Load(object sender, EventArgs e)
        {
            load_bill();
        }
        private void load_bill()
        {
            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                sql = @"select bills.id as b_id, bills.checkins_id as b_c_id, bills.status as b_status, end_date, start_date , rooms.price as r_price
                        from bills
                        inner join checkins on bills.checkins_id = checkins.id
                        inner join rooms on checkins.room_id = rooms.id";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    using (doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            int startDate = Convert.ToInt32(doc["start_date"].ToString());
                            int endDate = Convert.ToInt32(doc["end_date"].ToString());
                            DateTime start = DateTime.ParseExact(startDate.ToString(), "yyyyMMdd", null);
                            DateTime end = DateTime.ParseExact(endDate.ToString(), "yyyyMMdd", null);
                            int totalDays = (end - start).Days;
                            double price = Convert.ToDouble(doc["r_price"].ToString());

                            dataGridView1.Rows.Add(doc["b_id"], doc["b_c_id"], totalDays, price, totalDays * price, doc["b_status"]);
                        }
                    }
                }
            }
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

        private void đãThanhToánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sql = @"update bills set status = 'Đã thanh toán' where id = @id";
            int id = get_id_bill();
            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@id", id);
                    thuchien.ExecuteNonQuery();
                }
                
            }
            MessageBox.Show("Update thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dataGridView1.Rows.Clear();
            load_bill();
        }

        public int get_id_bill()
        {
            int id = -1;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var row = dataGridView1.SelectedRows[0];
                id = Int32.Parse(row.Cells[0].Value.ToString()); //lấy id từ dòng đã chọn
            }
            return id;
        }

        private void inHóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = get_id_bill();
            PrintBills form = new PrintBills(id);
            form.ShowDialog();
        }
    }
}
