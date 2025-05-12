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
        private string nameGuest;
        private string type;
        private Action _callback;
        public FormAdd_check_in(string roomName, string guestName, string roomType, Action callback)
        {
            InitializeComponent();
            nameRoom = roomName;
            nameGuest = guestName;
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
            labelGuestname.Text = nameGuest;
            labelStart_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void buttonCapnhat_Click(object sender, EventArgs e)
        {
            string Room = nameRoom;
            string Tenant = nameGuest;
            int start_date = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
            int end_date = Convert.ToInt32(dateTimePicker2.Value.ToString("yyyyMMdd"));
            int current_date = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
            string typeRoom = labelTypeRoom.Text;

            if (end_date <= start_date || start_date < current_date)
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
                        thuchien.Parameters.AddWithValue("@name", Tenant);
                        using (doc = thuchien.ExecuteReader())
                        {
                            if (doc.Read())
                            {
                                idTenant = Convert.ToInt32(doc["id"].ToString());
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
