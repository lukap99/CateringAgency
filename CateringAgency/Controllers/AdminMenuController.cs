using CateringAgency.Domain;
using CateringAgency.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CateringAgency.Controllers
{
    public class AdminMenuController : Controller
    {
        FoodRepository foodRepo = new FoodRepository();
        ComboMenuRepository comboRepo = new ComboMenuRepository();
        public ActionResult Index(int? id)
        {

            if (id.HasValue != true || id <= 0)
            {
                ViewBag.selectedCategoryId = 0;
                return View(foodRepo.GetAll());
            }
            else
            {
                int categoryId = (int)id;
                ViewBag.selectedCategoryId = categoryId;
                return View(foodRepo.GetFoodCategoryItems(categoryId));
            }
        }

        [ChildActionOnly]
        public ActionResult AdminMenuCategories(int selectedCategoryId)
        {
            TempData["selectedCategoryId"] = selectedCategoryId;
            return PartialView("_adminMenuCategories", foodRepo.GetAllFoodCategories());
        }

        public ActionResult CreateFood()
        {
            ViewBag.Categories = foodRepo.GetAllFoodCategories();
            return View();
        }

        [HttpPost]
        public ActionResult CreateFood(FoodBo food)
        {
            foodRepo.Create(food);
            return RedirectToAction("Index");
        }

        public ActionResult EditFood(int foodId)
        {
            FoodBo food = foodRepo.GetFoodItem(foodId);
            ViewBag.Categories = foodRepo.GetAllFoodCategories();
            return View(food);
        }

        [HttpPost]
        public ActionResult EditFood(FoodBo food)
        {
            foodRepo.Edit(food);
            return RedirectToAction("Index", new { id = food.FoodCategory.FoodCategoryId });
        }

        [HttpGet]
        public ActionResult DeleteFood(int foodId)
        {
            FoodBo food = foodRepo.GetFoodItem(foodId);
            return View(food);
        }

        [HttpPost, ActionName("DeleteFood")]
        public ActionResult DeleteFoodItem(int foodId)
        {
            foodRepo.Delete(foodId);
            return RedirectToAction("Index");
        }

        public ActionResult EditCategoryDiscount()
        {
            ViewBag.Categories = foodRepo.GetAllFoodCategories();
            return View();
        }

        [HttpPost]
        public ActionResult EditFoodCategoryDiscount(int foodCategoryId, int discountPercent)
        {
            try
            {
                foodRepo.EditCategoryDiscount(foodCategoryId, discountPercent);
                return RedirectToAction("Index", new { id = foodCategoryId });
            }
            catch (Exception)
            {
                return View("Index");
            }
        }


        //------------
        // COMBO MENU
        //------------

        public ActionResult CreateComboItem()
        {
            /*
            ComboMenuBo newComboItem = new ComboMenuBo();
            if (Session["newComboItem"] != null)
            {
                newComboItem = Session["newComboItem"] as ComboMenuBo;
                newComboItem.CalculatePrice();
            }

            ViewBag.food = foodRepo.GetAllActive();
            //ViewBag.newComboItem = newComboItem;*/
            ViewBag.Items = foodRepo.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult AddFoodToComboItem(int foodId, int amount)
        {
            try
            {
                FoodBo newFood = foodRepo.GetFoodItem(foodId);
                newFood.Amount = amount;

                ComboMenuBo newComboItem = new ComboMenuBo();
                if (Session["newComboItem"] != null)
                {
                    newComboItem = Session["newComboItem"] as ComboMenuBo;
                    newComboItem.AddItemToMenu(newFood);

                    ViewBag.food = foodRepo.GetAllActive();
                    return View("CreateComboItem", newComboItem);
                }
                else
                {
                    return RedirectToAction("CreateComboItem");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "AdminMenu");
            }
        }

        public ActionResult RemoveItemFromCombo(int foodId)
        {
            try
            {
                ComboMenuBo newComboItem = new ComboMenuBo();
                if (Session["newComboItem"] != null)
                {
                    newComboItem = Session["newComboItem"] as ComboMenuBo;
                    newComboItem.RemoveItemFromMenu(foodId);

                    ViewBag.food = foodRepo.GetAllActive();
                    return View("CreateComboItem", newComboItem);
                }
                else
                {
                    return RedirectToAction("CreateComboItem");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "AdminMenu");
            }
        }

        [HttpPost]
        public ActionResult CreateComboItem(ComboMenuBo comboMenu)
        {
            try
            {
                comboMenu.CalculatePrice(); //ACTUALLY SETS THE PRICE
                comboRepo.CreateComboMenu(comboMenu);
                //Session.Remove("newComboItem");
                return RedirectToAction("Index", "AdminMenu");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "AdminMenu");
            }
        }

        // ---------------
        // Edit combo menu
        // ---------------

        [HttpGet]
        public ActionResult EditComboMenu(int id)
        {
            ViewBag.foodItems = foodRepo.GetAllActive();
            ViewBag.comboFoodItems = comboRepo.GetComboMenuItems(id);
            try
            {
                return View(comboRepo.GetComboMenu(id));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "AdminMenu");
            }
        }

        [HttpPost]
        public ActionResult EditComboMenuDetails(ComboMenuBo comboMenu)
        {
            try
            {
                comboRepo.EditComboMenu(comboMenu.ComboMenuId, comboMenu.Name, comboMenu.BasePrice, comboMenu.DiscountAmount, comboMenu.IsVisible);
                return RedirectToAction("EditComboMenu", new { id = comboMenu.ComboMenuId });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "AdminMenu");
            }
        }

        [HttpPost]
        public ActionResult AddItemToComboMenu(int comboMenuId, int foodId, int amount)
        {
            try
            {
                FoodBo newFood = foodRepo.GetFoodItem(foodId);
                newFood.Amount = amount;

                comboRepo.AddComboMenuItem(foodBo: newFood, comboMenuId: comboMenuId);
                return RedirectToAction("EditComboMenu", new { id = comboMenuId });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "AdminMenu");
            }
        }

        [HttpPost]
        public ActionResult EditComboMenuItemAmount(int comboMenuId, int comboMenuItemId, int amount)
        {
            try
            {
                comboRepo.EditComboMenuItemAmount(comboMenuItemId, amount);
                return RedirectToAction("EditComboMenu", new { id = comboMenuId });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "AdminMenu");
            }
        }

        [HttpGet]
        public ActionResult DeleteComboMenuItem(int comboMenuId, int comboMenuItemId)
        {
            try
            {
                comboRepo.DeleteComboMenuItem(comboMenuItemId);
                return RedirectToAction("EditComboMenu", new { id = comboMenuId });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "AdminMenu");
            }
        }

        [HttpGet]
        public ActionResult DeleteComboMenu(int id)
        {
            try
            {
                comboRepo.DeleteComboMenu(id);
                return RedirectToAction("Index", "AdminMenu");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "AdminMenu");
            }
        }

    }
}