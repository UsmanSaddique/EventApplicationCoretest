using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApplicationCore.Model
{
   public  class Area
    {
        [Key]
        public int Id { get; set; }
        public string AreaName { get; set; }
        public int? CityId { get; set; }
    }
}
