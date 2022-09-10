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

        [ScaffoldColumn(false)]
        public int FoodCategoryId { get => foodCategoryId; set => foodCategoryId = value; }
        [DataType(DataType.Text)]
        public string Name { get => name; set => name = value; }
        public FoodCategoryDiscountBo CategoryDiscount { get; set; }
    }
}
