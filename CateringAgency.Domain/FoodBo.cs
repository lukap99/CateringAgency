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
        private int foodId;
        private string name;
        private FoodCategoryBo foodCategory;
        private FoodItemDiscountBo discount;
        private string unitOfMeasurement;
        private string ingredients;

        private float basePrice;
        private float sellingPrice;

        private int amount; // previous default = 1
        private bool isVisible = false;
        #endregion

        #region Properties
        public int FoodId { get => foodId; set => foodId = value; }
        public string Name { get => name; set => name = value; }
        public FoodCategoryBo FoodCategory { get => foodCategory; set => foodCategory = value; }
        public FoodItemDiscountBo Discount { get => discount; set => discount = value; }
        public string UnitOfMeasurement { get => unitOfMeasurement; set => unitOfMeasurement = value; }
        public string Ingredients { get => ingredients; set => ingredients = value; }
        public float BasePrice
        {
            get { return basePrice; }
            set
            {
                basePrice = value;
                sellingPrice = value;
            }
        }
        public float SellingPrice
        {
            // multiple discounts formula:
            // bp - base price, x, y - discounts
            // sellingPrice = basePrice * (1 - x * 1/100)(1 - y * 1/100)
            get
            {
                sellingPrice = basePrice * (1 - this.Discount.DiscountAmount * (1 / 100))
                    * (1 - this.FoodCategory.CategoryDiscount.DiscountAmount * (1 / 100));
                sellingPrice *= amount;
                return sellingPrice;
            }
            set
            { sellingPrice = value; }
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
    }
}
