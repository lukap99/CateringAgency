using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringAgency.Domain
{
    public class OrderDiscountBo
    {
        private int id = 1;
        private float discountAmount = 0;
        private string name = "No Discount";
        private int pointCost = 0;

        public OrderDiscountBo()
        {

        }

        public OrderDiscountBo(int id, float discountAmount, string name, int pointCost)
        {
            this.id = id;
            DiscountAmount = discountAmount;
            this.name = name;
            PointCost = pointCost;
        }

        public int Id { get => id; set => id = value; }
        public float DiscountAmount
        {
            get => discountAmount;
            set
            {
                if (value < 0) { discountAmount = 0; }
                else if (value > 100) { discountAmount = 100; }
                else discountAmount = value;
            }
        }
        public string Name { get => name; set => name = value; }
        public int PointCost { get => pointCost; set => pointCost = value < 0 ? 0 : value; }
    }
}
