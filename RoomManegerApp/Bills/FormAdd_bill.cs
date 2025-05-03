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
            sql = @"select tenants.name as t_name, rooms.name as r_name, check_in.start_date as c_s_date, check_in.end_date as c_e_date, check_in.type as c_type, rooms.price as r_price
                    from check_in
                    inner join rooms on check_in.room_id = rooms.id
                    inner join tenants on check_in.tenant_id = tenants.id
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
                            label2.Text = doc["r_name"].ToString();
                            label4.Text = doc["t_name"].ToString();
                            label6.Text = (Convert.ToInt32(doc["c_e_date"].ToString()) - Convert.ToInt32(doc["c_s_date"].ToString())).ToString();
                            label8.Text = doc["r_price"].ToString();
                            label10.Text = doc["c_type"].ToString();
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

            sql = @"select check_in.id as c_id
                    from check_in
                    inner join rooms on check_in.room_id = rooms.id
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

                sql = @"insert into bills (check_in_id, total_days, rent, type, total, status, note) values (@check_in_id, @total_days, @rent, @type, @total, @status, @note)";
                using(thuchien = new SQLiteCommand (sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@check_in_id", c_id);
                    thuchien.Parameters.AddWithValue("@total_days", label6.Text);
                    thuchien.Parameters.AddWithValue("@rent", label8.Text);
                    thuchien.Parameters.AddWithValue("@type", label10.Text);
                    thuchien.Parameters.AddWithValue("@total", total);
                    thuchien.Parameters.AddWithValue("@status", status);
                    thuchien.Parameters.AddWithValue("@note", note);
                    thuchien.ExecuteNonQuery();
                }

                sql = @"update rooms set status = 'Trống' where name = @name";
                using(thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@name", roomName);
                    thuchien.ExecuteNonQuery();
                }

                sql = @"delete from check_in where id = @id";
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
