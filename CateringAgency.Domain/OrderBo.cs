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
        private DateTime dateCompleted;
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
        private UserBo User { get => user; set => user = value; }
        public DateTime DateCreated { get => dateCreated; set => dateCreated = value; }
        public DateTime DateCompleted { get => dateCompleted; set => dateCompleted = value; }
        public string DeliveryLocation { get => deliveryLocation; set => deliveryLocation = value; }
        internal List<FoodBo> OrderItems { get => orderItems; set => orderItems = value; }
        internal List<ComboMenuBo> OrderComboItems { get => orderComboItems; set => orderComboItems = value; }
        public float BasePrice { get => basePrice; set => basePrice = value < 0 ? 0 : value; }
        public float BuyingPrice { get => buyingPrice; set => buyingPrice = value < 0 ? 0 : value; }
        internal DeliveryMethodBo PaymentMethod { get => deliveryMethod; set => deliveryMethod = value; }
        internal PaymentMethodBo PaymentMethodBo { get => paymentMethod; set => paymentMethod = value; }
        internal OrderDiscountBo OrderDiscount { get => orderDiscount; set => orderDiscount = value; }
        public bool IsComplete { get => isComplete; set => isComplete = value; }
        #endregion

        #region Methods
        public void AddToCart(FoodBo foodBo)
        {
            orderItems.Add(foodBo);
        }
        public void ClearCart()
        {
            orderItems.Clear();
        }
        public bool HasItems()
        {
            if (orderItems.Count() > 0)
                return true;
            else return false;
        }
        public void CalculatePrice()
        {
            if (orderItems.Count > 0)
            {
                float sum = 0;
                foreach (FoodBo item in orderItems)
                {
                    sum += item.SellingPrice;
                }
                foreach (ComboMenuBo menu in orderComboItems)
                {
                    sum += menu.SellingPrice;
                }
                basePrice = sum;
                buyingPrice = basePrice * (1 - orderDiscount.DiscountAmount * (1 / 100));
            }
        }
        #endregion
    }
}
