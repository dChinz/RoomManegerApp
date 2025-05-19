using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace RoomManegerApp.Booking
{
    public partial class FormBooking : Form
    {
        public FormBooking()
        {
            InitializeComponent();
        }

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

        private void FormBooking_Load(object sender, EventArgs e)
        {
            load_form();
        }
        private void load_form()
        {
            dataGridView1.Rows.Clear();
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                sql = "select name, phone, email, roomSize, checkin, checkout, type from booking";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    using (doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            DateTime.TryParseExact(doc["checkin"].ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime dbcheckin);
                            string checkin = dbcheckin.ToString("dd/MM/yyyy");

                            DateTime.TryParseExact(doc["checkin"].ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime dbcheckout);
                            string checkout = dbcheckout.ToString("dd/MM/yyyy");

                            dataGridView1.Rows.Add(doc[0], doc[1], doc[2], doc[3], checkin, checkout, doc[6]);
                        }
                    }
                }
            }

            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.HeaderText = "Xác nhận";
            btn.Name = "accept";
            btn.Text = "Xác nhận";
            btn.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(btn);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 8)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string id_room = "";
                string id_tenant = "";
                string id = row.Cells[0].Value.ToString();
                string name = row.Cells[1].Value.ToString();
                string phone = row.Cells[2].Value.ToString();
                string email = row.Cells[3].Value.ToString();
                string roomSize = row.Cells[4].Value.ToString();
                string checkin = row.Cells[5].Value.ToString();
                string checkout = row.Cells[6].Value.ToString();
                string type = row.Cells[7].Value.ToString();
                try
                {
                    using(ketnoi = Database_connect.connection())
                    {
                        ketnoi.Open();
                        sql = "select 1 from tenants where name = @name";
                        using(thuchien = new SQLiteCommand(sql, ketnoi))
                        {
                            thuchien.Parameters.AddWithValue("@name", name);
                            using(doc = thuchien.ExecuteReader())
                            {
                                if (doc.Read())
                                {
                                    doc.Close();
                                    sql = @"update tenants set phone = @phone, email = @email where name = @name";
                                }
                                else
                                {
                                    doc.Close();
                                    sql = @"insert into tenants (name, phone, email) values (@name, @phone, @email)";
                                }
                                using (thuchien = new SQLiteCommand(sql, ketnoi))
                                {
                                    thuchien.Parameters.AddWithValue("@phone", phone);
                                    thuchien.Parameters.AddWithValue("@email", email);
                                    thuchien.Parameters.AddWithValue("@name", name);
                                    thuchien.ExecuteNonQuery();
                                }
                            }
                        }

                        sql = @"select id
                                from rooms
                                where status = @status and size = @size and type = @type
                                order by id asc
                                limit 1";
                        using(thuchien = new SQLiteCommand(sql, ketnoi))
                        {
                            thuchien.Parameters.AddWithValue("@status", "Trống");
                            thuchien.Parameters.AddWithValue("@size", roomSize);
                            thuchien.Parameters.AddWithValue("@type", type);
                            using(doc = thuchien.ExecuteReader())
                            {
                                if (doc.Read())
                                {
                                    id_room = doc["id"].ToString();
                                    doc.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Không có phòng tương ứng.", "Hết phòng", MessageBoxButtons.OK);
                                    return;
                                }
                            }
                        }

                        sql = @"select id from tenants where name = @name";
                        using (thuchien = new SQLiteCommand(sql, ketnoi))
                        {
                            thuchien.Parameters.AddWithValue("@name", name);
                            using (doc = thuchien.ExecuteReader())
                            {
                                if (doc.Read())
                                {
                                    id_tenant = doc["id"].ToString();
                                }
                            }
                        }

                        sql = @"insert into checkins (room_id, tenant_id, start_date, end_date, status) values (@room_id, @tenant_id, @start_date, @end_date, @status)";
                        using(thuchien = new SQLiteCommand(sql, ketnoi))
                        {
                            thuchien.Parameters.AddWithValue("@room_id", id_room);
                            thuchien.Parameters.AddWithValue("@tenant_id", id_tenant);
                            thuchien.Parameters.AddWithValue("@start_date", checkin);
                            thuchien.Parameters.AddWithValue("@end_date", checkout);
                            thuchien.Parameters.AddWithValue("@status", "Hiệu lực");
                            thuchien.ExecuteNonQuery();
                        }

                        sql = @"update rooms set status = 'Đã thuê' where id = @id";
                        using (thuchien = new SQLiteCommand(sql, ketnoi))
                        {
                            thuchien.Parameters.AddWithValue("@id", id_room);
                            thuchien.ExecuteNonQuery();
                        }

                        sql = @"delete from booking where id = @id";
                        using(thuchien = new SQLiteCommand(sql, ketnoi))
                        {
                            thuchien.Parameters.AddWithValue("@id", id);
                            thuchien.ExecuteNonQuery() ;
                        }
                        MessageBox.Show("Xác nhận thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        load_form();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex}");
                }
            }
        }
    }
}
