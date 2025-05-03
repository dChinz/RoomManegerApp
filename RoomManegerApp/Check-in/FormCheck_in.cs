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
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();

                sql = @"create table if not exists check_in(
                    id integer primary key autoincrement,
                    room_id integer,
                    tenant_id integer,
                    start_date integer,
                    end_date integer,
                    type text,
                    status text,
                    foreign key (room_id) references rooms(id) on delete cascade,
                    foreign key (tenant_id) references tenants(id) on delete cascade);";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.ExecuteNonQuery();
                }

                sql = @"select check_in.id as check_in, rooms.name as room_name, tenants.name as tenants_name, tenants.phone as tenants_phone, start_date, end_date, check_in.type as check_in_type, check_in.status as check_in_status
                    from check_in
                    inner join rooms on check_in.room_id = rooms.id
                    inner join tenants on check_in.tenant_id = tenants.id";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    using (doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            load_datagridview();
                        }
                    }
                }
            }
            update_status();
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
                sql = @"select check_in.id as check_in,rooms.name as room_name, tenants.name as tenants_name, tenants.phone as tenants_phone, start_date, end_date, check_in.type as check_in_type, check_in.status as check_in_status
                    from check_in
                    inner join rooms on check_in.room_id = rooms.id
                    inner join tenants on check_in.tenant_id = tenants.id
                    where room_name like '%' || @name || '%'";
            }
            else
            {
                sql = @"select check_in.id as check_in, rooms.name as room_name, tenants.name as tenants_name, tenants.phone as tenants_phone, start_date, end_date, check_in.type as check_in_type, check_in.status as check_in_status
                    from check_in
                    inner join rooms on check_in.room_id = rooms.id
                    inner join tenants on check_in.tenant_id = tenants.id
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
                            load_datagridview();
                        }
                    }
                }
            }

            textBoxSearch.Text = null;
        }
        private void load_datagridview()
        {
            string dbStart_date = doc["start_date"].ToString();
            DateTime.TryParseExact(dbStart_date, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime start_date);
            string formattedStart_date = start_date.ToString("dd/MM/yyyy");

            string dbEnd_date = doc["end_date"].ToString();
            DateTime.TryParseExact(dbEnd_date, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime end_date);
            string formattedEnd_date = end_date.ToString("dd/MM/yyyy");

            int rowIndex = dataGridView1.Rows.Add(doc["check_in"], doc["room_name"], doc["tenants_name"], doc["tenants_phone"], formattedStart_date, formattedEnd_date, doc["check_in_type"], doc["check_in_status"]);

            SetStatusColor(dataGridView1.Rows[rowIndex], doc["check_in_status"].ToString());
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

                    sql = @"select end_date from check_in where id = @id";
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
                    sql = @"update check_in set status = 'Hết hạn' where id = @id";
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
                        row.Cells["status"].Value = "Hết hạn";
                        SetStatusColor(row, "Hết hạn");
                    }
                }
            }
        }


        private void SetStatusColor(DataGridViewRow row, string status)
        {
            Color color = Color.White;
            if (status == "Còn hiệu lực") color = Color.LightGreen;
            else if (status == "Hết hạn") color = Color.OrangeRed;

            row.Cells["status"].Style.BackColor = color;
        }

        private void buttonAdd_new_Click(object sender, EventArgs e)
        {
            FormStatus_room f = new FormStatus_room();
            f.Show();
        }
    }
}
