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

namespace RoomManegerApp.Forms
{
    public partial class FormCheck_in : Form
    {
        public FormCheck_in()
        {
            InitializeComponent();
        }

        private void reloadData()
        {
            load_check_in();
        }

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

        private void FormCheck_in_Load(object sender, EventArgs e)
        {
            load_check_in();
        }

        public void load_check_in()
        {
            dataGridView1.Rows.Clear();
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();

                sql = @"select checkins.id as checkins_id, rooms.name as room_name, tenants.name as tenants_name, tenants.phone as tenants_phone, start_date, end_date, rooms.type as r_type, checkins.status as checkins_status
                    from checkins
                    inner join rooms on checkins.room_id = rooms.id
                    inner join tenants on checkins.tenant_id = tenants.id";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    using (doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            load_datagridview(doc);
                        }
                        doc.Close();
                    }
                }
            }
            update_status();
            cancel_status();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string textbox = textBoxSearch.Text.Trim();

            if (string.IsNullOrEmpty(textbox))
            {
                load_check_in();
            }

            bool checkSearch = false;
            sql = @"select * from rooms where name = @name";
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@name", textbox);
                    using (doc = thuchien.ExecuteReader())
                    {
                        if (doc == null)
                        {
                            checkSearch = true;
                        }
                    }
                }
            }

            if (!checkSearch)
            {
                sql = @"select checkins.id as checkins_id,rooms.name as room_name, tenants.name as tenants_name, tenants.phone as tenants_phone, start_date, end_date, rooms.type as r_type, checkins.status as checkins_status
                    from checkins
                    inner join rooms on checkins.room_id = rooms.id
                    inner join tenants on checkins.tenant_id = tenants.id
                    where room_name like '%' || @name || '%'";
            }
            else
            {
                sql = @"select checkins.id as checkins_id, rooms.name as room_name, tenants.name as tenants_name, tenants.phone as tenants_phone, start_date, end_date, rooms.type as r_type, checkins.status as checkins_status
                    from checkins
                    inner join rooms on checkins.room_id = rooms.id
                    inner join tenants on checkins.tenant_id = tenants.id
                    where tenants_name like '%' || @name || '%'";
            }
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@name", textbox);
                    using (doc = thuchien.ExecuteReader())
                    {
                        dataGridView1.Rows.Clear();
                        while (doc.Read())
                        {
                            load_datagridview(doc);
                        }
                    }
                }
            }

            textBoxSearch.Text = null;
        }
        private void load_datagridview(SQLiteDataReader doc)
        {
            string dbStart_date = doc["start_date"].ToString();
            DateTime.TryParseExact(dbStart_date, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime start_date);
            string formattedStart_date = start_date.ToString("dd/MM/yyyy");

            string dbEnd_date = doc["end_date"].ToString();
            DateTime.TryParseExact(dbEnd_date, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime end_date);
            string formattedEnd_date = end_date.ToString("dd/MM/yyyy");

            int rowIndex = dataGridView1.Rows.Add(doc["checkins_id"], doc["room_name"], doc["tenants_name"], doc["tenants_phone"], formattedStart_date, formattedEnd_date, doc["r_type"], doc["checkins_status"]);

            SetStatusColor(dataGridView1.Rows[rowIndex], doc["checkins_status"].ToString());
        }

        private void update_status()
        {
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                List<string> listID = new List<string>();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow || row.Cells["id"].Value == null) continue;

                    string id = row.Cells["id"].Value.ToString();

                    sql = @"select end_date from checkins where id = @id";
                    using (thuchien = new SQLiteCommand(sql, ketnoi))
                    {
                        thuchien.Parameters.AddWithValue("@id", id);

                        using (doc = thuchien.ExecuteReader())
                        {
                            if (doc.Read())
                            {
                                int end = Convert.ToInt32(doc["end_date"].ToString());
                                int today = int.Parse(DateTime.Now.ToString("yyyyMMdd"));

                                if (end < today)
                                {
                                    listID.Add(id);
                                }
                            }
                        }
                    }
                }
                foreach (string id in listID)
                {
                    sql = @"update checkins set status = 'Hết hiệu lực' where id = @id";
                    using (thuchien = new SQLiteCommand(sql, ketnoi))
                    {
                        thuchien.Parameters.AddWithValue("@id", id);
                        thuchien.ExecuteNonQuery();
                    }
                }
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow || row.Cells["id"].Value == null) continue;

                    if (listID.Contains(row.Cells["id"].Value.ToString()))
                    {
                        row.Cells["status"].Value = "Hết hiệu lực";
                        SetStatusColor(row, "Hết hiệu lực");
                    }
                }
            }
        }


        private void SetStatusColor(DataGridViewRow row, string status)
        {
            Color color = Color.White;
            if (status == "Còn hiệu lực") color = Color.LightGreen;
            else if (status == "Hết hiệu lực") color = Color.OrangeRed;

            row.Cells["status"].Style.BackColor = color;
        }

        private void buttonAdd_new_Click(object sender, EventArgs e)
        {
            FormStatus_room f = new FormStatus_room(reloadData);
            f.Show();
        }

        private void hủyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = get_id();
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                sql = @"select room_id, status from checkins where id = @id";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@id", id);
                    using (doc = thuchien.ExecuteReader())
                    {
                        if (doc.Read())
                        {
                            
                            string room_id = doc["room_id"].ToString();
                            string status = doc["status"].ToString();
                            doc.Close();
                            if(status == "Còn hiệu lực")
                            {
                                sql = @"update checkins set status = 'Hủy' where id = @id";
                                using (thuchien = new SQLiteCommand(sql, ketnoi))
                                {
                                    thuchien.Parameters.AddWithValue("@id", id);
                                    thuchien.ExecuteNonQuery();
                                }

                                sql = @"update rooms set status = 'Trống' where id = @id";
                                using (thuchien = new SQLiteCommand(sql, ketnoi))
                                {
                                    thuchien.Parameters.AddWithValue("@id", room_id);
                                    thuchien.ExecuteNonQuery();
                                }
                                MessageBox.Show("Hủy thành công", "Thông báo", MessageBoxButtons.OK);
                                load_check_in();
                            }
                            else
                            {
                                MessageBox.Show("Lịch này đã hết hiệu lực", "Thông báo", MessageBoxButtons.OK);
                            }
                        }
                    }
                }
            }
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

        public int get_id()
        {
            int id = -1;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var row = dataGridView1.SelectedRows[0];
                id = Int32.Parse(row.Cells[0].Value.ToString()); //lấy id từ dòng đã chọn
            }
            return id;
        }

        private void cancel_status()
        {
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                sql = @"select id, room_id,start_date, status from checkins";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    using (doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            string id = doc["id"].ToString();
                            string room_id = doc["room_id"].ToString();
                            string status = doc["status"].ToString();
                            int startDate = Convert.ToInt32(doc["start_date"].ToString());
                            DateTime start = DateTime.ParseExact(startDate.ToString(), "yyyyMMdd", null);
                            DateTime current = DateTime.Now;

                            if ((current - start).Days > 1 && status == "Còn hiệu lực")
                            {
                                sql = @"update checkins set status = 'Hủy' where id = @id";
                                using (thuchien = new SQLiteCommand(sql, ketnoi))
                                {
                                    thuchien.Parameters.AddWithValue("@id", id);
                                    thuchien.ExecuteNonQuery();
                                }

                                sql = @"update rooms set status = 'Trống' where id = @id";
                                using (thuchien = new SQLiteCommand(sql, ketnoi))
                                {
                                    thuchien.Parameters.AddWithValue("@id", room_id);
                                    thuchien.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
