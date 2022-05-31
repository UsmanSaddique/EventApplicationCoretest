using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApplicationCore.Model
{
  public  class TourVM
    {
        public int Id { get; set; }
        public string TourName { get; set; }
        public string Discription { get; set; }
        public string TourFrom { get; set; }
        public string TourTo { get; set; }
        public int Cost { get; set; }
        public int PerHead { get; set; }
        public int TransportType { get; set; }
        public DateTime TourDate { get; set; }
  
        public DateTime EndDate { get; set; }
        public int Seats { get; set; }
    }
}
