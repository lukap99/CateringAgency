using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringAgency.Domain
{
    public class FoodItemDiscountBo
    {
        float discountAmount = 0;

        public FoodItemDiscountBo()
        {
        }
        public FoodItemDiscountBo(float discountAmount)
        {
            DiscountAmount = discountAmount;
        }
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
