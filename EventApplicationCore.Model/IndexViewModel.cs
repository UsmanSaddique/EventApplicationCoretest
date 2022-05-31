using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventApplicationCore.Model
{
    public class IndexViewModel
    {
        public IEnumerable<Category> Category { get; set; }
        public IEnumerable<Coupon> Coupon { get; set; }

        public IEnumerable<MenuItem> MenuItem { get; set; }

    }
}
