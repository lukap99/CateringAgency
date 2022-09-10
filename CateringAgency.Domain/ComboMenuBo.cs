using System;
using System.Collections.Generic;
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

        private float basePrice = 0;
        private float sellingPrice = 0;

        private int amount = 1;
        private float discountAmount = 0;
        private bool isVisible = false;
        #endregion

        #region Constructors
        public ComboMenuBo()
        {

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
        internal List<FoodBo> ComboMenuItems { get => comboMenuItems; set => comboMenuItems = value; }
        public float BasePrice 
        { get => basePrice; set => basePrice = value; }
        public float SellingPrice 
        { get => sellingPrice; set => sellingPrice = value; }
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
        public bool IsValid
        {
            get
            {
                if (DiscountAmount > 0 && discountAmount < 100)
                    return true;
                else
                    return false;
            }
        }

        public bool IsVisible { get => isVisible; set => isVisible = value; }
        public int Amount { get => amount; set => amount = value; }
        #endregion

        // Methods
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
            if (comboMenuItems.Count > 0)
            {
                float sum = 0;
                foreach (FoodBo item in comboMenuItems)
                {
                    item.CalculatePrice();
                    sum += item.SellingPrice;
                }
                basePrice = sum;
                SellingPrice = (basePrice * (1 - this.discountAmount * (1 / 100))) * amount;
            }
        }
        #endregion
    }
}
