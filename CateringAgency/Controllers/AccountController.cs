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
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            ModelState.Clear();
            return View();
        }
        
        [HttpPost, ActionName("LoginUser")]
        public ActionResult Login(UserBo user)
        {
            if (userRepository.IsValid(user))
            {
                UserBo userBo = userRepository.GetUser(user);
                FormsAuthentication.SetAuthCookie(user.Username, false);
                Session["Firstname"] = userBo.FirstName;
                Session["Lastname"] = userBo.LastName;
                Session["Role"] = userBo.Role.RoleName;
                Session["Cart"] = new CartBo(userBo);
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
            if (userRepository.IsValid(user) != true && user.Password == user.PasswordConfirm)
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
                Session["Firstname"] = newUser.FirstName;
                Session["Lastname"] = newUser.LastName;
                Session["Role"] = newUser.Role.RoleName;
                Session["Cart"] = new CartBo(newUser);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Korisnik sa tom E-mail adresom već pstoji");
                return View();
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}