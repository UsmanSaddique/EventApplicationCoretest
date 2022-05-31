using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApplicationCore.Model
{
  public  class Tour
    {
        [Key]
        public int Id { get; set; }
        public string TourName { get; set; }
        public string Discription { get; set; }
        public int TourFrom { get; set; }
        public int TourTo { get; set; }
        public int Cost { get; set; }
        public int PerHead { get; set; }
        public int TransportType { get; set; }
        public DateTime TourDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
