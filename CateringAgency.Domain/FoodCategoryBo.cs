using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringAgency.Domain
{
    public class FoodCategoryBo
    {
        private int foodCategoryId;
        private string name;

        [Display(Name = " Food Category")]
        public int FoodCategoryId { get => foodCategoryId; set => foodCategoryId = value; }

        [DataType(DataType.Text)]
        [Display(Name = "Category Name")]
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string Name { get => name; set => name = value; }

        public FoodCategoryDiscountBo CategoryDiscount { get; set; }
    }
}
