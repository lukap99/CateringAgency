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
        UserRepository userRepo = new UserRepository();
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
            try
            {
                CartBo cart = Session["Cart"] as CartBo;

                FoodBo foo = cart.CartItems.FirstOrDefault(t => t.FoodId == FoodId);
                int oldAmount = foo.Amount;

                if (Amount > 0)
                    ViewBag.changeAmountMessage = foo.Name + " ( " + oldAmount + " → " + Amount + " ) amount changed";
                else
                    ViewBag.errorMessage = "Cannot change food quantity to 0!";

                cart.UpdateItemAmount(FoodId, Amount);
                cart.CalculatePrice();
                cart.CalculatePoints();
                Session["Cart"] = cart;
                Session["CartItemsCount"] = cart.ItemCount;

                return View("Index", cart);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
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
                Session["CartItemsCount"] = cart.ItemCount;
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
        public ActionResult UpdateCartComboMenuItem(int ComboMenuId, int Amount)
        {
            try
            {
                CartBo cart = Session["Cart"] as CartBo;

                ComboMenuBo foo = cart.CartComboItems.FirstOrDefault(t => t.ComboMenuId == ComboMenuId);
                int oldAmount = foo.Amount;

                if (Amount > 0)
                    ViewBag.changeAmountMessage = foo.Name + " ( " + oldAmount + " → " + Amount + " ) amount changed";
                else
                    ViewBag.errorMessage = "Cannot change food quantity to 0!";

                cart.UpdateComboMenuAmount(ComboMenuId, Amount);
                cart.CalculatePrice();
                cart.CalculatePoints();
                Session["Cart"] = cart;
                Session["CartItemsCount"] = cart.ItemCount;

                return View("Index", cart);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

        }

        public ActionResult RemoveCartComboItem(int ComboMenuId)
        {
            if (Session["Cart"] != null)
            {
                CartBo cart = Session["Cart"] as CartBo;
                cart.RemoveComboMenuItemFromCart(ComboMenuId);
                cart.CalculatePoints();
                cart.CalculatePrice();
                Session["Cart"] = cart;
                Session["CartItemsCount"] = cart.ItemCount;
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
        [Authorize]
        public ActionResult CheckoutReview(CartBo crt)
        {
            CartBo cart = Session["Cart"] as CartBo;

            cart.DeliveryLocation = crt.DeliveryLocation;
            cart.DeliveryMethod = orderRepo.GetDeliveryMethod(crt.DeliveryMethod.DeliveryMethodId);
            cart.PaymentMethod = orderRepo.GetPaymentMethod(crt.PaymentMethod.PaymentMethodId);
            cart.OrderDiscount = orderRepo.GetOrderDiscount(crt.OrderDiscount.OrderDiscountId);

            cart.CalculatePrice();
            cart.CalculatePoints();

            Session["Cart"] = cart;
            Session["CartItemsCount"] = cart.ItemCount;

            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateOrder()
        {
            if (Session["Cart"] != null)
            {
                CartBo cart = Session["Cart"] as CartBo;
                orderRepo.CreateOrder(cart);

                UserBo userWithUpdatedPoints = cart.User;
                userWithUpdatedPoints.Points = userRepo.GetUserPoints((int)Session["UserId"]);

                Session["Cart"] = new CartBo(userWithUpdatedPoints); // updates old session value with fresh one
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