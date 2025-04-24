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
using RoomManegerApp.Meter_readings;

namespace RoomManegerApp.Forms
{
    public partial class FormMeterreading : Form
    {
        public FormMeterreading()
        {
            InitializeComponent();

            // Bật DoubleBuffering để giảm giật khi cuộn
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null, dataGridView1, new object[] { true });
        }

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

        private void FormMeterreading_Load(object sender, EventArgs e)
        {
            load_meterreadings();

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = false;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[5].ReadOnly = true;

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/yyyy";
        }

        public void load_meterreadings()
        {
            string month = dateTimePicker1.Value.AddMonths(-1).Month.ToString("D2");
            string year = dateTimePicker1.Value.AddMonths(-1).Year.ToString();

            sql = @"select meterreadings.id, rooms.name, month, e_number, w_number
                    from meterreadings
                    inner join rooms on rooms.id = meterreadings.room_id
                    where month = @month";
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@month", month + "/" + year);
                    using (doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            dataGridView1.Rows.Add(false, doc["id"], doc["name"], doc["month"], doc["e_number"], doc["w_number"]);
                        }
                    }
                }
            }
        }

        private void buttonAdd_new_Click(object sender, EventArgs e)
        {
            sql = @"select month from meterreadings order by id desc limit 1";
            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using(thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    using(doc = thuchien.ExecuteReader())
                    {
                        string month = DateTime.Now.ToString("MM/yyyy");
                        if (doc.Read())
                        {
                            if (doc["month"].ToString() == month)
                            {
                                DialogResult result = MessageBox.Show("Tháng này đã có bản ghi \n Vẫn tiếp tục ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (result == DialogResult.No)
                                {
                                    return;
                                }
                                else
                                {
                                    FormAdd_new form = new FormAdd_new();
                                    form.Show();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string month = dateTimePicker1.Value.Month.ToString("D2");
            string year = dateTimePicker1.Value.Year.ToString();

            sql = @"select meterreadings.id, rooms.name, month, e_number, w_number
                    from meterreadings
                    inner join rooms on rooms.id = meterreadings.room_id
                    where month = @month";
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@month", month + "/" + year);
                    using (doc = thuchien.ExecuteReader())
                    {
                        if (doc.HasRows)
                        {
                            dataGridView1.Rows.Clear();
                            while (doc.Read())
                            {
                                dataGridView1.Rows.Add(false, doc["id"], doc["name"], doc["month"], doc["e_number"], doc["w_number"]);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Tháng được chọn không có dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }
    }
}
