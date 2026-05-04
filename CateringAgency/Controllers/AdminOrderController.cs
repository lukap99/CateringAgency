using CateringAgency.Domain;
using CateringAgency.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CateringAgency.Controllers
{
    [Authorize(Roles = "Manager, Admin")]
    public class AdminOrderController : Controller
    {
        OrderRepository orderRepo = new OrderRepository();

        [Authorize(Roles = "Manager, Admin")]
        public ActionResult Index(string sortOrder, string searchString, int? id, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentFilter = searchString;
            int pageNumber = page ?? 1;
            int pageSize = 10;

            // Get orders (either all or filtered by user)
            IEnumerable<OrderBo> orders;

            if (id.HasValue == false)
                orders = orderRepo.GetAllOrders();
            else
                orders = orderRepo.GetUserOrders(id.Value);

            // Apply search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o =>
                    o.User.FirstName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    o.User.LastName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    o.User.Email.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    o.OrderId.ToString().IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                );
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "orderId_desc":
                    orders = orders.OrderByDescending(o => o.OrderId);
                    break;
                case "date_asc":
                    orders = orders.OrderBy(o => o.DateCreated);
                    break;
                case "date_desc":
                    orders = orders.OrderByDescending(o => o.DateCreated);
                    break;
                case "total_asc":
                    orders = orders.OrderBy(o => o.BuyingPrice);
                    break;
                case "total_desc":
                    orders = orders.OrderByDescending(o => o.BuyingPrice);
                    break;
                case "customer_asc":
                    orders = orders.OrderBy(o => o.User.LastName).ThenBy(o => o.User.FirstName);
                    break;
                case "customer_desc":
                    orders = orders.OrderByDescending(o => o.User.LastName).ThenByDescending(o => o.User.FirstName);
                    break;
                default:
                    orders = orders.OrderBy(o => o.OrderId);
                    break;
            }

            // Count total items before pagination
            int totalItems = orders.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            ViewBag.TotalItems = totalItems;

            // Apply pagination
            var pagedOrders = orders.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Pass pagination data to the view
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return View(pagedOrders);
        }

        public ActionResult ViewOrder(int id)
        {
            try
            {
                return View(orderRepo.GetOrder(id));
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
            
        }
        public ActionResult CancelOrder(int id)
        {
            bool isComplete = orderRepo.IsOrderComplete(id);
            if (User.Identity.IsAuthenticated && isComplete == false)
            {
                orderRepo.CancelOrder(id);
                return RedirectToAction("ViewOrder", new { id = id });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult CompleteOrder(int id)
        {
            try
            {
                orderRepo.CompleteOrder(id);
                return RedirectToAction("ViewOrder", new { id = id });
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }
    }
}