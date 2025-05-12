using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using RoomManegerApp.Models;
using RoomManegerApp.ModelsReport;

namespace RoomManegerApp.Report
{
    public partial class FormReport : Form
    {
        public FormReport()
        {
            InitializeComponent();
        }

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

        private void FormReport_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }

        private void comboBoxReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            string report = comboBoxReport.Text;
            if( report == "Báo cáo khách hàng")
            {
                comboBoxSelectTime.Enabled = false;
                comboBoxTime.Enabled = false;
            }
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            string report = comboBoxReport.Text;

            if (report == "Báo cáo doanh thu" && comboBoxTime.SelectedItem != null)
            {
                string month = comboBoxTime.Text;
                List<ReportRevenue> reportRevenues = GetRevenueReports();

                ReportDataSource rds = new ReportDataSource("DataSet1", reportRevenues);
                reportViewer1.LocalReport.ReportPath = "../../Report/Report1.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.RefreshReport();
            }
            else if(report == "Báo cáo công suất phòng")
            {
                string month = comboBoxTime.Text;
                List<RateReport> rateReports = GetRateReports();

                ReportDataSource rds = new ReportDataSource("DataSet1", rateReports);
                reportViewer1.LocalReport.ReportPath = "../../Report/Report2.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.RefreshReport();
            }
            else
            {
                List<GuestReport> guestReports = GetGuestReports();

                ReportDataSource rds = new ReportDataSource("DataSet1", guestReports);
                reportViewer1.LocalReport.ReportPath = "../../Report/Report3.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.RefreshReport();
            }
        }

        private List<ReportRevenue> GetRevenueReports()
        {
            var list = new List<ReportRevenue>();
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                sql = @"SELECT
                        substr(start_date, 1, 4) || '-' ||
                        substr(start_date, 5, 2) || '-' ||
                        substr(start_date, 7, 2) as date,
                        count(*) as rent_count,
                        sum(bills.total) as total_revenue
                        from bills
                        inner join checkins on checkins.id = bills.checkins_id";
                if (comboBoxTime.Text == "Tháng")
                {
                    string condition = @" where substr(start_date, 5, 2) = @time
                                        GROUP by date
                                        order by date";
                    sql += condition;
                }
                if(comboBoxTime.Text == "Quý")
                {
                    string condition = @" where substr(start_date, 5, 2) between @startMonth and @endMonth
                                        GROUP by date
                                        order by date";
                    sql += condition;
                }
                if (comboBoxTime.Text == "Năm")
                {
                    string condition = @" where substr(start_date, 1, 4) = @time
                                        GROUP by date
                                        order by date";
                    sql += condition;
                }
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    int seletedTime = Convert.ToInt16(comboBoxSelectTime.SelectedItem);
                    string time = seletedTime.ToString("D2");
                    if(comboBoxTime.Text == "Tháng" || comboBoxTime.Text == "Năm")
                    {
                        thuchien.Parameters.AddWithValue("@time", time);
                    } 
                    if(comboBoxTime.Text == "Quý")
                    {
                        thuchien.Parameters.AddWithValue("@startMonth", (seletedTime * 3 - 2).ToString("D2"));
                        thuchien.Parameters.AddWithValue("@endMonth", (seletedTime * 3 ).ToString("D2"));
                    }
                    using (doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            list.Add(new ReportRevenue
                            {
                                date = doc["date"].ToString(),
                                rentCount = doc["rent_count"].ToString(),
                                revenueCount = Convert.ToDouble(doc["total_revenue"].ToString())
                            });
                        }
                    }
                }
            }
            
            return list;
        }

        List<RateReport> GetRateReports()
        {
            var list = new List<RateReport>();
            sql = @"SELECT
                    substr(start_date, 1, 4) || '-' ||
                    substr(start_date, 5, 2) || '-' ||
                    substr(start_date, 7, 2) as date,
                    (select count(*) from rooms) as totalRoom,
                    count(DISTINCT bills.id) as roomInUse,
                    round((count(distinct bills.id) * 100.0) /
                          (select count(*) from rooms), 2) as occupancyRate
                    from bills
                    inner join checkins on checkins.id = bills.checkins_id
                    inner join rooms on checkins.room_id = rooms.id";
            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                if (comboBoxTime.Text == "Tháng")
                {
                    string condition = @" where substr(start_date, 5, 2) = @time
                                        GROUP by date
                                        order by date";
                    sql += condition;
                }
                if (comboBoxTime.Text == "Quý")
                {
                    string condition = @" where substr(start_date, 5, 2) between @startMonth and @endMonth
                                        GROUP by date
                                        order by date";
                    sql += condition;
                }
                if (comboBoxTime.Text == "Năm")
                {
                    string condition = @" where substr(start_date, 1, 4) = @time
                                        GROUP by date
                                        order by date";
                    sql += condition;
                }
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    int seletedTime = Convert.ToInt16(comboBoxSelectTime.SelectedItem);
                    string time = seletedTime.ToString("D2");
                    if (comboBoxTime.Text == "Tháng" || comboBoxTime.Text == "Năm")
                    {
                        thuchien.Parameters.AddWithValue("@time", time);
                    }
                    if (comboBoxTime.Text == "Quý")
                    {
                        thuchien.Parameters.AddWithValue("@startMonth", (seletedTime * 3 - 2).ToString("D2"));
                        thuchien.Parameters.AddWithValue("@endMonth", (seletedTime * 3).ToString("D2"));
                    }
                    using (doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            list.Add(new RateReport
                            {
                                date = doc["date"].ToString(),
                                totalRoom = Convert.ToInt32(doc["totalRoom"].ToString()),
                                roomInUse = Convert.ToInt32(doc["roomInUse"].ToString()),
                                occupancyRate = Convert.ToDouble(doc["occupancyRate"].ToString())
                            });
                        }
                    }
                }
            }
            return list;
        }

        List<GuestReport> GetGuestReports()
        {
            var list = new List<GuestReport>();
            sql = @"select name, phone, id_card, gender, count(checkins.id) as checkinCount, sum(bills.total) as total
from tenants
inner join checkins on tenants.id = checkins.tenant_id
inner join bills on checkins.id = bills.checkins_id
group by name
order by name";
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    using (doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            list.Add(new GuestReport
                            {
                                name = doc["name"].ToString(),
                                phone = doc["phone"].ToString(),
                                id_card = doc["id_card"].ToString(),
                                gender = doc["gender"].ToString(),
                                checkisCount = doc["checkinCount"].ToString(),
                                total = doc["total"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        private void ComboBoxTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxSelectTime.Items.Clear();
            if (comboBoxTime.SelectedIndex == 0)
            {
                for(int i = 1; i <= 12; i++)
                {
                    comboBoxSelectTime.Items.Add(i.ToString());
                }
                int month = DateTime.Now.Month;
                comboBoxSelectTime.SelectedItem = month.ToString();
            }
            else if(comboBoxTime.SelectedIndex == 1)
            {
                for (int i = 1; i <= 4; i++)
                {
                    comboBoxSelectTime.Items.Add(i.ToString());
                }
                int month = DateTime.Now.Month;
                int quarter = (month - 1) / 3 + 1;
                comboBoxSelectTime.SelectedItem = quarter.ToString();
            }
            else if( comboBoxTime.SelectedIndex == 2)
            {
                int year = DateTime.Now.Year;
                for (int i = year - 5; i <= year+1; i++)
                {
                    comboBoxSelectTime.Items.Add(i.ToString());
                }
                comboBoxSelectTime.SelectedItem = year.ToString();
            }
        }
    }
}
