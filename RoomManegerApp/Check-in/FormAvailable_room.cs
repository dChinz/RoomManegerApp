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
using RoomManegerApp.Forms;

namespace RoomManegerApp.Contracts
{
    public partial class FormAvailable_room : Form
    {
        private Action _callback;

        public FormAvailable_room(Action callback)
        {
            InitializeComponent();
            _callback = callback;
        }

        private void FormAvailable_room_Load(object sender, EventArgs e)
        {
            //load_status_room();
            UpdateStatusRoom();
        }

        private void load_status_room()
        {
            string sql = @"select name, status, type, size from rooms";
            loadButton(sql);
        }

        private void loadButton(string sql, Dictionary<string, object> paragramers = null)
        {
            ClearRoomButtons();

            var data = Database_connect.ExecuteReader(sql, paragramers);
            foreach (var row in data)
            {
                string roomName = row["name"].ToString();
                string status = row["status"].ToString();
                string type = row["type"].ToString();
                string size = row["size"].ToString();
                string checkin = dateTimePicker1.Value.ToString("dd-MM-yyyy");
                string checkout = dateTimePicker2.Value.ToString("dd-MM-yyyy");

                Button button = new Button();
                button.Text = roomName;
                button.Width = 70;
                button.Height = 40;
                button.Margin = new Padding(5);
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 1;
                button.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                //if (status == "Trống")
                //{
                    if (size == "Đơn")
                    {
                        button.BackColor = Color.LightGreen;
                        button.ForeColor = Color.DarkGreen; // chữ xanh đậm
                        button.FlatAppearance.BorderColor = Color.SeaGreen; // viền xanh đậm

                        // Hover sáng lên nhẹ
                        button.MouseEnter += (s, e) => button.BackColor = Color.MediumSeaGreen;
                        button.MouseLeave += (s, e) => button.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        button.BackColor = Color.LightBlue;
                        button.ForeColor = Color.DarkBlue;
                        button.FlatAppearance.BorderColor = Color.SeaGreen;

                        // Hover sáng lên nhẹ
                        button.MouseEnter += (s, e) => button.BackColor = Color.DeepSkyBlue;
                        button.MouseLeave += (s, e) => button.BackColor = Color.LightBlue;
                    }
                //}
                //else if (status == "Đang thuê" || status == "Đang sửa chữa")
                //{
                //    button.ForeColor = Color.Gray;
                //    button.Enabled = false;
                //}

                button.Click += (s, e) =>
                {
                    DialogResult result = MessageBox.Show("Bạn muốn chọn phòng: " + roomName, "Thông báo", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        //FormAdd_check_in f = new FormAdd_check_in(roomName, type, _callback);
                        FormAvailable_Guest f = new FormAvailable_Guest(roomName, type, size, _callback, checkin, checkout);
                        f.ShowDialog();
                    }
                    return;
                };
                if (type == "Standard")
                {
                    flowLayoutPanel1.Controls.Add(button);
                }
                else if (type == "Superior")
                {
                    flowLayoutPanel2.Controls.Add(button);
                }
                else if (type == "Deluxe")
                {
                    flowLayoutPanel3.Controls.Add(button);
                }
                else if (type == "Executive")
                {
                    flowLayoutPanel4.Controls.Add(button);
                }
                else
                {
                    flowLayoutPanel5.Controls.Add(button);
                }
            }
        }

        private void UpdateStatusRoom()
        {
            string sql = @"select count(status) as total 
                    from rooms
                    where status = @status and type = @type";
            var roomType = new[] { "Standard", "Superior", "Deluxe", "Executive", "VIP" };
            var statuses = new[] { "Trống", "Đang thuê", "Đang sửa chữa" };

            var labelMap = new Dictionary<(string type, string status), Label>
            {
                {("Standard", "Trống"), label16 },
                {("Standard", "Đang thuê"), label18 },
                {("Standard", "Đang sửa chữa"), label20 },

                {("Superior", "Trống"), label23 },
                {("Superior", "Đang thuê"), label25 },
                {("Superior", "Đang sửa chữa"), label27 },

                {("Deluxe", "Trống"), label29 },
                {("Deluxe", "Đang thuê"), label31 },
                {("Deluxe", "Đang sửa chữa"), label33 },

                {("Executive", "Trống"), label35 },
                {("Executive", "Đang thuê"), label37 },
                {("Executive", "Đang sửa chữa"), label39 },

                {("VIP", "Trống"), label41 },
                {("VIP", "Đang thuê"), label43 },
                {("VIP", "Đang sửa chữa"), label45 },
            };

            foreach (var type in roomType)
            {
                foreach (var status in statuses)
                {
                    var key = (type, status);
                    if (labelMap.TryGetValue(key, out var label))
                    {
                        var parameters = new Dictionary<string, object>
                        {
                            {"@status", status },
                            {"@type", type }
                        };
                        label.Text = Database_connect.ExecuteScalar(sql, parameters).ToString();
                    }
                }
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if(dateTimePicker1.Value >= dateTimePicker2.Value)
            {
                ClearRoomButtons();
                return;
            }
                

            string checkin = dateTimePicker1.Value.ToString("yyyyMMdd");
            string checkout = dateTimePicker2.Value.ToString("yyyyMMdd");

            
            string sql = @"SELECT name, status, type, size
                        FROM rooms
                        WHERE id NOT IN (
                            SELECT room_id
                            FROM checkins
                            WHERE NOT (
                                end_date <= @checkin OR start_date >= @checkout
                            )
                        )";
            Dictionary<string, object> paragrames = new Dictionary<string, object>();
            paragrames.Add("@checkin", checkin);
            paragrames.Add("@checkout", checkout);
            loadButton(sql, paragrames);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value >= dateTimePicker2.Value)
            {
                ClearRoomButtons();
                return;
            }

            string checkin = dateTimePicker1.Value.ToString("yyyyMMdd");
            string checkout = dateTimePicker2.Value.ToString("yyyyMMdd");

            string sql = @"SELECT name, status, type, size
                        FROM rooms
                        WHERE id NOT IN (
                            SELECT room_id
                            FROM checkins
                            WHERE NOT (
                                end_date <= @checkin OR start_date >= @checkout
                            )
                        )";
            Dictionary<string, object> paragrames = new Dictionary<string, object>();
            paragrames.Add("@checkin", checkin);
            paragrames.Add("@checkout", checkout);
            loadButton(sql, paragrames);
        }

        private void ClearRoomButtons()
        {
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel2.Controls.Clear();
            flowLayoutPanel3.Controls.Clear();
            flowLayoutPanel4.Controls.Clear();
            flowLayoutPanel5.Controls.Clear();
        }

    }
}
