using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RoomManegerApp.Check_in
{
    public partial class FormEditCheckin : Form
    {
        private int id;
        private Action _callback;
        public FormEditCheckin(int id, Action callback)
        {
            InitializeComponent();
            this.id = id;
            _callback = callback;
        }

        private void FormEditCheckin_Load(object sender, EventArgs e)
        {
            load_form();
        }
        private void load_form()
        {
            string sql = @"select checkins.id as checkins_id, rooms.name as room_name, tenants.name as tenants_name, 
                    tenants.phone as tenants_phone, start_date, end_date, rooms.type as r_type, deposit, checkins.status as c_status, 
                    rooms.price as price, bills.status as billStatus
                    from checkins
                    inner join rooms on checkins.room_id = rooms.id
                    inner join tenants on checkins.tenant_id = tenants.id
                    left join bills on checkins.id = bills.checkins_id
                    where checkins.id = @id";
            var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object> { { "@id", id } });
            foreach (var row in data)
            {
                load_datagridview(row);
            }

        }

        private string status;

        private void load_datagridview(Dictionary<string, object> row)
        {
            string dbStart_date = row["start_date"].ToString();
            DateTime.TryParseExact(dbStart_date, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime start_date);
            string formattedStart_date = start_date.ToString("dd/MM/yyyy");

            string dbEnd_date = row["end_date"].ToString();
            DateTime.TryParseExact(dbEnd_date, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime end_date);
            string formattedEnd_date = end_date.ToString("dd/MM/yyyy");

            string price = Convert.ToInt32(row["price"]).ToString("#,##0");
            string deposit = Convert.ToInt32(row["deposit"]).ToString("#,##0");

            labelNameRoom.Text = row["room_name"].ToString();
            labelGuestname.Text = row["tenants_name"].ToString();

            status = row["c_status"].ToString();

            dateTimePickerCheckin.Value = start_date;
            dateTimePickerCheckout.Value = end_date;
        }

        private void buttonCapnhat_Click(object sender, EventArgs e)
        {
            try
            {
                string checkin = dateTimePickerCheckin.Value.ToString("yyyyMMdd");
                string checkout = dateTimePickerCheckout.Value.ToString("yyyyMMdd");
                DateTime now = DateTime.Now.Date;
                DateTime checkinStr = dateTimePickerCheckin.Value.Date;
                DateTime checkoutStr = dateTimePickerCheckout.Value.Date;

                if (checkinStr > checkoutStr)
                {
                    MessageBox.Show("Lỗi: ngày đặt phòng phải bé hơn ngày trả phòng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (status == "Đang thuê" || status == "Tới ngày trả phòng")
                {
                    string updateSql = @"update checkins set end_date = @checkout where id = @id";
                    Database_connect.ExecuteNonQuery(updateSql, new Dictionary<string, object>
                    {
                        {"@checkout", checkout },
                        {"@id", id }
                    });
                }
                else
                {

                    if (checkinStr < now)
                    {
                        MessageBox.Show("Lỗi: ngày đặt phòng phải lớn hơn hoặc bằng ngày hiện tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    string sql = @"update checkins set start_date = @checkin, end_date = @checkout where id = @id";
                    Database_connect.ExecuteNonQuery(sql, new Dictionary<string, object>
                    {
                        {"@checkin", checkin },
                        {"@checkout", checkout },
                        {"@id", id }
                    });
                }
                MessageBox.Show("Cập nhật ngày thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                _callback?.Invoke();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra: " +  ex.Message,"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
            
        }
    }
}
