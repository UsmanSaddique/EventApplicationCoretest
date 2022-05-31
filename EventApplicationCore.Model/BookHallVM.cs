using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApplicationCore.Model
{
   public class BookHallVM
    {
       
        public int BookingHallID { get; set; }

        [Required(ErrorMessage = "Select Venue")]
        [Display(Description = "Venue Type")]
        public string VenueName { get; set; }

        [Required(ErrorMessage = "Select Event")]
        [Display(Description = " Event Type")]
        public string EventType { get; set; }

        [Required(ErrorMessage = "Required Guest Count")]
        [Display(Description = "No .Of Guest")]
        public int GuestCount { get; set; }

        public string Createdby { get; set; }

        public DateTime? CreatedDate { get; set; }


        public DateTime? BookingDate { get; set; }

        public int Cost { get; set; }
    }
}
