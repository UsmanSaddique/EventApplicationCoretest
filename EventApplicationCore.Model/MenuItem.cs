using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventApplicationCore.Model
{
    public class MenuItem
    {

        public int Id { get; set; }
        [Required]
        public String Name { get; set; }


        [Display(Name = "Description")]
        public String Discription { get; set; }

        [Display(Name="Sale type")]
        public String Spicyness { get; set; }

        public enum ESpicy { Normal=0,Sale=1,Exclusive=2,Limited=3 }

        public String  Image { get; set; }
        [Display(Name="Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        

        [Display(Name = "SubCategory")]
        public int SubCategoryId { get; set; }
        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; }

        [Range(1,int.MaxValue,ErrorMessage ="Price should be greater then ${1}")]
        public double Price { get; set; }

        [Display(Name = " Aviliable Sizes")]
        [MaxLength(10)]
        public string ItemSize { get; set; }



    }
}
