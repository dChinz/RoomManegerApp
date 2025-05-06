using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using RoomManegerApp.Forms;
using RoomManegerApp.Tetants;

namespace RoomManegerApp.Contracts
{
    public partial class FormAdd_check_in : Form
    {
        private string nameRoom;
        private string type;
        private Action _callback;
        public FormAdd_check_in(string roomName, string roomType, Action callback)
        {
            InitializeComponent();
            nameRoom = roomName;
            type = roomType;
            _callback = callback;
        }
        

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

        private void FormAdd_contract_Load(object sender, EventArgs e)
        {
            load_add_contract();
        }
        private void load_add_contract()
        {
            labelNameRoom.Text = nameRoom;
            labelTypeRoom.Text = type;
        }

        private void buttonCapnhat_Click(object sender, EventArgs e)
        {
            string nameRoom = labelNameRoom.Text;
            string nameTenant = textBoxNameTenant.Text;
            int start_date = Convert.ToInt32(dateTimePicker1.Value.ToString("yyyyMMdd"));
            int end_date = Convert.ToInt32(dateTimePicker2.Value.ToString("yyyyMMdd"));
            int current_date = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
            string typeRoom = labelTypeRoom.Text;

            if (string.IsNullOrWhiteSpace(nameTenant) || !Regex.IsMatch(nameTenant, @"[^a-zA-z\s]"))
            {
                MessageBox.Show("Vui lòng nhập tên người thuê","Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (end_date < start_date || start_date < current_date)
            {
                MessageBox.Show("Lỗi dữ liệu ngày", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                try
                {
                    int idTenant = 0;
                    sql = @"select id from tenants where name = @name";
                    using (thuchien = new SQLiteCommand(sql, ketnoi))
                    {
                        thuchien.Parameters.AddWithValue("@name", nameTenant);
                        using (doc = thuchien.ExecuteReader())
                        {
                            if (doc.Read())
                            {
                                idTenant = Convert.ToInt32(doc["id"].ToString());
                            }
                            if (idTenant == 0)
                            {
                                MessageBox.Show("Dữ liệu chưa tồn tại. Cần thêm mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                FormAdd_one_tenant f = new FormAdd_one_tenant(nameTenant);
                                f.Show();
                            }
                        }
                    }

                    int idRoom = 0;
                    sql = @"select id from rooms where name = @name";
                    using (thuchien = new SQLiteCommand(sql, ketnoi))
                    {
                        thuchien.Parameters.AddWithValue("@name", nameRoom);
                        using (doc = thuchien.ExecuteReader())
                        {
                            if (doc.Read())
                            {
                                idRoom = Convert.ToInt32(doc["id"].ToString());
                            }
                        }
                    }

                    sql = @"insert into checkins (room_id, tenant_id, start_date, end_date, status) values (@room_id, @tenant_id, @start_date, @end_date, 'Còn hiệu lực')";
                    using (thuchien = new SQLiteCommand(sql, ketnoi))
                    {
                        thuchien.Parameters.AddWithValue("@room_id", idRoom);
                        thuchien.Parameters.AddWithValue("@tenant_id", idTenant);
                        thuchien.Parameters.AddWithValue("@start_date", start_date);
                        thuchien.Parameters.AddWithValue("@end_date", end_date);
                        thuchien.ExecuteNonQuery();
                    }

                    sql = @"update rooms set status = 'Đã thuê' where id = @id";
                    using (thuchien = new SQLiteCommand(sql, ketnoi))
                    {
                        thuchien.Parameters.AddWithValue("@id", idRoom);
                        thuchien.ExecuteNonQuery();
                    }

                    MessageBox.Show("Check in thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _callback?.Invoke();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
