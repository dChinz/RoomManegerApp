using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RoomManegerApp.Booking;
using RoomManegerApp.Report;
using static RoomManegerApp.FormDangNhap;

namespace RoomManegerApp.Forms
{
    public partial class FormDashboard : Form
    {
        public FormDashboard()
        {
            InitializeComponent();
            
        }
        public void reloadData()
        {
            load_form();
        }
        public Role UserRole { get; set; }
        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

        private void FormDashboard_Load(object sender, EventArgs e)
        {
            createDTB();
            load_form();
        }

        private void load_form()
        {
            if (UserRole == Role.Admin)
            {
                labelRole.Text = "Admin";
            }
            else if (UserRole == Role.Manager)
            {
                labelRole.Text = "Manager";
            }
            else
            {
                labelRole.Text = "Staff";
            }

            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            timer1.Start();
            labelDangXuat.MouseEnter += (s, e) => labelDangXuat.BackColor = Color.LightGreen;
            labelDangXuat.MouseLeave += (s, e) => labelDangXuat.BackColor = SystemColors.ActiveCaption;
            label2.Text = "Phòng trống: " + countStatusRoom("Trống");
            label3.Text = "Đang thuê: " + countStatusRoom("Đã thuê");

            
            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                sql = @"select count(*) as count from checkins
                    where status = 'Còn hiệu lực' and start_date = @time";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@time", DateTime.Now.ToString("yyyyMMdd"));
                    using(doc = thuchien.ExecuteReader())
                    {
                        if (doc.Read())
                        {
                            label4.Text = "Thuê hôm nay: " + doc["count"].ToString();
                        }
                    }
                }

                sql = @"select ifnull(sum(total), 0) as total from bills
                    where substr(time, 1, 8) = @time";
                using(thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@time", DateTime.Now.ToString("yyyyMMdd"));
                    using (doc = thuchien.ExecuteReader())
                    {
                        if (doc.Read())
                        {
                            double total = Convert.ToDouble(doc["total"].ToString());
                            label5.Text = "Doanh thu: " + string.Format(new CultureInfo("vi-VN"), "{0:N0} đ", total);
                        }
                    }
                }
            }
        }

        private int countStatusRoom(string status)
        {
            sql = @"select count(status) as total 
                    from rooms
                    where status = @status";
            int total = 0;
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@status", status);
                    using (doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            total = Int32.Parse(doc[0].ToString());
                        }
                    }
                }
            }
            return total;
        }

        public void createDTB()
        {
            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();

                using (var cmd = new SQLiteCommand("PRAGMA journal_mode = WAL;", ketnoi))
                    cmd.ExecuteNonQuery();

                using (var cmd = new SQLiteCommand("PRAGMA busy_timeout = 5000;", ketnoi))
                    cmd.ExecuteNonQuery();

                sql = @"create table if not exists rooms(
                        id integer primary key autoincrement,
                        name text,
                        status text,
                        type text,
                        price real,
                        size text,
                        note text)";
                using(thuchien = new SQLiteCommand(sql, ketnoi))
                    thuchien.ExecuteNonQuery();

                sql = @"create table if not exists tenants(
                        id integer primary key autoincrement,
                        name text,
                        phone text,
                        id_card text,
                        email text,
                        gender text,
                        address text)";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                    thuchien.ExecuteNonQuery();

                sql = @"create table if not exists users(
                        id integer primary key autoincrement,
                        username text,
                        password text,
                        role text)";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                    thuchien.ExecuteNonQuery();

                sql = @"create table if not exists checkins(
                    id integer primary key autoincrement,
                    room_id integer,
                    tenant_id integer,
                    start_date integer,
                    end_date integer,
                    status text,
                    foreign key (room_id) references rooms(id) on delete cascade,
                    foreign key (tenant_id) references tenants(id) on delete cascade);";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                    thuchien.ExecuteNonQuery();

                sql = @"create table if not exists bills(
                    id integer primary key autoincrement,
                    checkins_id integer,
                    total real,
                    time integer,
                    status text,
                    foreign key (checkins_id) references checkins(id) on delete cascade)";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                    thuchien.ExecuteNonQuery();

                sql = @"create table if not exists service(
                    id integer primary key autoincrement,
                    name text,
                    price real)";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                    thuchien.ExecuteNonQuery();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelTime.Text = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
        }

        private void LoadFormToTableLayout(Form formToLoad, int row, int column)
        {
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel1.Visible = false;

            Control oldForm = tableLayoutPanel1.GetControlFromPosition(column, row);
            if (oldForm != null)
            {
                tableLayoutPanel1.Controls.Remove(oldForm);
                oldForm.Dispose();
            }

            formToLoad.TopLevel = false;                     // Không phải là form độc lập
            formToLoad.FormBorderStyle = FormBorderStyle.None; // Bỏ viền
            formToLoad.Dock = DockStyle.Fill;                // Tự động lấp đầy ô

            tableLayoutPanel1.Controls.Add(formToLoad, column, row); // Thêm form vào vị trí cụ thể
            formToLoad.Show(); // Hiển thị form

            tableLayoutPanel1.Visible = true;
            tableLayoutPanel1.ResumeLayout();
        }

        private void QLPhong_Click(object sender, EventArgs e)
        {
            FormRooms form = new FormRooms();
            LoadFormToTableLayout(form, 1, 1);
        }

        private void QLDatPhong_Click(object sender, EventArgs e)
        {
            FormCheck_in form = new FormCheck_in(this);
            LoadFormToTableLayout(form, 1, 1);
        }

        private void QLKhachHang_Click(object sender, EventArgs e)
        {
            FormTenant form = new FormTenant();
            LoadFormToTableLayout(form, 1, 1);
        }

        private void ThanhToan_Click(object sender, EventArgs e)
        {
            FormBills form = new FormBills(this);
            LoadFormToTableLayout(form, 1, 1);
        }

        private void BaoCao_Click(object sender, EventArgs e)
        {
            FormReport form = new FormReport();
            LoadFormToTableLayout(form, 1, 1);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void booking_Click(object sender, EventArgs e)
        {
            FormBooking form = new FormBooking();
            LoadFormToTableLayout(form, 1, 1);
        }
    }
}
