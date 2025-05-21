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

        private void FormAdd_bill_Load(object sender, EventArgs e)
        {
            load_add_bill();
        }

        private void load_add_bill()
        {
            string name = roomName;
            try
            {
                string sql = @"select tenants.name as t_name, rooms.name as r_name, checkins.start_date as c_s_date, checkins.end_date as c_e_date, rooms.type as r_type, rooms.price as r_price, rooms.size as r_size
                    from checkins
                    inner join rooms on checkins.room_id = rooms.id
                    inner join tenants on checkins.tenant_id = tenants.id
                    where rooms.name = @name";
                var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object> { { "@name", name } });
                foreach (var row in data)
                {
                    FillDataGridView(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillDataGridView(Dictionary<string, object> row)
        {
            int startDate = Convert.ToInt32(row["c_s_date"].ToString());
            int endDate = Convert.ToInt32(row["c_e_date"].ToString());
            DateTime start = DateTime.ParseExact(startDate.ToString(), "yyyyMMdd", null);
            DateTime end = DateTime.ParseExact(endDate.ToString(), "yyyyMMdd", null);
            int totalDays = (end - start).Days;

            label2.Text = row["r_name"].ToString();
            label4.Text = row["t_name"].ToString();
            label6.Text = totalDays.ToString();
            label8.Text = row["r_price"].ToString();
            label10.Text = row["r_type"].ToString();
            label13.Text = row["r_size"].ToString();
            double price = Convert.ToInt32(label6.Text) * Convert.ToInt32(label8.Text);
            label12.Text = string.Format(new CultureInfo("vi-VN"), "{0:N0} đ", price);
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            string name = roomName;
            string status = comboBox1.Text;
            string note = textBox1.Text;
            string total = label12.Text;
            string time = DateTime.Now.ToString("yyyyMMddHHmmss");
            total = total.Replace("đ", "").Replace(".", "").Trim();

            if (string.IsNullOrEmpty(status))
            {
                MessageBox.Show("Vui lòng chọn tình trạng thanh toán", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                int checkinId = 0;
                int endData = 0;
                string sql = @"select checkins.id as checkinId, end_date as endDate
                    from checkins
                    inner join rooms on checkins.room_id = rooms.id
                    where rooms.name = @name";
                var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object> { { "@name", name } });
                foreach(var row in data)
                {
                    checkinId = Convert.ToInt32(row["checkinId"].ToString());
                    endData = Convert.ToInt32(row["endDate"].ToString());
                }
                if(checkinId == 0)
                {
                    MessageBox.Show("Không tìm thấy thông tin check-in cho phòng này", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DateTime end = DateTime.ParseExact(endData.ToString(), "yyyyMMdd", null);
                DateTime now = DateTime.Now;
                if ((now - end).Days < 0)
                {
                    DialogResult result = MessageBox.Show("Chưa đến ngày checkout! Tiếp tục?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        sql = @"insert into bills (checkins_id, total, status, time) values (@checkins_id, @total, @status, @time)";
                        int rowAffected = Convert.ToInt16(Database_connect.ExecuteNonQuery(sql, new Dictionary<string, object>
                        {
                            { "@checkins_id", checkinId},
                            { "@total", total},
                            { "@status", status},
                            { "@time", time},
                        }));

                        if (rowAffected > 0)
                        {
                            sql = @"update rooms set status = 'Trống' where name = @name";
                            Database_connect.ExecuteNonQuery(sql, new Dictionary<string, object> { { "@name", name } });

                            sql = @"update checkins set status = 'Đã xử lý' where id = @id";
                            Database_connect.ExecuteNonQuery(sql, new Dictionary<string, object> { { "@id", checkinId } });

                            MessageBox.Show("Tạo mới thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _callback?.Invoke();
                            this.Close();
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
