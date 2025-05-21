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

namespace RoomManegerApp.Bills
{
    public partial class FormRoom_checkined : Form
    {
        private Action _callback;

        public FormRoom_checkined(Action callback)
        {
            InitializeComponent();
            _callback = callback;
        }

        private void FormStatus_room_2_Load(object sender, EventArgs e)
        {
            load_status_room();
        }

        private void load_status_room()
        {
            string sql = @"select name, status from rooms";
            var data = Database_connect.ExecuteReader(sql);
            foreach(var row in data)
            {
                string roomName = row["name"].ToString();
                string status = row["status"].ToString();

                Button button = new Button();
                button.Text = roomName;
                button.Width = 70;
                button.Height = 40;
                button.Margin = new Padding(5);
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 1;
                button.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                if (status == "Đã thuê")
                {
                    button.BackColor = Color.White;
                    button.ForeColor = Color.DarkOrange;
                    button.FlatAppearance.BorderColor = Color.Red;

                    // Hover sáng lên nhẹ
                    button.MouseEnter += (s, e) => button.BackColor = Color.LightBlue;
                    button.MouseLeave += (s, e) => button.BackColor = Color.White;
                }
                else if (status == "Trống" || status == "Đang sửa chữa")
                {
                    button.ForeColor = Color.Gray;
                    button.Enabled = false;
                }

                button.Click += (s, e) =>
                {
                    DialogResult result = MessageBox.Show("Bạn muốn chọn phòng: " + roomName, "Thông báo", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        FormAdd_bill form = new FormAdd_bill(roomName, _callback);
                        form.Show();
                        this.Hide();
                    }
                    return;
                };

                flowLayoutPanel1.Controls.Add(button);
            }
        }
    }
}
