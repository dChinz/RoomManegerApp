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
using RoomManegerApp.Forms;

namespace RoomManegerApp.Contracts
{
    public partial class FormStatus_room : Form
    {
        public FormStatus_room()
        {
            InitializeComponent();
        }

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

        private void FormStatus_room_Load(object sender, EventArgs e)
        {
            load_status_room();
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel2.AutoScroll = true;
            flowLayoutPanel3.AutoScroll = true;
        }

        private void load_status_room()
        {
            sql = @"select name, status, type from rooms";
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    using (doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            string roomName = doc["name"].ToString();
                            string status = doc["status"].ToString();
                            string type = doc["type"].ToString();

                            Button button = new Button();
                            button.Text = roomName;
                            button.Width = 70;
                            button.Height = 40;
                            button.Margin = new Padding(5);
                            button.FlatStyle = FlatStyle.Flat;
                            button.FlatAppearance.BorderSize = 1;
                            button.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                            if (status == "Trống")
                            {
                                button.BackColor = Color.LightGreen;
                                button.ForeColor = Color.DarkGreen; // chữ xanh đậm
                                button.FlatAppearance.BorderColor = Color.SeaGreen; // viền xanh đậm

                                // Hover sáng lên nhẹ
                                button.MouseEnter += (s, e) => button.BackColor = Color.MediumSeaGreen;
                                button.MouseLeave += (s, e) => button.BackColor = Color.LightGreen;
                            }
                            else if(status == "Đã thuê" || status == "Đang sửa chữa")
                            {
                                button.ForeColor = Color.Gray;
                                button.Enabled = false;
                            }

                                button.Click += (s, e) =>
                                {
                                    DialogResult result =  MessageBox.Show("Bạn muốn chọn phòng: " + roomName, "Thông báo", MessageBoxButtons.YesNo);
                                    if (result == DialogResult.Yes)
                                    {
                                        FormAdd_check_in f = new FormAdd_check_in(roomName, type);
                                        f.Show();
                                        this.Hide();
                                    }
                                    return;
                                };
                            if(type == "Superior")
                            {
                                flowLayoutPanel1.Controls.Add(button);
                            }
                            else if(type == "Deluxe")
                            {
                                flowLayoutPanel2.Controls.Add(button);
                            }
                            else if (type == "Executive")
                            {
                                flowLayoutPanel3.Controls.Add(button);
                            }
                            else 
                            {
                                flowLayoutPanel4.Controls.Add(button);
                            }
                        }
                    }
                }
            }
        }
    }
}
