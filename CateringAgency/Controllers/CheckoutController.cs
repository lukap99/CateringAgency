using CateringAgency.Domain;
using CateringAgency.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CateringAgency.Controllers
{
    public class CheckoutController : Controller
    {
        OrderRepository orderRepo = new OrderRepository();

        public ActionResult Index()
        {
            if (Session["Cart"] == null)
            {
                CartBo newCart = new CartBo();
                Session["Cart"] = newCart;
                Session["CartItemsCount"] = 0;
                return View(newCart);
            }
            else
            {
                CartBo cart = Session["Cart"] as CartBo;
                Session["CartItemsCount"] = cart.ItemCount;
                return View(cart);
            }
        }
        public ActionResult EmptyCart()
        {
            if (Session["Cart"] != null)
            {
                CartBo cart = Session["Cart"] as CartBo;
                cart.ClearCart();
                Session["Cart"] = cart;
                Session["CartItemsCount"] = 0;
                return View("Index",cart);
            }
            else
            {
                CartBo newCart = new CartBo();
                Session["Cart"] = newCart;
                Session["CartItemsCount"] = 0;
                return View(newCart);
            }
        }

        [HttpPost]
        public ActionResult UpdateCartItem(int FoodId, int Amount)
        {
            if (Session["Cart"] != null)
            {
                CartBo cart = Session["Cart"] as CartBo;
                cart.UpdateItemAmount(FoodId, Amount);
                Session["Cart"] = cart;
                Session["CartItemsCount"] = cart.CartItems.Count;
                return View("Index", cart);
            }
            else
            {
                CartBo newCart = new CartBo();
                Session["Cart"] = newCart;
                Session["CartItemsCount"] = 0;
                return View(newCart);
            }
        }

        public ActionResult RemoveCartItem(int FoodId)
        {
            if (Session["Cart"] != null)
            {
                CartBo cart = Session["Cart"] as CartBo;
                cart.RemoveItemFromCart(FoodId);
                cart.CalculatePoints();
                cart.CalculatePrice();
                Session["Cart"] = cart;
                Session["CartItemsCount"] = cart.CartItems.Count;
                return View("Index", cart);
            }
            else
            {
                CartBo newCart = new CartBo();
                Session["Cart"] = newCart;
                Session["CartItemsCount"] = 0;
                return View(newCart);
            }
        }

        [HttpPost]
        public ActionResult CheckoutReview(CartBo crt)
        {
            CartBo cart = Session["Cart"] as CartBo;
            cart.PaymentMethod = orderRepo.GetPaymentMethod(crt.PaymentMethod.PaymentMethodId);
            cart.DeliveryLocation = crt.DeliveryLocation;
            cart.DeliveryMethod = orderRepo.GetDeliveryMethod(crt.DeliveryMethod.DeliveryMethodId);
            cart.OrderDiscount = orderRepo.GetOrderDiscount(crt.OrderDiscount.OrderDiscountId);
            cart.CalculatePrice();
            cart.CalculatePoints();
            Session["Cart"] = cart;
            Session["CartItemsCount"] = cart.CartItems.Count;
            return View(cart);
        }

        [HttpPost]
        public ActionResult CreateOrder()
        {
            if (Session["Cart"] != null)
            {
                CartBo cart = Session["Cart"] as CartBo;
                orderRepo.CreateOrder(cart);

                Session["Cart"] = new CartBo(cart.User); // updates old session value with fresh one
                Session["CartItemsCount"] = 0; // resets item count to 0
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult OrderDiscountOptions(int UserPoints)
        {
            ViewBag.points = UserPoints;
            ViewBag.discounts = orderRepo.GetDiscounts();
            return PartialView("_OrderDiscountOptions", Session["Cart"] as CartBo);
        }
    }
}