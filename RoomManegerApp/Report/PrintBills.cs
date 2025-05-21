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

        private void PrintBills_Load(object sender, EventArgs e)
        {
            List<M_PrintBills> list = GetData();

            ReportDataSource rds = new ReportDataSource("DataSet1", list);
            reportViewer1.LocalReport.ReportPath = "../../Report/PrintBills.rdlc";
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.RefreshReport();

            this.reportViewer1.RefreshReport();
        }

        private List<M_PrintBills> GetData()
        {
            var list = new List<M_PrintBills>();
            string sql = @"select tenants.name as t_name, rooms.name as r_name, rooms.type, checkins.start_date, checkins.end_date, bills.total
                        from bills
                        inner join checkins on bills.checkins_id = checkins.id
                        inner join rooms on checkins.room_id = rooms.id
                        inner join tenants on checkins.tenant_id = tenants.id
                        where bills.id = @id";
            var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object> { { "@id", idBills } });
            foreach (var row in data)
            {
                list.Add(new M_PrintBills
                {
                    t_name = row["t_name"].ToString(),
                    r_name = row["r_name"].ToString(),
                    type = row["type"].ToString(),
                    start_date = DateTime.ParseExact(row["start_date"].ToString(), "yyyyMMdd", null),
                    end_date = DateTime.ParseExact(row["end_date"].ToString(), "yyyyMMdd", null),
                    total = string.Format(new CultureInfo("vi-VN"), "{0:N0} đ", row["total"])
                });
            }
            return list;
        }
    }
}
