using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApplicationCore.Model
{
  public  class TourBookigngShowVM
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string TourName { get; set; }
        public string From { get; set; }
        public string TO { get; set; }
        public int NoOfPerson { get; set; }
        public int Total { get; set; }

    }
}
