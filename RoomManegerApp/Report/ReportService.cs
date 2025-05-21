using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomManegerApp.Models;
using RoomManegerApp.ModelsReport;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace RoomManegerApp.Report
{
    public class ReportService
    {
        public List<ReportRevenue> GetRevenueReports(string timeType, int timeValue)
        {
            var list = new List<ReportRevenue>();
            var parameter = new Dictionary<string, object>();
            var condition = new List<string>();

            string time = timeValue.ToString("D2");
            string sql = @"SELECT
                        substr(start_date, 1, 4) || '-' ||
                        substr(start_date, 5, 2) || '-' ||
                        substr(start_date, 7, 2) as date,
                        count(*) as rent_count,
                        sum(bills.total) as total_revenue
                        from bills
                        inner join checkins on checkins.id = bills.checkins_id";
            TimeConditions(timeType, timeValue, condition, parameter);

            if (condition.Count > 0)
            {
                sql += " where " + string.Join(" and ", condition) + "\nGROUP by date\norder by date";
            }
            var data = Database_connect.ExecuteReader(sql, parameter);
            foreach (var row in data)
            {
                list.Add(new ReportRevenue
                {
                    date = row["date"].ToString(),
                    rentCount = row["rent_count"].ToString(),
                    revenueCount = Math.Round(Convert.ToDouble(row["total_revenue"]), 2)
                });
            }
            return list;
        }

        public List<RateReport> GetRateReports(string timeType, int timeValue)
        {
            var list = new List<RateReport>();
            var parameter = new Dictionary<string, object>();
            var condition = new List<string>();

            string time = timeValue.ToString("D2");
            string sql = @"SELECT
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
            
            TimeConditions(timeType, timeValue, condition, parameter);
            if (condition.Count > 0)
            {
                sql += " where " + string.Join(" and ", condition) + "\nGROUP by date\norder by date";
            }
            var data = Database_connect.ExecuteReader(sql, parameter);
            foreach (var row in data)
            {
                list.Add(new RateReport
                {
                    date = row["date"].ToString(),
                    totalRoom = Convert.ToInt32(row["totalRoom"]),
                    roomInUse = Convert.ToInt32(row["roomInUse"]),
                    occupancyRate = Convert.ToDouble(row["occupancyRate"])
                });
            }
            return list;
        }

        private void TimeConditions(string timeType, int timeValue, List<string> condition, Dictionary<string, object> parameter)
        {
            if (timeType == "Tháng")
            {
                condition.Add("strftime('%m', date(substr(start_date,1,4) || '-' || substr(start_date,5,2) || '-' || substr(start_date,7,2))) = @month");
                parameter.Add("@month", timeValue.ToString("D2"));
            }
            if (timeType == "Quý")
            {
                condition.Add("strftime('%m', date(substr(start_date,1,4) || '-' || substr(start_date,5,2) || '-' || substr(start_date,7,2))) between @startMonth and @endMonth");
                parameter.Add("@startMonth", (timeValue * 3 - 2).ToString("D2"));
                parameter.Add("@endMonth", (timeValue * 3).ToString("D2"));
            }
            if (timeType == "Năm")
            {
                condition.Add("strftime('%Y', date(substr(start_date,1,4) || '-' || substr(start_date,5,2) || '-' || substr(start_date,7,2))) = @year");
                parameter.Add("@year", timeValue.ToString());
            }
        }

        public List<GuestReport> GetGuestReports()
        {
            var list = new List<GuestReport>();
            string sql = @"select name, phone, id_card, gender, count(checkins.id) as checkinCount, sum(bills.total) as total
                    from tenants
                    inner join checkins on tenants.id = checkins.tenant_id
                    inner join bills on checkins.id = bills.checkins_id
                    group by name
                    order by name";
            var data = Database_connect.ExecuteReader(sql);
            foreach (var row in data)
            {
                list.Add(new GuestReport
                {
                    name = row["name"].ToString(),
                    phone = row["phone"].ToString(),
                    id_card = row["id_card"].ToString(),
                    gender = row["gender"].ToString(),
                    checkisCount = row["checkinCount"].ToString(),
                    total = row["total"].ToString()
                });
            }
            return list;
        }
    }
}
