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
using Microsoft.Reporting.WinForms;
using RoomManegerApp.Check_in;
using RoomManegerApp.Models;

namespace RoomManegerApp.Report
{
    public partial class PrintBills : Form
    {
        private int idBills;
        public PrintBills(int id)
        {
            InitializeComponent();
            idBills = id;
        }

        private void reloadData()
        {
            lasttotal = 0;
            loadPanel(idBills);
            GetData();
        }

        private void PrintBills_Load(object sender, EventArgs e)
        {
            loadPanel(idBills);
            GetData();
            this.reportViewer1.RefreshReport();
        }

        private List<M_PrintBills> GetData()
        {
            var list = new List<M_PrintBills>();
            string sql = @"select tenants.name as t_name, rooms.name as r_name, rooms.type, rooms.price, checkins.start_date, checkins.end_date, users.username as userName, checkins.deposit
                        from checkins
                        inner join bills on bills.checkins_id = checkins.id
                        inner join rooms on checkins.room_id = rooms.id
                        inner join tenants on checkins.tenant_id = tenants.id
                        inner join users on bills.userId = users.id
                        where checkins.id = @id";
            var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object> { { "@id", idBills } });
            foreach (var row in data)
            {
                DateTime start_date = DateTime.ParseExact(row["start_date"].ToString(), "yyyyMMdd", null);
                string checkin = start_date.ToString("dd-MM-yyyy");
                DateTime end_date = DateTime.ParseExact(row["end_date"].ToString(), "yyyyMMdd", null);
                string checkout = end_date.ToString("dd-MM-yyyy");
                string price = row["price"].ToString();
                int days = (end_date - start_date).Days;
                string total = (Convert.ToDouble(price) * days).ToString("#,##0");
                string deposit = (Convert.ToDouble(row["deposit"])).ToString("#,##0");
                string AmountDue = (Convert.ToDouble(total) + lasttotal - Convert.ToDouble(deposit)).ToString("#,##0");
                labelTenKhachhang.Text = $"Tên khách hàng: {row["t_name"]}";
                labelPhòng.Text = $"Tên phòng: {row["r_name"]}";
                labelLoaiphong.Text = $"Loại phòng: {row["type"]}";
                labelChechin.Text = $"Ngày checkin: {checkin}";
                labelCheckout.Text = $"Ngày checkout: {checkout}";
                labelDemluutru.Text = $"Đêm lưu trú: {days}";
                labelTienPhong.Text = $"Tiền phòng: {total} VNĐ";
                labelTienDichVu.Text = $"Tiền dịch vụ: {lasttotal:N0} VNĐ";
                labelTongtien.Text = $"Tổng tiền: {(Convert.ToDouble(total) + lasttotal):N0} VNĐ";
                labelDeposit.Text = $"Tiền cọc: {deposit} VNĐ";
                labelTiencanThanhtoan.Text = $"Tiền cần thanh toán: {AmountDue} VNĐ";

                list.Add(new M_PrintBills
                {
                    t_name = row["t_name"].ToString(),
                    r_name = row["r_name"].ToString(),
                    type = row["type"].ToString(),
                    start_date = DateTime.ParseExact(row["start_date"].ToString(), "yyyyMMdd", null),
                    end_date = DateTime.ParseExact(row["end_date"].ToString(), "yyyyMMdd", null),
                    total = string.Format(new CultureInfo("vi-VN"), "{0:N0} đ", total),
                    user = row["userName"].ToString(),
                    deposit = string.Format(new CultureInfo("vi-VN"), "{0:N0} đ", deposit),
                    service = string.Format(new CultureInfo("vi-VN"), "{0:N0} đ", lasttotal),
                });
            }
            return list;
        }

        private void buttonTao_Click_1(object sender, EventArgs e)
        {
            List<M_PrintBills> list = GetData();

            ReportDataSource rds = new ReportDataSource("DataSet1", list);
            reportViewer1.LocalReport.ReportPath = "../../Report/PrintBills.rdlc";
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormAddService form = new FormAddService(idBills, reloadData);
            form.ShowDialog();
        }

        private double lasttotal = 0;
        private void loadPanel(int id)
        {
            int y = 10;
            panelService.Controls.Clear();
            string sql = @"select service_id, number, name, price from serviceDetails
                        inner join service on service.id = serviceDetails.service_id
                        where bill_id = @id";
            var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object> { { @"id", id} });
            foreach(var row in data)
            {
                string name = row["name"].ToString();
                int number = Convert.ToInt32(row["number"].ToString());
                double price = Convert.ToDouble(row["price"].ToString());
                double total = number * price;

                Label lbl = new Label()
                {
                    Text = $"{name} x {number} = {total:N0}đ",
                    Location = new Point(10, y),
                    AutoSize = true
                };

                panelService.Controls.Add(lbl);
                lasttotal += total;
                y += 20;
            }
        }

        private void panelService_DoubleClick(object sender, EventArgs e)
        {
            FormServiceDetails form = new FormServiceDetails(idBills);
            form.ShowDialog();
        }
    }
}
