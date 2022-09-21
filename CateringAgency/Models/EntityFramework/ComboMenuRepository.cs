using CateringAgency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CateringAgency.Models.EntityFramework
{
    public class ComboMenuRepository
    {
        //TODO
        // GetAllComboMenu
        // GetAllActiveComboMenu
        // GetComboMenu(id)
        // Create
        // Edit
        // Delete
        private readonly CateringAgencyEntities cateringEntities;
        private readonly FoodRepository foodRepo = new FoodRepository();

        public ComboMenuRepository()
        {
            cateringEntities = new CateringAgencyEntities();
        }

        public ComboMenuBo ComboMenuMap(combo_menu comboModel)
        {
            ComboMenuBo comboBo = new ComboMenuBo
            {
                ComboMenuId = comboModel.id,
                Name = comboModel.name,
                DiscountAmount = (float)comboModel.discount_percent,
                IsVisible = comboModel.is_visible,
                BasePrice = (float)comboModel.price
            };

            // CASCADE DELETE setting on "food" table ensures that ComboMenuItems get deleted
            // if the parent food item is deleted
            foreach (combo_menu_item comboMenuItem in comboModel.combo_menu_item)
            {
                FoodBo foo_food = foodRepo.FoodMap(comboMenuItem.food);
                foo_food.FoodId = comboMenuItem.id; // sets FOOD_ID to value of COMBO_MENU_ITEM_ID !! Used for edits and deletion
                foo_food.Amount = comboMenuItem.amount;
                foo_food.FoodCategory.CategoryDiscount.DiscountAmount = 0; // items in combo menu do not get discounts applied to them
                foo_food.Discount.DiscountAmount = 0;
                foo_food.IsVisible = true;
                foo_food.CalculatePrice();

                comboBo.AddItemToMenu(foo_food);
            }
            comboBo.CalculatePrice();
            return comboBo;
        }

        public IEnumerable<ComboMenuBo> GetAllComboMenus()
        {
            List<ComboMenuBo> comboMenuList = new List<ComboMenuBo>();
            foreach (combo_menu comboMenuItem in cateringEntities.combo_menu)
            {
                comboMenuList.Add(ComboMenuMap(comboMenuItem));
            }
            return comboMenuList;
        }
        public IEnumerable<ComboMenuBo> GetAllActiveComboMenus()
        {
            List<ComboMenuBo> comboMenuList = new List<ComboMenuBo>();
            foreach (combo_menu comboMenuItem in cateringEntities.combo_menu.Where(t => t.is_visible == true))
            {
                comboMenuList.Add(ComboMenuMap(comboMenuItem));
            }
            return comboMenuList;
        }

        public ComboMenuBo GetComboMenu(int comboMenuId)
        {
            if (cateringEntities.combo_menu.Any(t=>t.id == comboMenuId))
            {
                combo_menu comboMenuModel = cateringEntities.combo_menu.FirstOrDefault(t => t.id == comboMenuId);

                ComboMenuBo foo_combo = ComboMenuMap(comboMenuModel);

                return foo_combo;
            }
            else
            {
                return new ComboMenuBo();
            }
        }

        public void CreateComboMenu(ComboMenuBo comboMenuBo)
        {
            combo_menu comboMenuModel = new combo_menu
            {
                name = comboMenuBo.Name,
                discount_percent = comboMenuBo.DiscountAmount,
                is_visible = comboMenuBo.IsVisible,
                price = comboMenuBo.BasePrice
            };

            try
            {
                cateringEntities.combo_menu.Add(comboMenuModel);
                cateringEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ComboMenuRepository.CreateComboMenu(ComboMenuBo comboMenuBo): " + ex.Message);
            }
        }
        public void CreateComboMenu(String name, float price, float discountAmount, bool isVisible)
        {
            combo_menu comboMenuModel = new combo_menu
            {
                name = name,
                discount_percent = discountAmount,
                is_visible = isVisible,
                price = price
            };

            try
            {
                cateringEntities.combo_menu.Add(comboMenuModel);
                cateringEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ComboMenuRepository.CreateComboMenu(ComboMenuBo comboMenuBo): " + ex.Message);
            }
        }

        public void EditComboMenu(ComboMenuBo comboMenuBo)
        {
            try
            {
                combo_menu comboMenuModel = cateringEntities.combo_menu.FirstOrDefault(t => t.id == comboMenuBo.ComboMenuId);

                comboMenuModel.name = comboMenuBo.Name;
                comboMenuModel.discount_percent = comboMenuBo.DiscountAmount;
                comboMenuModel.is_visible = comboMenuBo.IsVisible;
                comboMenuModel.price = comboMenuBo.BasePrice;

                cateringEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ComboMenuRepository.EditComboMenu(ComboMenuBo comboMenuBo): " + ex.Message);
            }
        }
        public void EditComboMenu(int comboMenuId, String name, float price, float discountAmount, bool isVisible)
        {
            try
            {
                combo_menu comboMenuModel = cateringEntities.combo_menu.FirstOrDefault(t => t.id == comboMenuId);

                comboMenuModel.name = name;
                comboMenuModel.discount_percent = discountAmount;
                comboMenuModel.is_visible = isVisible;
                comboMenuModel.price = price;

                cateringEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ComboMenuRepository.EditComboMenu(ComboMenuBo comboMenuBo): " + ex.Message);
            }
        }

        public void DeleteComboMenu(ComboMenuBo comboMenuBo)
        {
            try
            {
                combo_menu comboMenuModel = cateringEntities.combo_menu.FirstOrDefault(t => t.id == comboMenuBo.ComboMenuId);

                cateringEntities.combo_menu.Remove(comboMenuModel);
                cateringEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ComboMenuRepository.DeleteComboMenu(ComboMenuBo comboMenuBo): " + ex.Message);
            }
        }
        public void DeleteComboMenu(int comboMenuId)
        {
            try
            {
                combo_menu comboMenuModel = cateringEntities.combo_menu.FirstOrDefault(t => t.id == comboMenuId);

                cateringEntities.combo_menu.Remove(comboMenuModel);
                cateringEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ComboMenuRepository.DeleteComboMenu(ComboMenuBo comboMenuBo): " + ex.Message);
            }
        }

        public void AddComboMenuItem(FoodBo foodBo, int comboMenuId)
        {
            try
            {
                combo_menu_item comboMenuItemModel = new combo_menu_item
                {
                    food_id = foodBo.FoodId,
                    combo_menu_id = comboMenuId,
                    amount = foodBo.Amount
                };

                cateringEntities.combo_menu_item.Add(comboMenuItemModel);
                cateringEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ComboMenuRepository.AddComboMenuItem(FoodBo foodBo, int comboMenuId): " + ex.Message);
            }
        }

        public void DeleteComboMenuItem(int comboItemId)
        {
            try
            {
                combo_menu_item comboMenuItemModel = cateringEntities.combo_menu_item.FirstOrDefault(t => t.id == comboItemId);
                
                cateringEntities.combo_menu_item.Remove(comboMenuItemModel);
                cateringEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ComboMenuRepository.DeleteComboMenuItem(int comboItemId): " + ex.Message);
            }
        }

        public void EditComboMenuItemAmount(int comboItemId, int amount)
        {
            try
            {
                combo_menu_item comboMenuItemModel = cateringEntities.combo_menu_item.FirstOrDefault(t => t.id == comboItemId);
                comboMenuItemModel.amount = amount;

                cateringEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ComboMenuRepository.EditComboMenuItemAmount(int comboItemId, int amount): " + ex.Message);
            }
        }

        public IEnumerable<FoodBo> GetComboMenuItems(int comboMenuId)
        {
            List<FoodBo> comboItemsList = new List<FoodBo>();

            foreach (combo_menu_item comboMenuItem in cateringEntities.combo_menu_item.Where(t => t.combo_menu_id == comboMenuId))
            {
                FoodBo foo_food = foodRepo.FoodMap(comboMenuItem.food);
                foo_food.Amount = comboMenuItem.amount;
                foo_food.FoodId = comboMenuItem.id; // sets FOOD_ID to value of COMBO_MENU_ITEM_ID !! So you can delete
                foo_food.IsVisible = true;
                comboItemsList.Add(foo_food);
            }

            return comboItemsList;
        }

        public FoodBo GetComboMenuItem(int comboMenuItemId)
        {
            combo_menu_item foo = cateringEntities.combo_menu_item.FirstOrDefault(t => t.id == comboMenuItemId);

            FoodBo foodBo = foodRepo.FoodMap(foo.food);
            foodBo.Amount = foo.amount;
            foodBo.FoodId = foo.id;
            foodBo.IsVisible = true;

            return foodBo;
        }

    }
}
