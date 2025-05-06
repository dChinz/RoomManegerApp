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

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

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
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                sql = @"select tenants.name as t_name, rooms.name as r_name, rooms.type, checkins.start_date, checkins.end_date, bills.total
                        from bills
                        inner join checkins on bills.checkins_id = checkins.id
                        inner join rooms on checkins.room_id = rooms.id
                        inner join tenants on checkins.tenant_id = tenants.id
                        where bills.id = @id";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@id", idBills);
                    using (doc = thuchien.ExecuteReader())
                    {
                        if(doc.Read())
                        {
                            list.Add(new M_PrintBills
                            {
                                t_name = doc["t_name"].ToString(),
                                r_name = doc["r_name"].ToString(),
                                type = doc["type"].ToString(),
                                start_date = DateTime.ParseExact(doc["start_date"].ToString(), "yyyyMMdd", null),
                                end_date = DateTime.ParseExact(doc["end_date"].ToString(), "yyyyMMdd", null),
                                total = string.Format(new CultureInfo("vi-VN"), "{0:N0} đ", doc["total"])
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
}
