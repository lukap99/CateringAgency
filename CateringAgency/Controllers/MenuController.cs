using CateringAgency.Domain;
using CateringAgency.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CateringAgency.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        FoodRepository foodRepo = new FoodRepository();
        ComboMenuRepository comboRepo = new ComboMenuRepository();

        public ActionResult Index(int? id)
        {
            if (TempData.ContainsKey("addMessage"))
                ViewBag.addMessage = TempData["addMessage"].ToString();

            if (TempData.ContainsKey("errorMessage"))
                ViewBag.errorMessage = TempData["errorMessage"].ToString();

            int selectedCategoryId = 0; // Controls which category is highlighted

            if (id.HasValue != true || id <= 0)
                selectedCategoryId = 0;
            else
                selectedCategoryId = (int)id;


            if (selectedCategoryId <= 0)
            {
                ViewBag.selectedCategoryId = 0;
                // Put combo items in view bag
                // Display ViewBag in view
                ViewBag.comboItems = comboRepo.GetAllActiveComboMenus();
                return View(foodRepo.GetAllActive());
            }
            else if (id < 10)
            {
                ViewBag.selectedCategoryId = selectedCategoryId;
                return View(foodRepo.GetFoodCategoryItemsActive(selectedCategoryId));
            }
            else
            {
                ViewBag.selectedCategoryId = 10; // 10 to select combo menu id
                ViewBag.comboItems = comboRepo.GetAllActiveComboMenus();
                return View(new List<FoodBo>());
            }
        }

        [ChildActionOnly]
        public ActionResult MenuCategories(int selectedCategoryId)
        {
            ViewBag.selectedCategoryId = selectedCategoryId;
            // Passes the selected foodCategoryId to AddToCart and AddComboMenuToCart ActionResults
            // So Index keeps showing the selected food category
            TempData["selectedCategoryId"] = selectedCategoryId;

            return PartialView("_menuCategories", foodRepo.GetAllFoodCategories());
        }

        public ActionResult FoodDetails(int id)
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddToCart(int FoodId, int Amount)
        {
            if (Amount > 0)
            {
                FoodBo newFood = foodRepo.GetFoodItem(FoodId);
                newFood.Amount = Amount;
                newFood.CalculatePrice();

                TempData["addMessage"] = 
                    newFood.Name.ToString() + " ( × " + newFood.Amount.ToString() + " ) added to cart";

                // if TempData has "selectedCategoryId", assign its value to selectedCategoryId
                // otherwise assign 0
                int selectedCategoryId =
                    TempData.ContainsKey("selectedCategoryId") ? (int)TempData["selectedCategoryId"] : 0;

                if (Session["Cart"] != null)
                {
                    CartBo cart = Session["Cart"] as CartBo; // "as" returns NULL if typecast isn't succesful
                    cart.AddToCart(newFood);

                    Session["Cart"] = cart;
                    Session["CartItemsCount"] = cart.ItemCount;

                    return RedirectToAction("Index", new { id = selectedCategoryId });
                }
                else // If cart doesn't exist in session
                {
                    CartBo cart = new CartBo();
                    cart.AddToCart(newFood);

                    Session["Cart"] = cart;
                    Session["CartItemsCount"] = cart.ItemCount;

                    return RedirectToAction("Index", new { id = selectedCategoryId });
                }
            }
            else
            {
                TempData["errorMessage"] = "Amount added to cart cannot be 0";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult AddComboMenuToCart(int ComboMenuId, int Amount)
        {
            if (Amount > 0)
            {
                ComboMenuBo newComboItem = comboRepo.GetComboMenu(ComboMenuId);
                newComboItem.Amount = Amount;
                newComboItem.CalculatePrice();

                TempData["addMessage"] = "Combo menu: " + newComboItem.Name.ToString() + " × " + newComboItem.Amount.ToString() + " added to cart";

                // if TempData has "selectedCategoryId", assign its value to selectedCategoryId
                // otherwise assign 0
                int selectedCategoryId =
                    TempData.ContainsKey("selectedCategoryId") ? (int)TempData["selectedCategoryId"] : 0;

                if (Session["Cart"] != null)
                {
                    CartBo cart = Session["Cart"] as CartBo; // "as" returns NULL if typecast isn't succesful
                    cart.AddToCart(newComboItem);

                    Session["Cart"] = cart;
                    Session["CartItemsCount"] = cart.ItemCount;

                    return RedirectToAction("Index", new { id = selectedCategoryId });
                }
                else // If cart doesn't exist in session
                {
                    CartBo cart = new CartBo();
                    cart.AddToCart(newComboItem);

                    Session["Cart"] = cart;
                    Session["CartItemsCount"] = cart.ItemCount;

                    return RedirectToAction("Index", new { id = selectedCategoryId });
                }

            }
            else
            {
                TempData["errorMessage"] = "Amount added to cart cannot be 0";
                return RedirectToAction("Index");
            }
        }

    }
}