using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringAgency.Domain
{
    public class FoodCategoryDiscountBo
    {
        float discountAmount = 0;
        private DateTime creationDate;
        private DateTime validFrom;
        private DateTime validUntil;

        public FoodCategoryDiscountBo()
        {
            creationDate = DateTime.Now;
        }
        public FoodCategoryDiscountBo(float discountAmount, DateTime validFrom, DateTime validUntil)
        {
            DiscountAmount = discountAmount;
            this.creationDate = DateTime.Now;
            this.validFrom = validFrom;
            this.validUntil = validUntil;
        }
        public FoodCategoryDiscountBo(float discountAmount, DateTime creationDate, DateTime validFrom, DateTime validUntil)
        {
            DiscountAmount = discountAmount;
            this.creationDate = creationDate;
            this.validFrom = validFrom;
            this.validUntil = validUntil;
        }

        public int FoodCategoryDiscountId { get; set; }
        public float DiscountAmount
        {
            get { return discountAmount; }
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
        public DateTime CreationDate { get => creationDate; set => creationDate = value; }
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
