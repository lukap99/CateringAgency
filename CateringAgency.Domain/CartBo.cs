using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringAgency.Domain
{
    //Doesn't have "cartId" so I can make "orderBo" object with id
    public class CartBo
    {
        #region Fields
        private UserBo user;
        private DateTime dateCreated;
        private DateTime dateCompleted;
        private string deliveryLocation = "Takeout";
        private List<FoodBo> cartItems;
        private List<ComboMenuBo> cartComboItems;
        private float basePrice = 0;
        private float buyingPrice = 0;
        private int pointsEarned = 0;
        private DeliveryMethodBo deliveryMethod;
        private PaymentMethodBo paymentMethod;
        private OrderDiscountBo orderDiscount; // 1 - no discount, add pointsEarned to user points score in DB
        // 2,3,4... - Discounts, subtract OrderDiscountBo.pointCost from user points score in DB

        #endregion

        public CartBo()
        {
            user = new UserBo();
            dateCreated = DateTime.Now;
            cartItems = new List<FoodBo>();
            deliveryMethod = new DeliveryMethodBo();
            paymentMethod = new PaymentMethodBo();
            orderDiscount = new OrderDiscountBo();
        }

        public CartBo(UserBo user, 
            DateTime dateCreated, 
            DateTime dateCompleted, 
            string deliveryLocation, 
            List<FoodBo> cartItems, 
            float basePrice, 
            float buyingPrice, 
            int pointsEarned, 
            DeliveryMethodBo deliveryMethod, 
            PaymentMethodBo paymentMethod, 
            OrderDiscountBo orderDiscount
            )
        {
            this.user = user;
            this.dateCreated = dateCreated;
            this.dateCompleted = dateCompleted;
            this.deliveryLocation = deliveryLocation;
            this.cartItems = cartItems;
            this.basePrice = basePrice;
            this.buyingPrice = buyingPrice;
            this.pointsEarned = pointsEarned;
            this.deliveryMethod = deliveryMethod;
            this.paymentMethod = paymentMethod;
            this.orderDiscount = orderDiscount;
        }



        #region Properties
        private UserBo User { get => user; set => user = value; }
        public DateTime DateCreated { get => dateCreated; set => dateCreated = value; }
        public DateTime DateCompleted { get => dateCompleted; set => dateCompleted = value; }
        public string DeliveryLocation { get => deliveryLocation; set => deliveryLocation = value; }
        internal List<FoodBo> CartItems { get => cartItems; set => cartItems = value; }
        internal List<ComboMenuBo> CartComboItems { get => cartComboItems; set => cartComboItems = value; }
        public float BasePrice
        {
            get
            {
                this.CalculatePrice();
                return basePrice;
            }
            set => basePrice = value;
        }
        public float BuyingPrice
        {
            get
            {
                this.CalculatePrice();
                return buyingPrice;
            }
            set => buyingPrice = value;
        }
        public int PointsEarned
        {
            get
            {
                this.CalculatePoints();
                return pointsEarned;
            }
            set => pointsEarned = value;
        }
        internal DeliveryMethodBo PaymentMethod { get => deliveryMethod; set => deliveryMethod = value; }
        internal PaymentMethodBo PaymentMethodBo { get => paymentMethod; set => paymentMethod = value; }
        internal OrderDiscountBo OrderDiscount { get => orderDiscount; set => orderDiscount = value; }
        #endregion

        #region Methods
        public void CalculatePoints()
        {
            this.pointsEarned = (int)Math.Round(basePrice / 500);
            if (this.orderDiscount.DiscountAmount > 0)
            {
                pointsEarned = 0;
            }
        }
        public void AddToCart(FoodBo foodBo)
        {
            cartItems.Add(foodBo);
        }
        public void RemoveItemFromCart(int foodId)
        {
            cartItems.RemoveAt(cartItems.IndexOf(cartItems.Single(t => t.FoodId == foodId)));
        }
        public void RemoveItemFromCart(FoodBo food)
        {
            cartItems.RemoveAt(cartItems.IndexOf(cartItems.Single(t => t.FoodId == food.FoodId)));
        }
        public void ClearCart()
        {
            cartItems.Clear();
        }
        public bool HasItems()
        {
            if (cartItems.Count() > 0)
                return true;
            else return false;
        }
        public void CalculatePrice()
        {
            if (cartItems.Count > 0)
            {
                float sum = 0;
                foreach (FoodBo item in cartItems)
                {
                    sum += item.SellingPrice;
                }
                foreach(ComboMenuBo menu in cartComboItems)
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
