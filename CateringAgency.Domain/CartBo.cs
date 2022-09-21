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

        private int pointDifference = 0;
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
            cartComboItems = new List<ComboMenuBo>();
            deliveryMethod = new DeliveryMethodBo();
            paymentMethod = new PaymentMethodBo();
            orderDiscount = new OrderDiscountBo();
        }

        public CartBo(UserBo userBo)
        {
            user = userBo;
            dateCreated = DateTime.Now;
            cartItems = new List<FoodBo>();
            cartComboItems = new List<ComboMenuBo>();
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
            this.pointDifference = pointsEarned;
            this.deliveryMethod = deliveryMethod;
            this.paymentMethod = paymentMethod;
            this.orderDiscount = orderDiscount;
        }



        #region Properties
        public UserBo User { get => user; set => user = value; }
        public DateTime DateCreated { get => dateCreated; set => dateCreated = value; }
        public DateTime DateCompleted { get => dateCompleted; set => dateCompleted = value; }
        public string DeliveryLocation { get => deliveryLocation; set => deliveryLocation = value; }
        public List<FoodBo> CartItems { get => cartItems; set => cartItems = value; }
        public List<ComboMenuBo> CartComboItems { get => cartComboItems; set => cartComboItems = value; }
        public float BasePrice
        {
            get => basePrice;
            set => basePrice = value < 0 ? 0 : value;
        }
        public string BasePriceString
        {
            get => String.Format("{0:0,0.00}", basePrice);
        }
        public float BuyingPrice
        {
            get => buyingPrice;
            set => buyingPrice = value < 0 ? 0 : value;
        }
        public string BuyingPriceString
        {
            get => String.Format("{0:0,0.00}", buyingPrice);
        }
        public int PointsDifference
        {
            get => pointDifference;
            set => pointDifference = value;
        }
        public int ItemCount
        {
            get => (this.CartItems.Count + this.CartComboItems.Count);
        }
        public DeliveryMethodBo DeliveryMethod { get => deliveryMethod; set => deliveryMethod = value; }
        public PaymentMethodBo PaymentMethod { get => paymentMethod; set => paymentMethod = value; }
        public OrderDiscountBo OrderDiscount { get => orderDiscount; set => orderDiscount = value; }
        #endregion

        // ----Methods----
        #region Methods
        public void CalculatePoints()
        {
            this.pointDifference = (int)Math.Round(basePrice / 500);
            if (this.orderDiscount.DiscountAmount > 0)
            {
                pointDifference = 0 - this.orderDiscount.PointCost;
            }
        }
        public void AddToCart(FoodBo food)
        {
            if (CartItems.Any(t=>t.FoodId == food.FoodId)) // if item already exists in list, just change the amount
            {
                CartItems.First(t => t.FoodId == food.FoodId).Amount = food.Amount;
            }
            else
            {
                cartItems.Add(food);
            }
        }

        public void UpdateItemAmount(int foodId, int amount)
        {
            if (CartItems.Any(t => t.FoodId == foodId)) // if item already exists in list, just change the amount
            {
                CartItems.FirstOrDefault(t => t.FoodId == foodId).Amount = amount;
            }
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
            cartComboItems.Clear();
        }
        public bool HasItems()
        {
            if (cartItems.Count() > 0 || cartComboItems.Count() > 0)
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
                    item.CalculatePrice();
                    sum += item.SellingPrice;
                }
                foreach(ComboMenuBo menu in cartComboItems)
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
