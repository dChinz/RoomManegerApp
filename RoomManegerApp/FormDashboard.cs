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
            loadTodayBooking();
            loadTodayRevenue();
        }
        public Role UserRole { get; set; }
        private Timer bookingTimer;

        private async void FormDashboard_Load(object sender, EventArgs e)
        {
            createDTB();
            loadUI();
            await loadRoomStatus();
            loadTodayRevenue();
            loadTodayBooking();
            updateBooking();
        }

        private void loadUI()
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

            timerNow.Interval = 1000;
            timerNow.Tick += timerNow_Tick;
            timerNow.Start();

            bookingTimer = new Timer();
            bookingTimer.Interval = 30000;
            bookingTimer.Tick += BookingTimer_Tick;
            bookingTimer.Start();

            labelDangXuat.MouseEnter += (s, e) => labelDangXuat.BackColor = Color.LightGreen;
            labelDangXuat.MouseLeave += (s, e) => labelDangXuat.BackColor = SystemColors.ActiveCaption;
        }
        private async Task loadRoomStatus()
        {
            int emptyRoom = await countStatusRoom("Trống");
            int rentedRoom = await countStatusRoom("Đã thuê");
            label2.Text = "Phòng trống: " + emptyRoom;
            label3.Text = "Đang thuê: " + rentedRoom;
        }

        private void loadTodayBooking() 
        {
            try
            {
                int todayBooking = Convert.ToInt32(Database_connect.ExecuteScalar(@"select count(*) from checkins
                    where status = @status and start_date = @time",
                    new Dictionary<string, object> {
                        { "@status", "Còn hiệu lực" },
                        { "@time", DateTime.Now.ToString("yyyyMMdd") }
                    }));
                label4.Text = "Thuê hôm nay: " + todayBooking.ToString();
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Lỗi " +  ex.Message,"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        } 

        private void loadTodayRevenue() 
        {
            try
            {
                double todayRevenue = Convert.ToDouble(Database_connect.ExecuteScalar(
                @"select ifnull(sum(total), 0) as total from bills
                    where substr(time, 1, 8) = @time",
                new Dictionary<string, object> {
                    { "@time", DateTime.Now.ToString("yyyyMMdd") }
                }));
                label5.Text = "Doanh thu: " + string.Format(new CultureInfo("vi-VN"), "{0:N0}", todayRevenue);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<int> countStatusRoom(string status)
        {
            string sql = "select count(*) from rooms where status = @status";
            try
            {
                int total = await Task.Run(() =>
                    Convert.ToInt32(Database_connect.ExecuteScalar(sql, new Dictionary<string, object> { { "@status", status } })
                ));
                return total;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            
        }

        public void createDTB()
        {
            string sql;
            SQLiteCommand command;
            using(var conn = Database_connect.connection())
            {
                conn.Open();

                using (command = new SQLiteCommand("PRAGMA journal_mode = WAL;", conn))
                    command.ExecuteNonQuery();

                using (command = new SQLiteCommand("PRAGMA busy_timeout = 5000;", conn))
                    command.ExecuteNonQuery();

                sql = @"create table if not exists rooms(
                        id integer primary key autoincrement,
                        name text,
                        status text,
                        type text,
                        price real,
                        size text,
                        note text)";
                using(command = new SQLiteCommand(sql, conn))
                    command.ExecuteNonQuery();

                sql = @"create table if not exists tenants(
                        id integer primary key autoincrement,
                        name text,
                        phone text,
                        id_card text,
                        email text,
                        gender text,
                        address text)";
                using (command = new SQLiteCommand(sql, conn))
                    command.ExecuteNonQuery();

                sql = @"create table if not exists users(
                        id integer primary key autoincrement,
                        username text,
                        password text,
                        role text)";
                using (command = new SQLiteCommand(sql, conn))
                    command.ExecuteNonQuery();

                sql = @"create table if not exists checkins(
                    id integer primary key autoincrement,
                    room_id integer,
                    tenant_id integer,
                    start_date integer,
                    end_date integer,
                    status text,
                    foreign key (room_id) references rooms(id) on delete cascade,
                    foreign key (tenant_id) references tenants(id) on delete cascade);";
                using (command = new SQLiteCommand(sql, conn))
                    command.ExecuteNonQuery();

                sql = @"create table if not exists bills(
                    id integer primary key autoincrement,
                    checkins_id integer,
                    total real,
                    time integer,
                    status text,
                    foreign key (checkins_id) references checkins(id) on delete cascade)";
                using (command = new SQLiteCommand(sql, conn))
                    command.ExecuteNonQuery();

                sql = @"create table if not exists service(
                    id integer primary key autoincrement,
                    name text,
                    price real)";
                using (command = new SQLiteCommand(sql, conn))
                    command.ExecuteNonQuery();
            }
        }

        private void timerNow_Tick(object sender, EventArgs e)
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
            if(UserRole == Role.Staff)
            {
                MessageBox.Show("Bạn không được phân quyền tại đây");
                return;
            }
            else
            {
                FormRooms form = new FormRooms();
                LoadFormToTableLayout(form, 1, 1);
            }
        }

        private void QLDatPhong_Click(object sender, EventArgs e)
        {
            FormCheck_in form = new FormCheck_in(this);
            LoadFormToTableLayout(form, 1, 1);
        }

        private void QLKhachHang_Click(object sender, EventArgs e)
        {
            if (UserRole == Role.Staff)
            {
                MessageBox.Show("Bạn không được phân quyền tại đây");
                return;
            }
            else
            {
                FormTenant form = new FormTenant();
                LoadFormToTableLayout(form, 1, 1);
            }
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

        private void updateBooking()
        {
            try
            {
                int count = Convert.ToInt32(Database_connect.ExecuteScalar("select count(*) from booking"));
                booking.Text = $"Booking ({count})";
                booking.BackColor = count > 0 ? Color.Aqua : SystemColors.ControlLight;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi" + ex.Message,"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BookingTimer_Tick(object sender, EventArgs e)
        {
            loadUI();
            updateBooking();
        }
    }
}
