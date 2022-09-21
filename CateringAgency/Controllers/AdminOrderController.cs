using CateringAgency.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CateringAgency.Controllers
{
    public class AdminOrderController : Controller
    {
        OrderRepository orderRepo = new OrderRepository();
        public ActionResult Index(int? id)
        {
            if (id.HasValue == false)
            {
                return View(orderRepo.GetAllOrders());
            }
            else
            {
                return View(orderRepo.GetUserOrders(id.Value));
            }
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
        public ActionResult CancelOrder(int orderId)
        {
            bool isComplete = orderRepo.IsOrderComplete(orderId);
            if (User.Identity.IsAuthenticated && isComplete == false)
            {
                orderRepo.CancelOrder(orderId);
                return RedirectToAction("MyOrders");
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