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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace RoomManegerApp.Report
{
    public partial class FormReport : Form
    {
        public FormReport()
        {
            InitializeComponent();
        }

        private void FormReport_Load(object sender, EventArgs e)
        {
            comboBoxReport.SelectedIndex = 0;
            comboBoxTime.SelectedIndex = 0;
            ComboBoxTime_SelectedIndexChanged(null, null);
            comboBoxSelectTime.SelectedItem = DateTime.Now.Month;

            this.BeginInvoke(new Action(() =>
            {
                buttonCreate.PerformClick();
            }));
        }

        private void comboBoxReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            string report = comboBoxReport.Text;
            if( report == "Báo cáo khách hàng")
            {
                comboBoxSelectTime.Enabled = false;
                comboBoxTime.Enabled = false;
            }
            else
            {
                comboBoxSelectTime.Enabled = true;
                comboBoxTime.Enabled = true;
                ComboBoxTime_SelectedIndexChanged(null, null);
            }
        }

        private ReportService reportService = new ReportService();

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            ReportDataSource rds;
            string report = comboBoxReport.Text;
            string timeType = comboBoxTime.Text;
            int timeValue = Convert.ToInt32(comboBoxSelectTime.Text);

            if (report == "Báo cáo doanh thu" && comboBoxTime.SelectedItem != null)
            {
                var data = reportService.GetRevenueReports(timeType, timeValue);

                rds = new ReportDataSource("DataSet1", data);
                reportViewer1.LocalReport.ReportPath = "../../Report/Report1.rdlc";
            }
            else if(report == "Báo cáo công suất phòng")
            {
                var data = reportService.GetRateReports(timeType, timeValue);
                rds = new ReportDataSource("DataSet1", data);
                reportViewer1.LocalReport.ReportPath = "../../Report/Report2.rdlc";
            }
            else
            {
                var data = reportService.GetGuestReports();
                rds = new ReportDataSource("DataSet1", data);
                reportViewer1.LocalReport.ReportPath = "../../Report/Report3.rdlc";
            }
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.RefreshReport();
        }

        private void ComboBoxTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTime.SelectedIndex >= 0)
            {
                LoadTimeOption(comboBoxTime.Text);
            }
        }

        private void LoadTimeOption(string type)
        {
            comboBoxSelectTime.Items.Clear();
            if (type == "Tháng")
            {
                for (int i = 1; i <= 12; i++)
                    comboBoxSelectTime.Items.Add(i.ToString());

                comboBoxSelectTime.SelectedItem = DateTime.Now.Month.ToString();
            }
            else if (type == "Quý")
            {
                for (int i = 1; i <= 4; i++)
                    comboBoxSelectTime.Items.Add(i.ToString());

                int quarter = (DateTime.Now.Month - 1) / 3 + 1;
                comboBoxSelectTime.SelectedItem = DateTime.Now.Month.ToString();
            }
            else if (type == "Năm")
            {
                int year = DateTime.Now.Year;
                for (int i = year - 5; i <= year + 1; i++)
                    comboBoxSelectTime.Items.Add(i.ToString());

                comboBoxSelectTime.SelectedItem = DateTime.Now.Month.ToString();
            }
        }
    }
}
