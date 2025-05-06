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
        public Role UserRole { get; set; }
        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;

        private void phòngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(UserRole == Role.Admin || UserRole == Role.Manager)
            {
                FormRooms rooms = new FormRooms();
                rooms.MdiParent = this;
                rooms.Dock = DockStyle.Fill;
                rooms.Show();
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập khu vực này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void kháchThuêToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserRole == Role.Admin || UserRole == Role.Manager)
            {
                FormTenant rooms = new FormTenant();
                rooms.MdiParent = this;
                rooms.Dock = DockStyle.Fill;
                rooms.Show();
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập khu vực này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void checkInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCheck_in form = new FormCheck_in();
            form.MdiParent = this;
            form.Dock = DockStyle.Fill;
            form.Show();
        }

        private void checkOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormBills form = new FormBills();
            form.MdiParent = this;
            form.Dock = DockStyle.Fill;
            form.Show();
        }

        private void FormDashboard_Load(object sender, EventArgs e)
        {
            createDTB();
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
                        note text)";
                using(thuchien = new SQLiteCommand(sql, ketnoi))
                    thuchien.ExecuteNonQuery();

                sql = @"create table if not exists tenants(
                        id integer primary key autoincrement,
                        name text,
                        phone text,
                        id_card text,
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
                    status text,
                    foreign key (checkins_id) references checkins(id) on delete cascade)";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                    thuchien.ExecuteNonQuery();
            }
        }

        private void thôngKêToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormReport f = new FormReport();
            f.MdiParent = this;
            f.Dock = DockStyle.Fill;
            f.Show();
        }
    }
}
