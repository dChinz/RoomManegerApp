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

                sql = @"create table if not exists bills (
                    id integer primary key autoincrement,
                    check_in_id integer,
                    total_days integer,
                    rent real,
                    type text,
                    total real,
                    status text,
                    note text,
                    foreign key (check_in_id) references check_in(id) on delete cascade
                    foreign key (type) references check_in(type) on delete cascade)";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.ExecuteNonQuery();
                }

                sql = @"select id, check_in_id, total_days, rent, type, total, status, note from bills";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    using (doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            dataGridView1.Rows.Add(doc["id"], doc["check_in_id"], doc["total_days"], doc["rent"], doc["type"], doc["total"], doc["status"], doc["note"]);
                        }
                    }
                }
            }
        }

        private void buttonAdd_new_Click(object sender, EventArgs e)
        {
            FormStatus_room_2 form = new FormStatus_room_2(reloadData);
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
    }
}
