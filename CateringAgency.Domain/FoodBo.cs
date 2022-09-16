using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringAgency.Domain
{
    public class FoodBo
    {
        #region Fields
        private int foodId; // Can be ComboMenuItemId when used in ComboMenu
        private string name;
        private FoodCategoryBo foodCategory;
        private FoodItemDiscountBo discount;
        private string unitOfMeasurement;
        private string ingredients;

        private float basePrice;
        private float sellingPrice;

        private int amount = 1; // previous default = 1
        private bool isVisible = false;
        #endregion

        public FoodBo()
        {

        }

        #region Properties
        public int FoodId { get => foodId; set => foodId = value; }
        public string Name { get => name; set => name = value; }
        public FoodCategoryBo FoodCategory { get => foodCategory; set => foodCategory = value; }
        public FoodItemDiscountBo Discount { get => discount; set => discount = value; }
        public string UnitOfMeasurement { get => unitOfMeasurement; set => unitOfMeasurement = value; }
        public string Ingredients { get => ingredients; set => ingredients = value; }
        public float BasePrice
        {
            get => basePrice;
            set => basePrice = value < 0 ? 0 : value;
        }
        public string BasePriceString
        {
            get => String.Format("{0:0,0.00}", basePrice);
        }
        public float SellingPrice
        { 
            get => sellingPrice;
            set => sellingPrice = value < 0 ? 0 : value;
        }
        public string SellingPriceString
        {
            get => String.Format("{0:0,0.00}", sellingPrice);
        }
        // Refers to amount in cart, cannot be below 1
        public int Amount
        {
            get { return amount; }
            set
            {
                if (value < 1)
                    amount = 1;
                else
                    amount = value;
            }
        }
        public bool IsVisible { get => isVisible; set => isVisible = value; }
        #endregion


        // multiple discounts formula:
        // bp - base price, x, y - discounts
        // sellingPrice = basePrice * (1 - x * 1/100)(1 - y * 1/100)
        public void CalculatePrice()
        {
            sellingPrice = basePrice * (1 - this.Discount.DiscountAmount * (1 / 100))
                * (1 - this.FoodCategory.CategoryDiscount.DiscountAmount * (1 / 100));
            sellingPrice *= amount;
        }
    }
}
