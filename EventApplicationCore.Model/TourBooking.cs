using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApplicationCore.Model
{
    public class TourBooking
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int NoOfPersons { get; set; }
        public int Total { get; set; }
        public int TourId { get; set; }
    }
}
