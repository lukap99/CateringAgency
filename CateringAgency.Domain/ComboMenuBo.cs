using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringAgency.Domain
{
    public class ComboMenuBo
    {
        #region Fields
        private int comboMenuId;
        private string name;
        private List<FoodBo> comboMenuItems;

        private float basePrice;
        private float sellingPrice;

        private int amount;
        private float discountAmount;
        private bool isVisible;
        #endregion

        #region Constructors
        public ComboMenuBo()
        {
            comboMenuItems = new List<FoodBo>();
            basePrice = 0;
            sellingPrice = 0;

            amount = 1;
            discountAmount = 0;

            IsVisible = false;
        }

        public ComboMenuBo(int comboMenuId, string name, float discountAmount, bool isVisible, int amount)
        {
            this.comboMenuId = comboMenuId;
            this.name = name;
            DiscountAmount = discountAmount;
            this.isVisible = isVisible;
            this.amount = amount;
        }
        #endregion

        #region Properties
        public int ComboMenuId { get => comboMenuId; set => comboMenuId = value; }
        public string Name { get => name; set => name = value; }
        public List<FoodBo> ComboMenuItems { get => comboMenuItems; set => comboMenuItems = value; }
        public string ListOfItems
        {
            get
            {
                List<String> listOfItems = new List<string>();

                foreach (FoodBo item in comboMenuItems)
                {
                    listOfItems.Add(item.Name + " x " + item.Amount);
                }
                string concat = string.Join(", ", listOfItems);
                return concat;
            }
        }
        [Display(Name = "Base price")]
        public float BasePrice 
        { get => basePrice; set => basePrice = value; }
        public string BasePriceString
        {
            get => String.Format("{0:0.0,0}", basePrice);
        }
        public float SellingPrice 
        { get => sellingPrice; set => sellingPrice = value; }
        public string SellingPricePerUnitString
        {
            get => String.Format("{0:0.0,0}", (sellingPrice / amount));
        }
        public string SellingPriceString
        {
            get => String.Format("{0:0.0,0}", sellingPrice);
        }
        [Display(Name ="Discount")]
        public float DiscountAmount
        {
            get { return discountAmount; }
            set
            {
                if (value < 0)
                    discountAmount = 0;
                else if (value > 100)
                    discountAmount = 100;
                else
                    discountAmount = value;
            }
        }
        public bool DiscountIsValid
        {
            get
            {
                if (DiscountAmount > 0 && discountAmount < 100)
                    return true;
                else
                    return false;
            }
        }
        [Display(Name = "Is visible")]
        public bool IsVisible { get => isVisible; set => isVisible = value; }
        public int Amount { get => amount; set => amount = value; }
        #endregion

        // -------
        // Methods
        // -------
        #region Methods
        public void AddItemToMenu(FoodBo foodBo)
        {
            comboMenuItems.Add(foodBo);
        }
        public void RemoveItemFromMenu(int foodId)
        {
            comboMenuItems.RemoveAt(comboMenuItems.IndexOf(comboMenuItems.Single(t => t.FoodId == foodId)));
        }
        public void RemoveItemFromMenu(FoodBo food)
        {
            comboMenuItems.RemoveAt(comboMenuItems.IndexOf(comboMenuItems.Single(t => t.FoodId == food.FoodId)));
        }
        public void ClearItems()
        {
            comboMenuItems.Clear();
        }
        public bool HasItems()
        {
            if (comboMenuItems.Count() > 0)
                return true;
            else return false;
        }

        public void CalculatePrice()
        {
            SellingPrice = (basePrice * ((100 - this.discountAmount) * 0.01f)) * amount;
            //(sum * ((100 - orderDiscount.DiscountAmount) * 0.01f) + deliveryMethod.Price)
        }

        public void CalculateSuggestedPrice()
        {
            if (comboMenuItems.Count > 0)
            {
                float sum = 0;
                foreach (FoodBo item in comboMenuItems)
                {
                    item.CalculatePrice();
                    sum += item.SellingPrice;
                }
                basePrice = sum;
                SellingPrice = (sum * ((100 - this.discountAmount) * 0.01f)) * amount;
                //(sum * ((100 - orderDiscount.DiscountAmount) * 0.01f) + deliveryMethod.Price)
            }
        }

        public float SuggestedPrice()
        {

            float sum = 0;
            foreach (FoodBo item in comboMenuItems)
            {
                item.CalculatePrice();
                sum += item.SellingPrice;
            }
            float foo_basePrice = sum;
            float foo_SellingPrice = (sum * ((100 - this.discountAmount) * 0.01f)) * amount;

            return foo_SellingPrice;
        }
        #endregion
    }
}
