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
using RoomManegerApp.Check_in;
using RoomManegerApp.Contracts;
using RoomManegerApp.Report;

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

        private int currentPage = 1;
        private int pageSize = 22;
        private int totalRecords = 0;
        private int totalPages = 0;

        private async void reloadData()
        {
            updateStatus();
            await load_check_in();
            dashboard.reloadData();
        }
        private async void FormCheck_in_Load(object sender, EventArgs e)
        {
            updateStatus();
            await load_check_in();
        }

        public async Task load_check_in()
        {
            UpdatePaginationInfo();
            int offset = (currentPage - 1) * pageSize;
            string time = DateTime.Today.ToString("yyyyMMdd");
            dataGridView1.Rows.Clear();
            try
            {
                string sql = @"select checkins.id as checkins_id, rooms.name as room_name, tenants.name as tenants_name, 
                    tenants.phone as tenants_phone, start_date, end_date, rooms.type as r_type, deposit, checkins.status as c_status, 
                    rooms.price as price, bills.status as billStatus
                    from checkins
                    inner join rooms on checkins.room_id = rooms.id
                    inner join tenants on checkins.tenant_id = tenants.id
                    left join bills on checkins.id = bills.checkins_id
                    where start_date = @time or end_date = @time
                    limit @pageSize offset @offset";
                var data = await Task.Run(() => Database_connect.ExecuteReader(sql, new Dictionary<string, object>
                {
                    { "@pageSize", pageSize },
                    { "@offset", offset},
                    {"@time", time }
                }));
                foreach (var row in data)
                {
                    load_datagridview(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đã xảy ra: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            labelPageInfo.Text = $"Trang {currentPage}/{totalPages}";
        }

        private void UpdatePaginationInfo()
        {
            string sql = "SELECT COUNT(*) FROM checkins";
            totalRecords = Convert.ToInt32(Database_connect.ExecuteScalar(sql));
            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        }

        private async void buttonSearch_Click(object sender, EventArgs e)
        {
            string name = textBoxSearch.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                await load_check_in();
                return;
            }

            string sql = @"select checkins.id as checkins_id, rooms.name as room_name, tenants.name as tenants_name, 
                    tenants.phone as tenants_phone, start_date, end_date, rooms.type as r_type, deposit, checkins.status as c_status, 
                    rooms.price as price, bills.status as billStatus
                    from checkins
                    inner join rooms on checkins.room_id = rooms.id
                    inner join tenants on checkins.tenant_id = tenants.id
                    left join bills on checkins.id = bills.checkins_id
                    where rooms.name like '%' || @name || '%' or tenants.name like '%' || @name || '%'";
            var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object> { { "@name", name} });
            dataGridView1.Rows.Clear();
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

            string price = Convert.ToInt32(row["price"]).ToString("#,##0");
            string deposit = Convert.ToInt32(row["deposit"]).ToString("#,##0");

            int rowIndex = dataGridView1.Rows.Add(row["checkins_id"], row["room_name"], row["tenants_name"], row["tenants_phone"],
                formattedStart_date, formattedEnd_date, row["r_type"], deposit, row["c_status"], price, row["billStatus"]);

            SetStatusColor(dataGridView1.Rows[rowIndex], row["c_status"].ToString());
        }

        private void SetStatusColor(DataGridViewRow row, string status)
        {
            Color backcolor = status switch
            {
                "Trống" => Color.LightGreen,
                "Sắp nhận phòng" => Color.Khaki,
                "Tới ngày trả phòng" => Color.LightGoldenrodYellow,
                "Đang thuê" => Color.LightSalmon,
                "Đã hủy" => Color.LightGray,
                _ => Color.White,
            };

            row.Cells["status"].Style.BackColor = backcolor;

            Color forecolor = status switch
            {
                "Trống" => Color.DarkGreen,
                "Sắp nhận phòng" => Color.DarkOrange,
                "Tới ngày trả phòng" => Color.DarkGoldenrod,
                "Đang thuê" => Color.Firebrick,
                "Đã hủy" => Color.DimGray,
                _ => Color.Black,
            };

            row.Cells["status"].Style.ForeColor = forecolor;
        }

        private void buttonAdd_new_Click(object sender, EventArgs e)
        {
            FormAvailable_room f = new FormAvailable_room(reloadData);
            f.ShowDialog();
        }

        private async void hủyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = get_id();
            string sql = @"select status from checkins where id = @id";
            var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object> { { "@id", id } });
            foreach (var row in data)
            {
                string status = row["status"].ToString();
                if(status == "Sắp nhận phòng")
                {
                    sql = @"update checkins set status = 'Đã hủy' where id = @id";
                    Database_connect.ExecuteNonQuery(sql, new Dictionary<string, object> { { "@id", id } });

                    MessageBox.Show("Hủy đặt trước thành công", "Thông báo", MessageBoxButtons.OK);
                    await load_check_in();
                }
                else
                {
                    MessageBox.Show("Không thể hoàn thành tác vụ.\nLỗi: phòng đang trong quá trình thuê", "Thông báo", MessageBoxButtons.OK);
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
            try
            {
                string sql = @"SELECT id, room_id, start_date, end_date, status FROM checkins";
                var data = Database_connect.ExecuteReader(sql);
                DateTime today = DateTime.Today;

                foreach (var row in data)
                {
                    string id = row["id"].ToString();
                    string room_id = row["room_id"].ToString();
                    string startDateStr = row["start_date"].ToString();
                    string endDateStr = row["end_date"].ToString();
                    string dbstatus = row["status"].ToString();

                    if (dbstatus == "Đã hủy")
                        continue;
                    if(dbstatus == "Đã trả phòng")
                    {
                        string updateRoomSql = @"UPDATE rooms SET status = @status WHERE id = @id";
                        var roomParams = new Dictionary<string, object>
                            {
                                { "@status", "Trống" },
                                { "@id", room_id }
                            };
                        Database_connect.ExecuteNonQuery(updateRoomSql, roomParams);
                    }
                    if (DateTime.TryParseExact(startDateStr, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime start) &&
                        DateTime.TryParseExact(endDateStr, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime end))
                    {
                        string status = "";

                        if (today < start)
                            status = "Sắp nhận phòng";
                        else if (today >= start && today < end)
                            status = "Đang thuê";
                        else if (today == end)
                            status = "Tới ngày trả phòng";
                        else if (today > end)
                            status = "Đã trả phòng";

                        if(status != dbstatus)
                        {
                            string updateSql = @"UPDATE checkins SET status = @status WHERE id = @id";
                            var parameters = new Dictionary<string, object>
                        {
                            { "@status", status },
                            { "@id", id }
                        };
                            Database_connect.ExecuteNonQuery(updateSql, parameters);

                            if (status == "Đang thuê")
                            {
                                string updateRoomSql = @"UPDATE rooms SET status = @status WHERE id = @id";
                                var roomParams = new Dictionary<string, object>
                            {
                                { "@status", status },
                                { "@id", room_id }
                            };
                                Database_connect.ExecuteNonQuery(updateRoomSql, roomParams);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi định dạng ngày " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnFirst_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            await load_check_in();
        }

        private async void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                await load_check_in();
            }
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            if(currentPage < totalPages)
            {
                currentPage++;
                await load_check_in();
            }
        }

        private async void btnLast_Click(object sender, EventArgs e)
        {
            currentPage = totalPages;
            await load_check_in();
        }

        private async void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            UpdatePaginationInfo();
            int offset = (currentPage - 1) * pageSize;
            string time = dateTimePicker1.Value.ToString("yyyyMMdd");
            dataGridView1.Rows.Clear();
            try
            {
                string sql = @"select checkins.id as checkins_id, rooms.name as room_name, tenants.name as tenants_name, 
                    tenants.phone as tenants_phone, start_date, end_date, rooms.type as r_type, deposit, checkins.status as c_status, 
                    rooms.price as price, bills.status as billStatus
                    from checkins
                    inner join rooms on checkins.room_id = rooms.id
                    inner join tenants on checkins.tenant_id = tenants.id
                    left join bills on checkins.id = bills.checkins_id
                    where start_date = @time or end_date = @time
                    limit @pageSize offset @offset";
                var data = await Task.Run(() => Database_connect.ExecuteReader(sql, new Dictionary<string, object>
                {
                    { "@pageSize", pageSize },
                    { "@offset", offset},
                    {"@time", time }
                }));
                foreach (var row in data)
                {
                    load_datagridview(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đã xảy ra: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            labelPageInfo.Text = $"Trang {currentPage}/{totalPages}";
        }

        private void xácNhậnThanhToánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = get_id();
            if (checkStatus(id))
            {
                MessageBox.Show("Phòng này đã được trả rồi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                string sql = @"update bills set status = 'Thanh toán toàn bộ' where checkins_id = @id";
                int hasRows = Convert.ToInt32(Database_connect.ExecuteNonQuery(sql, new Dictionary<string, object> { { @"id", id } }));
                if (hasRows > 0)
                {
                    double deposit = 0;
                    sql = @"select start_date, end_date, rooms.price
                            from checkins
                            inner join rooms on checkins.room_id = rooms.id
                            where checkins.id = @id";
                    var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object> { { "@id", id } });
                    foreach(var row in data)
                    {
                        DateTime.TryParseExact(row["start_date"].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime checkin);
                        DateTime.TryParseExact(row["end_date"].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime checkout);
                        double price = Convert.ToDouble(row["price"].ToString());
                        deposit = (checkout - checkin).Days * price;
                    }

                    sql = @"update checkins set deposit = @deposit where id = @id";
                    Database_connect.ExecuteNonQuery(sql, new Dictionary<string, object> { { "@deposit", deposit }, { "@id", id } });

                    MessageBox.Show("Cập nhật thanh toán thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    reloadData();
                }
                else
                {
                    MessageBox.Show("Cập nhật không thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đã xảy ra " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

        }

        private void inHóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = get_id();
            PrintBills form = new PrintBills(id);
            form.ShowDialog();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            UpdatePaginationInfo();
            int offset = (currentPage - 1) * pageSize;
            string time = dateTimePicker1.Value.ToString("yyyyMMdd");
            dataGridView1.Rows.Clear();
            try
            {
                string sql = @"select checkins.id as checkins_id, rooms.name as room_name, tenants.name as tenants_name, 
                    tenants.phone as tenants_phone, start_date, end_date, rooms.type as r_type, deposit, checkins.status as c_status, 
                    rooms.price as price, bills.status as billStatus
                    from checkins
                    inner join rooms on checkins.room_id = rooms.id
                    inner join tenants on checkins.tenant_id = tenants.id
                    left join bills on checkins.id = bills.checkins_id
                    limit @pageSize offset @offset";
                var data = await Task.Run(() => Database_connect.ExecuteReader(sql, new Dictionary<string, object>
                {
                    { "@pageSize", pageSize },
                    { "@offset", offset},
                }));
                foreach (var row in data)
                {
                    load_datagridview(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đã xảy ra: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            labelPageInfo.Text = $"Trang {currentPage}/{totalPages}";
        }

        private async void xácNhậnTrảPhòngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int id = get_id();
                string sql = @"SELECT room_id, start_date, end_date, status FROM checkins where id = @id";
                var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object> { { "@id", id} });
                DateTime today = DateTime.Today;

                foreach (var row in data)
                {
                    string startDateStr = row["start_date"].ToString();
                    string endDateStr = row["end_date"].ToString();
                    string dbstatus = row["status"].ToString();

                    if (dbstatus == "Đã hủy")
                    {
                        MessageBox.Show("Lịch đặt phòng này đã bị hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (checkStatus(id))
                    {
                        MessageBox.Show("Phòng này đã được trả rồi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (DateTime.TryParseExact(startDateStr, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime start) &&
                        DateTime.TryParseExact(endDateStr, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime end))
                    {
                        string status = "";

                         if (today >= start && today <= end)
                         {
                            DialogResult result =  MessageBox.Show("Xác nhận trả phòng?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information );
                            if (result == DialogResult.Yes)
                            {
                                status = "Đã trả phòng";

                                string updateSql = @"UPDATE checkins SET status = @status WHERE id = @id";
                                var parameters = new Dictionary<string, object>
                                {
                                    { "@status", status },
                                    { "@id", id }
                                };

                                Database_connect.ExecuteNonQuery(updateSql, parameters);
                                await load_check_in();
                            }
                         }
                        else
                        {
                            MessageBox.Show("Lỗi không thể hoàn thành tác vụ.\nLỗi: phòng chưa trong quá trình thuê", "Thông báo", MessageBoxButtons.OK);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi định dạng ngày " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void chỉnhSửaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = get_id();
            if (checkStatus(id))
            {
                MessageBox.Show("Phòng này đã được trả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                FormEditCheckin form = new FormEditCheckin(id, reloadData);
                form.ShowDialog();
            }
        }

        private bool checkStatus(int id)
        {
            string status = "";
            string sql = @"select status from checkins where id = @id";
            var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object> { { "@id", id } });
            foreach (var row in data)
            {
                status = row["status"].ToString();
            }
            if (status == "Đã trả phòng")
                return true;
            return false;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            UpdatePaginationInfo();
            int offset = (currentPage - 1) * pageSize;
            string time = dateTimePicker1.Value.ToString("yyyyMMdd").Substring(4, 2);
            dataGridView1.Rows.Clear();
            try
            {
                string sql = @"select checkins.id as checkins_id, rooms.name as room_name, tenants.name as tenants_name, 
                    tenants.phone as tenants_phone, start_date, end_date, rooms.type as r_type, deposit, checkins.status as c_status, 
                    rooms.price as price, bills.status as billStatus
                    from checkins
                    inner join rooms on checkins.room_id = rooms.id
                    inner join tenants on checkins.tenant_id = tenants.id
                    left join bills on checkins.id = bills.checkins_id
                    where substr(start_date, 5, 2) = @time or substr(end_date, 5, 2) = @time
                    limit @pageSize offset @offset";
                var data = await Task.Run(() => Database_connect.ExecuteReader(sql, new Dictionary<string, object>
                {
                    { "@pageSize", pageSize },
                    { "@offset", offset},
                    {"@time", time }
                }));
                foreach (var row in data)
                {
                    load_datagridview(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đã xảy ra: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            labelPageInfo.Text = $"Trang {currentPage}/{totalPages}";
        }
    }
}
