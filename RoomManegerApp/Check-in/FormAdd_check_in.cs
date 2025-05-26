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
        private string checkin;
        private string checkout;
        public FormAdd_check_in(string roomName, string guestName, string roomType, Action callback, string checkin, string checkout)
        {
            InitializeComponent();
            nameRoom = roomName;
            nameGuest = guestName;
            type = roomType;
            _callback = callback;
            this.checkin = checkin;
            this.checkout = checkout;
        }

        private void FormAdd_contract_Load(object sender, EventArgs e)
        {
            load_add_contract();
        }
        private void load_add_contract()
        {
            labelNameRoom.Text = nameRoom;
            labelTypeRoom.Text = type;
            labelGuestname.Text = nameGuest;
            labelChechin.Text = checkin;
            labelCheckout.Text = checkout;

        }

        private void buttonCapnhat_Click(object sender, EventArgs e)
        {
            string Room = nameRoom;
            string Tenant = nameGuest;
            DateTime.TryParseExact(labelChechin.Text, "dd-MM-yyyy",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out DateTime checkinDate);
            int start_date = Convert.ToInt32(checkinDate.ToString("yyyyMMdd"));

            DateTime.TryParseExact(labelCheckout.Text, "dd-MM-yyyy",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out DateTime checkoutDate);
            int end_date = Convert.ToInt32(checkoutDate.ToString("yyyyMMdd"));
            int current_date = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
            string typeRoom = labelTypeRoom.Text;
            string depositText = Regex.Replace(textBoxDeposit.Text, @"[^\d]", "");
            double deposit = double.Parse(depositText);
            string status_pay = comboBox1.Text;

            try
            {
                string sql = @"select id from tenants where name = @name";
                int idTenant = Convert.ToInt32(Database_connect.ExecuteScalar(sql, new Dictionary<string, object> { { "@name", Tenant } }));

                sql = @"select id from rooms where name = @name";
                int idRoom = Convert.ToInt16(Database_connect.ExecuteScalar(sql, new Dictionary<string, object> { { "@name", Room } }));

                sql = @"insert into checkins (room_id, tenant_id, start_date, end_date, deposit) values (@room_id, @tenant_id, @start_date, @end_date, @deposit)";
                long checkinId = Convert.ToInt16(Database_connect.ExecutiveInsertAndGetId(sql, new Dictionary<string, object>
                    {
                        { "@room_id", idRoom },
                        { "@tenant_id", idTenant},
                        { "@start_date", start_date},
                        { "@end_date", end_date},
                        { "@deposit", deposit},
                    }));

                sql = @"select price from rooms where type = @type";
                double price = Convert.ToDouble(Database_connect.ExecuteScalar(sql, new Dictionary<string, object> { { "@type", typeRoom } }));
                int soNgay = (checkoutDate - checkinDate).Days;
                double total = soNgay * price;

                sql = @"insert into bills (checkins_id, total, userId, status, create_date) values (@checkins_id, @total, @userId, @status, @create_date)";
                Database_connect.ExecuteNonQuery(sql, new Dictionary<string, object>
                    {
                        { "@checkins_id", checkinId },
                        { "@total", total},
                        { "@userId", Session.UserId},
                        { "@status", status_pay},
                        { "@create_date", DateTime.Now.ToString("yyyy-MM-dd")}
                    });

                MessageBox.Show("Check in thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _callback?.Invoke();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thao tác dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxDesposit_TextChanged(object sender, EventArgs e)
        {
            string raw = Regex.Replace(textBoxDeposit.Text, @"[^\d]", "");
            if (double.TryParse(raw, out double value))
            {
                textBoxDeposit.TextChanged -= textBoxDesposit_TextChanged; // tránh đệ quy
                textBoxDeposit.Text = string.Format("{0:N0}", value); // hiển thị 500,000
                textBoxDeposit.SelectionStart = textBoxDeposit.Text.Length;
                textBoxDeposit.TextChanged += textBoxDesposit_TextChanged;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == 0)
            {
                textBoxDeposit.Text = "500,000";
            }
            else
            {
                string typeRoom = labelTypeRoom.Text;
                DateTime.TryParseExact(labelChechin.Text, "dd-MM-yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime checkinDate);

                DateTime.TryParseExact(labelCheckout.Text, "dd-MM-yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime checkoutDate);

                string sql = @"select price from rooms where type = @type";
                double price = Convert.ToDouble(Database_connect.ExecuteScalar(sql, new Dictionary<string, object> { { "@type", typeRoom } }));
                int soNgay = (checkoutDate - checkinDate).Days;
                double total = soNgay * price;
                textBoxDeposit.Text = total.ToString("#,##0");
            }
        }
    }
}
