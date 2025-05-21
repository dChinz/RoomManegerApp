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
        private FormDashboard dashboard;
        public FormCheck_in(FormDashboard dashboard)
        {
            InitializeComponent();
            this.dashboard = dashboard;
        }

        private async void reloadData()
        {
            await load_check_in();
            dashboard.reloadData();
        }
        private async void FormCheck_in_Load(object sender, EventArgs e)
        {
            await load_check_in();
            updateStatus();
        }

        public async Task load_check_in()
        {
            dataGridView1.Rows.Clear();
            string sql = @"select checkins.id as checkins_id, rooms.name as room_name, tenants.name as tenants_name, tenants.phone as tenants_phone, start_date, end_date, rooms.type as r_type, checkins.status as checkins_status
                    from checkins
                    inner join rooms on checkins.room_id = rooms.id
                    inner join tenants on checkins.tenant_id = tenants.id";
            var data = await Task.Run(() => Database_connect.ExecuteReader(sql));
            foreach (var row in data)
            {
                load_datagridview(row);
            }
        }

        private async void buttonSearch_Click(object sender, EventArgs e)
        {
            string name = textBoxSearch.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                await load_check_in();
            }

            string sql = @"select checkins.id as checkins_id,rooms.name as room_name, tenants.name as tenants_name, tenants.phone as tenants_phone, start_date, end_date, rooms.type as r_type, checkins.status as checkins_status
                    from checkins
                    inner join rooms on checkins.room_id = rooms.id
                    inner join tenants on checkins.tenant_id = tenants.id
                    where rooms.name like '%' || @name || '%' or tenants.name like '%' || @name || '%'";
            var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object> { { "@name", name} });
            foreach(var row in data)
            {
                load_datagridview(row);
            }

            textBoxSearch.Text = null;
        }
        private void load_datagridview(Dictionary<string, object> row)
        {
            string dbStart_date = row["start_date"].ToString();
            DateTime.TryParseExact(dbStart_date, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime start_date);
            string formattedStart_date = start_date.ToString("dd/MM/yyyy");

            string dbEnd_date = row["end_date"].ToString();
            DateTime.TryParseExact(dbEnd_date, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime end_date);
            string formattedEnd_date = end_date.ToString("dd/MM/yyyy");

            int rowIndex = dataGridView1.Rows.Add(row["checkins_id"], row["room_name"], row["tenants_name"], row["tenants_phone"], formattedStart_date, formattedEnd_date, row["r_type"], row["checkins_status"]);

            SetStatusColor(dataGridView1.Rows[rowIndex], row["checkins_status"].ToString());
        }

        private void SetStatusColor(DataGridViewRow row, string status)
        {
            Color color = status switch
            {
                "Hiệu lực" => Color.LightGreen,
                "Hết hiệu lục" => Color.OrangeRed,
                "Đã xử lý" => Color.Aqua,
                "Hủy" => Color.Gray,
                _ => Color.White,
            };

            row.Cells["status"].Style.BackColor = color;
        }

        private void buttonAdd_new_Click(object sender, EventArgs e)
        {
            FormAvailable_room f = new FormAvailable_room(reloadData);
            f.Show();
        }

        private async void hủyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = get_id();
            string sql = @"select room_id, status from checkins where id = @id";
            var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object> { { "@id", id } });
            foreach (var row in data)
            {
                string room_id = row["room_id"].ToString();
                string status = row["status"].ToString();
                if(status == "Còn hiệu lực")
                {
                    sql = @"update checkins set status = ' Đã hủy' where id = @id";
                    Database_connect.ExecuteNonQuery(sql, new Dictionary<string, object> { { "@id", id } });

                    sql = @"update rooms set status = 'Trống' where id = @id";
                    Database_connect.ExecuteNonQuery(sql, new Dictionary<string, object> { { "@id", room_id } });

                    MessageBox.Show("Hủy thành công", "Thông báo", MessageBoxButtons.OK);
                    await load_check_in();
                }
                else
                {
                    MessageBox.Show("Lịch này đã hết hiệu lực", "Thông báo", MessageBoxButtons.OK);
                    return;
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
            if (dataGridView1.SelectedRows.Count > 0 && Int32.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int id))
                {
                return id;
            }
            return - 1;
        }

        private void updateStatus()
        {
            string sql = @"select id, end_date, status from checkins";
            var data = Database_connect.ExecuteReader(sql);
            DateTime now = DateTime.Now;
            foreach (var row in data)
            {
                string id = row["id"].ToString();
                string end_date = row["end_date"].ToString();
                DateTime end = DateTime.ParseExact(end_date, "yyyyMMdd", null);
                if ((end - now).Days >= 0)
                {
                    sql = @"update checkins set status = 'Hết hiệu lực' where id = @id";
                    Database_connect.ExecuteNonQuery(sql, new Dictionary<string, object> { { "@id", id } });
                }
            }
        }
    }
}
