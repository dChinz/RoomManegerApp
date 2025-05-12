using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomManegerApp.Models
{
    public class RateReport
    {
        public string date {  get; set; }
        public int totalRoom {  get; set; }
        public int roomInUse {  get; set; }
        public double occupancyRate { get; set; }
    }
}
