using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApplicationCore.Model
{
   public class BookingVM
    {
     public   BookingEquipment bookingEquipment { get; set; }
        public BookingLight bookingLight { get; set; }
        public BookingFood bookingFood { get; set; }
        public BookingFlower bookingFlower { get; set; }

    }
}
