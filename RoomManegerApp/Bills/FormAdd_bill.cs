using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RoomManegerApp.Forms;

namespace RoomManegerApp.Bills
{
    public partial class FormAdd_bill : Form
    {
        private string roomName;
        private Action _callback;

        public FormAdd_bill(string name, Action callback)
        {
            InitializeComponent();
            roomName = name;
            _callback = callback;
        }

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

        private void FormAdd_bill_Load(object sender, EventArgs e)
        {
            load_add_bill();
        }

        private void load_add_bill()
        {
            sql = @"select tenants.name as t_name, rooms.name as r_name, checkins.start_date as c_s_date, checkins.end_date as c_e_date, rooms.type as r_type, rooms.price as r_price
                    from checkins
                    inner join rooms on checkins.room_id = rooms.id
                    inner join tenants on checkins.tenant_id = tenants.id
                    where rooms.name = @name";
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@name", roomName);
                    using (doc = thuchien.ExecuteReader())
                    {
                        if (doc.Read())
                        {
                            int startDate = Convert.ToInt32(doc["c_s_date"].ToString());
                            int endDate = Convert.ToInt32(doc["c_e_date"].ToString());
                            DateTime start = DateTime.ParseExact(startDate.ToString(), "yyyyMMdd", null);
                            DateTime end = DateTime.ParseExact(endDate.ToString(), "yyyyMMdd", null);
                            int totalDays = (end - start).Days;

                            label2.Text = doc["r_name"].ToString();
                            label4.Text = doc["t_name"].ToString();
                            label6.Text = totalDays.ToString();
                            label8.Text = doc["r_price"].ToString();
                            label10.Text = doc["r_type"].ToString();
                            double price = Convert.ToInt32(label6.Text) * Convert.ToInt32(label8.Text);
                            label12.Text = string.Format(new CultureInfo("vi-VN"), "{0:N0} đ", price); 
                        }
                    }
                }
            }
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            int c_id = 0;
            string status = comboBox1.Text;
            string note = textBox1.Text;
            string total = label12.Text;
            total = total.Replace("đ", "").Replace(".", "").Trim();

            if (string.IsNullOrEmpty(status))
            {
                MessageBox.Show("Vui lòng chọn tình trạng thanh toán", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            sql = @"select checkins.id as c_id
                    from checkins
                    inner join rooms on checkins.room_id = rooms.id
                    where rooms.name = @name";
            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using(thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@name", roomName);
                    using (doc = thuchien.ExecuteReader())
                    {
                        if (doc.Read())
                        {
                            c_id = Convert.ToInt32(doc["c_id"].ToString());
                        }
                    }
                }

                sql = @"insert into bills (checkins_id, total, status) values (@checkins_id, @total, @status)";
                using(thuchien = new SQLiteCommand (sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@checkins_id", c_id);
                    thuchien.Parameters.AddWithValue("@total", total);
                    thuchien.Parameters.AddWithValue("@status", status);
                    thuchien.ExecuteNonQuery();
                }

                sql = @"update rooms set status = 'Trống' where name = @name";
                using(thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@name", roomName);
                    thuchien.ExecuteNonQuery();
                }

                sql = @"update checkins set status = 'Hết hiệu lực' where id = @id";
                using(thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@id", c_id);
                    thuchien.ExecuteNonQuery();
                }

                MessageBox.Show("Thêm mới thanh công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _callback?.Invoke();
                this.Close();
            }
        }
    }
}
