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
using RoomManegerApp.Services;
using RoomManegerApp.Users;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
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
        private Timer bookingTimer;

        private void FormDashboard_Load(object sender, EventArgs e)
        {
            createDTB();
            loadUI();
            loadRoomStatus();
            loadTodayRevenue();
            loadTodayBooking();
            updateBooking();

            FormCheck_in form = new FormCheck_in(this);
            LoadFormToTableLayout(form, 1, 1);
        }

        private void loadUI()
        {
            labelRole.Text = Session.Role;

            timerNow.Interval = 1000;
            timerNow.Tick += timerNow_Tick;
            timerNow.Start();

            if (bookingTimer == null)
            {
                bookingTimer = new Timer();
                bookingTimer.Interval = 1000;
                bookingTimer.Tick += BookingTimer_Tick;
                bookingTimer.Start();
            }

            labelDangXuat.MouseEnter += (s, e) => labelDangXuat.BackColor = Color.SkyBlue;
            labelDangXuat.MouseLeave += (s, e) => labelDangXuat.BackColor = SystemColors.ActiveCaption;
        }
        private void loadRoomStatus()
        {
            string sql = "select count(*) from rooms where status = @status";
            try
            {
                label2.Text = "Phòng trống: " + Convert.ToInt32(Database_connect.ExecuteScalar(sql, new Dictionary<string, object> { { "@status", "Trống" } }));
                label3.Text = "Đang thuê: " + Convert.ToInt32(Database_connect.ExecuteScalar(sql, new Dictionary<string, object> { { "@status", "Đang thuê" } }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadTodayBooking()
        {
            try
            {
                string sql = @"select count(*) from checkins where status = @status and start_date = @time";
                int todayBooking = Convert.ToInt32(Database_connect.ExecuteScalar(sql,
                    new Dictionary<string, object> {
                        { "@status", "Đang thuê" },
                        { "@time", DateTime.Now.ToString("yyyyMMdd")}
                    }));
                label4.Text = "Thuê hôm nay: " + todayBooking.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void loadTodayRevenue()
        {
            try
            {
                double checkinRevenue = 0;
                string sql = @"select deposit from checkins where start_date = @time";
                var datacheckin = Database_connect.ExecuteReader(sql, new Dictionary<string, object>
                {
                    { "@time", DateTime.Now.ToString("yyyyMMdd")}
                });
                foreach(var row in datacheckin)
                {
                    checkinRevenue += Convert.ToDouble(row["deposit"]);
                }

                double checkoutRevenue = 0;
                sql = @"select start_date, end_date, rooms.price as price from checkins
                        inner join rooms on checkins.room_id = rooms.id
                        where end_date = @time";
                var datacheckout = Database_connect.ExecuteReader(sql, new Dictionary<string, object>
                {
                    { "@time", DateTime.Now.ToString("yyyyMMdd")}
                });
                foreach (var row in datacheckout)
                {
                    DateTime.TryParseExact(row["start_date"].ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime checkin);
                    DateTime.TryParseExact(row["end_date"].ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime checkout);

                    double price = Convert.ToDouble(row["price"].ToString());

                    checkoutRevenue += (checkout - checkin).Days * price;
                }

                double todayRevenue = checkinRevenue + checkoutRevenue;

                label5.Text = "Doanh thu: " + string.Format(new CultureInfo("vi-VN"), "{0:N0} VNĐ", todayRevenue);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    desposit real,
                    foreign key (room_id) references rooms(id) on delete cascade,
                    foreign key (tenant_id) references tenants(id) on delete cascade);";
                using (command = new SQLiteCommand(sql, conn))
                    command.ExecuteNonQuery();

                sql = @"create table if not exists bills(
                    id integer primary key autoincrement,
                    checkins_id integer,
                    total real,
                    staff text,
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
            try
            {
                labelTime.Text = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
            }
            catch (Exception ex) 
            {
                Debug.WriteLine("Lỗi ở timerNow_Tick: " + ex.Message);
            }
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
            if(Session.Role == "Staff")
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
        private void QLUser_Click(object sender, EventArgs e)
        {
            if(Session.Role == "Staff")
            {
                MessageBox.Show("Bạn không được phân quyền tại đây");
                return;
            }
            FormUser form = new FormUser();
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
            try
            {
                updateBooking();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Lỗi ở BookingTimer_Tick: " + ex.Message);
            }
        }

        private void QLiDichVu_Click(object sender, EventArgs e)
        {
            FormService form = new FormService();
            LoadFormToTableLayout(form, 1, 1);
        }
    }
}
