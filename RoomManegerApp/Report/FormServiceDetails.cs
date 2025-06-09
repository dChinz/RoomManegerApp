using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoomManegerApp.Report
{
    public partial class FormServiceDetails : Form
    {
        private int id;
        public FormServiceDetails(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void FormServiceDetails_Load(object sender, EventArgs e)
        {
            load_form();
            labelTotal.Text = "Tiền dịch vụ: " + total.ToString("N0") + "đ";
        }
        double total = 0;
        private void load_form()
        {
            int y = 10;
            int x = 10;
            int columnWidth = 200; // mỗi cột cách nhau 200px (tuỳ chỉnh theo nhu cầu)

            string sql = @"SELECT service_id, number, service.name as serviceName, service.price as servicePrice, 
                      rooms.name as roomName, tenants.name as tenantName
               FROM serviceDetails
               INNER JOIN service ON service.id = serviceDetails.service_id
               INNER JOIN bills ON serviceDetails.bill_id = bills.id
               INNER JOIN checkins ON checkins.id = bills.checkins_id
               INNER JOIN rooms ON checkins.room_id = rooms.id
               INNER JOIN tenants ON checkins.tenant_id = tenants.id
               WHERE bill_id = @id";

            var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object> { { "@id", id } });

            panel1.Controls.Clear();

            foreach (var row in data)
            {
                labelRoomName.Text = "Tên phòng: " + row["roomName"].ToString();
                labelTenantName.Text = "Tên khách hàng: " + row["tenantName"].ToString();

                string serviceName = row["serviceName"].ToString();
                int number = Convert.ToInt32(row["number"].ToString());
                double price = Convert.ToDouble(row["servicePrice"].ToString());
                total += number * price;

                var lbl = new Label()
                {
                    Text = $"{serviceName} x {number} = {number * price:N0} đ",
                    Location = new Point(x, y),
                    AutoSize = true,
                };

                panel1.Controls.Add(lbl);
                y += 25;

                // Nếu quá chiều cao panel thì chuyển sang cột kế bên
                if (y + 25 > panel1.Height)
                {
                    y = 10;
                    x += columnWidth;
                }
            }
        }
    }
}
