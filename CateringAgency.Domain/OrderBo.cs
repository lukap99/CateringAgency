using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringAgency.Domain
{
    public class OrderBo
    {
        #region Fields
        private int orderId;
        private UserBo user;
        private DateTime dateCreated;
        private DateTime? dateCompleted;
        private string deliveryLocation;

        private List<FoodBo> orderItems;
        private List<ComboMenuBo> orderComboItems;

        private float basePrice = 0;
        private float buyingPrice = 0;

        private DeliveryMethodBo deliveryMethod;
        private PaymentMethodBo paymentMethod;
        private OrderDiscountBo orderDiscount;
        private bool isComplete = false;

        #endregion

        #region Ctor
        public OrderBo()
        {
            user = new UserBo();
            dateCreated = DateTime.Now;
            orderItems = new List<FoodBo>();
            orderComboItems = new List<ComboMenuBo>();
            deliveryMethod = new DeliveryMethodBo();
            paymentMethod = new PaymentMethodBo();
            orderDiscount = new OrderDiscountBo();
        }

        public OrderBo(int orderId, 
            UserBo user, 
            DateTime dateCreated, 
            DateTime dateCompleted, 
            string deliveryLocation, 
            List<FoodBo> orderItems, 
            List<ComboMenuBo> orderComboItems, 
            float basePrice, 
            float buyingPrice, 
            int pointsEarned, 
            DeliveryMethodBo deliveryMethod, 
            PaymentMethodBo paymentMethod, 
            OrderDiscountBo orderDiscount, 
            bool isComplete)
        {
            this.orderId = orderId;
            this.user = user;
            this.dateCreated = dateCreated;
            this.dateCompleted = dateCompleted;
            this.deliveryLocation = deliveryLocation;
            this.orderItems = orderItems;
            this.orderComboItems = orderComboItems;
            this.buyingPrice = buyingPrice;
            this.deliveryMethod = deliveryMethod;
            this.paymentMethod = paymentMethod;
            this.orderDiscount = orderDiscount;
            this.isComplete = isComplete;
        }
        #endregion

        #region Properties
        public int OrderId { get => orderId; set => orderId = value; }
        public UserBo User { get => user; set => user = value; }
        public DateTime DateCreated { get => dateCreated; set => dateCreated = value; }
        public DateTime? DateCompleted { get => dateCompleted; set => dateCompleted = value; }
        public string DeliveryLocation { get => deliveryLocation; set => deliveryLocation = value; }
        public List<FoodBo> OrderItems { get => orderItems; set => orderItems = value; }
        public List<ComboMenuBo> OrderComboItems { get => orderComboItems; set => orderComboItems = value; }
        public float BasePrice { get => basePrice; set => basePrice = value < 0 ? 0 : value; }
        public string BasePriceString
        {
            get => String.Format("{0:0.0,0}", basePrice);
        }
        public float BuyingPrice { get => buyingPrice; set => buyingPrice = value < 0 ? 0 : value; }
        public string BuyingPriceString
        {
            get => String.Format("{0:0.0,0}", buyingPrice);
        }
        public DeliveryMethodBo DeliveryMethod { get => deliveryMethod; set => deliveryMethod = value; }
        public PaymentMethodBo PaymentMethod { get => paymentMethod; set => paymentMethod = value; }
        public OrderDiscountBo OrderDiscount { get => orderDiscount; set => orderDiscount = value; }
        public bool IsComplete { get => isComplete; set => isComplete = value; }
        public int ItemCount
        {
            get => (this.OrderItems.Count + this.OrderComboItems.Count);
        }
        #endregion

        #region Methods

        public bool HasItems()
        {
            if (orderItems.Any() || orderComboItems.Any())
                return true;
            else return false;
        }
        public void CalculatePrice()
        {
            if (ItemCount > 0)
            {
                float sum = 0;
                foreach (FoodBo item in orderItems)
                {
                    item.CalculatePrice();
                    sum += item.SellingPrice;
                }
                foreach (ComboMenuBo menu in orderComboItems)
                {
                    menu.CalculatePrice();
                    sum += menu.SellingPrice;
                }
                basePrice = sum;
                buyingPrice = (sum * ((100 - orderDiscount.DiscountAmount) * 0.01f) + deliveryMethod.Price);
            }
        }
        #endregion
    }
}
