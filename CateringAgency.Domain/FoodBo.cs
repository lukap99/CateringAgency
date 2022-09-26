using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private float sellingPricePerUnit;
        private float sellingPrice;

        private int amount; // previous default = 1
        private bool isVisible;

        private string imagePath = "/Images/no_image.png";
        #endregion

        public FoodBo()
        {
            amount = 1;
            basePrice = 0;
            sellingPrice = 0;
            isVisible = false;
        }

        #region Properties
        public int FoodId { get => foodId; set => foodId = value; }

        [Required]
        public string Name { get => name; set => name = value; }
        [Display(Name = " Food category")]
        public FoodCategoryBo FoodCategory { get => foodCategory; set => foodCategory = value; }
        public FoodItemDiscountBo Discount { get => discount; set => discount = value; }

        [Display(Name = " Unit of measurement")]
        public string UnitOfMeasurement { get => unitOfMeasurement; set => unitOfMeasurement = value; }
        public string Ingredients { get => ingredients; set => ingredients = value; }

        [Range(1, float.MaxValue)]
        [Display(Name = " Base price")]
        [DataType(DataType.Currency)]
        public float BasePrice
        {
            get => basePrice;
            set => basePrice = value < 0 ? 0 : value;
        }
        public string BasePriceString
        {
            get => String.Format("{0:0.0,0}", basePrice);
        }

        [Display(Name = " Selling price")]
        public float SellingPrice
        {
            get => sellingPrice;
            set => sellingPrice = value < 0 ? 0 : value;
        }
        public float SellingPricePerUnit
        {
            get => (sellingPrice / amount);
        }
        public string SellingPricePerUnitString
        {
            get => String.Format("{0:0.0,0}", (sellingPrice / amount));
        }
        public string SellingPriceString
        {
            get => String.Format("{0:0.0,0}", sellingPrice);
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

        [Display(Name = " Is visible")]
        public bool IsVisible { get => isVisible; set => isVisible = value; }

        [Display(Name = " Image path")]
        public string ImagePath { get => imagePath; set => imagePath = value; }

        #endregion


        // multiple discounts formula:
        // bp - base price, x, y - discounts
        // sellingPrice = basePrice * (1 - x * 1/100)(1 - y * 1/100)
        public void CalculatePrice()
        {
            sellingPricePerUnit = (
                basePrice * ((100 - this.Discount.DiscountAmount) * 0.01f)
                * ((100 - this.FoodCategory.CategoryDiscount.DiscountAmount) * 0.01f)
                );
            sellingPrice = sellingPricePerUnit * amount;
        }
    }
}
