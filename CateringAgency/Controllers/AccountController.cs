using CateringAgency.Domain;
using CateringAgency.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CateringAgency.Controllers
{
    public class AccountController : Controller
    {
        readonly UserRepository userRepository = new UserRepository();
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            ModelState.Clear();
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserBo user)
        {
            if (userRepository.IsValid(user))
            {
                UserBo userBo = userRepository.GetUser(user);
                FormsAuthentication.SetAuthCookie(userBo.Email, false);
                Session["UserId"] = userBo.UserId;
                Session["Firstname"] = userBo.FirstName;
                Session["Lastname"] = userBo.LastName;
                Session["Role"] = userBo.Role.RoleName;

                CartBo newCart = new CartBo(userBo);
                Session["Cart"] = newCart;
                Session["CartItemsCount"] = newCart.CartItems.Count;
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Netacan username ili password");
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserBo user)
        {
            if (!userRepository.Exists(user.Email))
            {
                UserBo newUser = new UserBo
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    Email = user.Email,
                    Password = user.Password,
                    Role = new RoleBo
                    {
                        RoleId = 1,
                        RoleName = "User"
                    },
                    Points = 0
                };
                userRepository.Create(newUser);
                FormsAuthentication.SetAuthCookie(newUser.Username, false);
                Session.Clear();
                Session["UserId"] = newUser.UserId;
                Session["Firstname"] = newUser.FirstName;
                Session["Lastname"] = newUser.LastName;
                Session["Role"] = newUser.Role.RoleName;
                Session["Cart"] = new CartBo(newUser);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Korisnik sa tom E-mail adresom već postoji");
                return View();
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Contents.RemoveAll();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult EditAccount()
        {
            int id = (int)Session["UserId"];
            return View(userRepository.GetUser(id));
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditAccount(UserBo user)
        {
            int id = (int)Session["UserId"];
            try
            {
                userRepository.Edit(user);

                return View(userRepository.GetUser(id));
            }
            catch (Exception)
            {
                return View(userRepository.GetUser(id));
            }
        }
    }
}