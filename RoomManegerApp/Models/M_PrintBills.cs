using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomManegerApp.Models
{
    public class M_PrintBills
    {
        public string t_name {  get; set; }
        public string r_name { get; set; }
        public string type { get; set; }
        public DateTime start_date {  get; set; }
        public DateTime end_date { get; set; }
        public string total {  get; set; }
    }
}
