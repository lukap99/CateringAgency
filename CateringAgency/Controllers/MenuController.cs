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

        public ActionResult Index(int? id)
        {
            if (id.HasValue != true || id <= 0)
            {
                ViewBag.selectedCategoryId = 0;
                return View(foodRepo.GetAllActive());
            }
            else
            {
                int categoryId = (int)id;
                ViewBag.selectedCategoryId = categoryId;
                return View(foodRepo.GetFoodCategoryItemsActive(categoryId));
            }
        }

        [ChildActionOnly]
        public ActionResult MenuCategories(int selectedCategoryId)
        {
            TempData["selectedCategoryId"] = selectedCategoryId;
            return PartialView("_menuCategories", foodRepo.GetAllFoodCategories());
        }

        public ActionResult FoodDetails(int id)
        {
            return View();
        }

        public ActionResult ComboMenu()
        {
            return View();
        }

        public ActionResult AddFoodToCart(int id)
        {
            throw new NotImplementedException();
        }
    }
}