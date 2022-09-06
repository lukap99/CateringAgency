using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringAgency.Domain
{
    class FoodItemDiscountBo
    {
        private int foodItemDiscountId;
        float discountAmount = 0;
        private DateTime dateCreated;
        private DateTime validFrom;
        private DateTime validUntil;

        public FoodItemDiscountBo()
        {
            dateCreated = DateTime.Now;
        }
        public FoodItemDiscountBo(int foodItemDiscountId, float discountAmount, DateTime validFrom, DateTime validUntil)
        {
            this.foodItemDiscountId = foodItemDiscountId;
            DiscountAmount = discountAmount;
            this.dateCreated = DateTime.Now;
            this.validFrom = validFrom;
            this.validUntil = validUntil;
        }

        public FoodItemDiscountBo(int foodItemDiscountId, float discountAmount, DateTime dateCreated, DateTime validFrom, DateTime validUntil)
        {
            this.foodItemDiscountId = foodItemDiscountId;
            DiscountAmount = discountAmount;
            this.dateCreated = dateCreated;
            this.validFrom = validFrom;
            this.validUntil = validUntil;
        }

        public int FoodItemDiscountId { get => foodItemDiscountId; set => foodItemDiscountId = value; }
        public float DiscountAmount
        {
            get
            {
                return discountAmount;
            }
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
        public DateTime DateCreated { get => dateCreated; set => dateCreated = value; }
        public DateTime ValidFrom { get => validFrom; set => validFrom = value; }
        public DateTime ValidUntil { get => validUntil; set => validUntil = value; }
        public bool IsValid
        {
            get
            {
                if (DateTime.Now > ValidFrom && DateTime.Now < ValidUntil && DiscountAmount > 0)
                {
                    return true;
                }
                else return false;
            }
        }

    }
}
