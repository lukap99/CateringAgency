using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringAgency.Domain
{
    public class FoodItemDiscountBo
    {
        float discountAmount;

        public FoodItemDiscountBo()
        {
            DiscountAmount = 0;
        }
        public FoodItemDiscountBo(float discountAmount)
        {
            DiscountAmount = discountAmount;
        }

        [Required(ErrorMessage = "Food item must be entered. Enter '0' for no discount")]
        [Display(Name = " Discount")]
        [Range(minimum: 0, maximum: 100, ErrorMessage = "Discount must be between 0 and 100")]
        public float DiscountAmount
        {
            get => discountAmount;
            set
            {
                if (value < 0)
                {
                    discountAmount = 0;
                }
                else if (value > 100)
                {
                    discountAmount = 100;
                }
                else
                {
                    discountAmount = value;
                }
            }
        }
        public bool IsValid
        {
            get
            {
                if (DiscountAmount > 0)
                    return true;
                else
                    return false;
            }
        }
    }
}
