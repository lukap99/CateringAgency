using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringAgency.Domain
{
    class ComboMenuBo
    {
        #region Fields
        private int comboMenuId;
        private string name;
        private List<FoodBo> comboMenuItems;
        private float basePrice = 0;
        private float sellingPrice = 0;
        private DateTime validFrom;
        private DateTime validUntil;
        private float discountAmount = 0;
        #endregion

        #region Constructors
        public ComboMenuBo()
        {

        }

        public ComboMenuBo(int comboMenuId, string name, DateTime validFrom, DateTime validUntil, float discountAmount)
        {
            this.comboMenuId = comboMenuId;
            this.name = name;
            this.validFrom = validFrom;
            this.validUntil = validUntil;
            DiscountAmount = discountAmount;
        }
        #endregion

        #region Properties
        public int ComboMenuId { get => comboMenuId; set => comboMenuId = value; }
        public string Name { get => name; set => name = value; }
        internal List<FoodBo> ComboMenuItems { get => comboMenuItems; set => comboMenuItems = value; }
        public float BasePrice 
        {
            get
            {
                this.CalculatePrice();
                return basePrice;
            }
            set => basePrice = value; 
        }
        public float SellingPrice 
        {
            get
            {
                this.CalculatePrice();
                return sellingPrice;
            }
            set => sellingPrice = value;
        }
        public DateTime ValidFrom { get => validFrom; set => validFrom = value; }
        public DateTime ValidUntil { get => validUntil; set => validUntil = value; }
        public float DiscountAmount
        {
            get { return discountAmount; }
            set
            {
                if (value < 0)
                {
                    discountAmount = 0;
                }
                else if (value > 100)
                {
                    discountAmount = 100;
                }
                else
                {
                    discountAmount = value;
                }
            }
        }
        public bool IsValid
        {
            get
            {
                if (DateTime.Now > ValidFrom && DateTime.Now < ValidUntil && DiscountAmount > 0)
                {
                    return true;
                }
                else return false;
            }
        }
        #endregion

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
                    sum += item.SellingPrice;
                }
                basePrice = sum;
                SellingPrice = basePrice * (1 - this.discountAmount * (1 / 100));
            }
        }
        #endregion
    }
}
