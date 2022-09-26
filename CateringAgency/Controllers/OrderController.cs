using CateringAgency.Domain;
using CateringAgency.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CateringAgency.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        OrderRepository orderRepo = new OrderRepository();
        public ActionResult Index()
        {
            return RedirectToAction("MyOrders");
        }

        public ActionResult MyOrders()
        {
            if (User.Identity.IsAuthenticated)
            {
                int id = Convert.ToInt32(Session["UserId"]);

                return View(orderRepo.GetUserOrders(id));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //User view order
        public ActionResult ViewOrder(int orderId)
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(orderRepo.GetOrder(orderId));
            }
            else
            {
                return RedirectToAction("Index", "Home");
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
    }
}