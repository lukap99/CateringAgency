using CateringAgency.Domain;
using CateringAgency.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CateringAgency.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        UserRepository userRepo = new UserRepository();
            
        public ActionResult Index()
        {
            return RedirectToAction("Users");
        }

        public ActionResult Users()
        {
            return View(userRepo.GetAllUsers());
        }

        public ActionResult EditUser(int id)
        {
            try
            {
                return View(userRepo.GetUser(id));
            }
            catch (Exception)
            {
                return RedirectToAction("Users");
            }
        }

        [HttpPost]
        public ActionResult EditUser(UserBo user)
        {
            try
            {
                user.Role.RoleId = 1;
                userRepo.Edit(user);
                return RedirectToAction("EditUser", new { id = user.UserId });
            }
            catch (Exception)
            {
                return RedirectToAction("Users");
            }
        }

        public ActionResult DeleteUser(int id)
        {
            try
            {
            return View(userRepo.GetUser(id));
            }
            catch (Exception)
            {
                return RedirectToAction("Users");
            }
        }

        public ActionResult ConfirmDeleteUser(int id)
        {
            userRepo.Delete(id);
            return RedirectToAction("Users");
        }

        // --------
        // MANAGERS
        // --------

        public ActionResult Managers()
        {
            return View(userRepo.GetAllManagers());
        }

        public ActionResult CreateManager()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateManager(UserBo user)
        {
            if (!userRepo.Exists(user))
            {
                try
                {
                    userRepo.CreateManager(user);
                    return RedirectToAction("Managers");
                }
                catch (Exception)
                {
                    return RedirectToAction("Managers");
                }
            }
            else
            {
                ModelState.AddModelError("", "Menadžer sa tom E-mail adresom već postoji");
                return View();
            }
        }

        public ActionResult EditManager(int id)
        {
            try
            {
                return View(userRepo.GetUser(id));
            }
            catch (Exception)
            {
                return RedirectToAction("Managers");
            }
        }

        [HttpPost]
        public ActionResult EditManager(UserBo user)
        {
            try
            {
                user.Role.RoleId = 2;
                userRepo.Edit(user);
                return RedirectToAction("EditManager", new { id = user.UserId });
            }
            catch (Exception)
            {
                return RedirectToAction("Managers");
            }

        }

        public ActionResult DeleteManager(int id)
        {
            try
            {
                return View(userRepo.GetUser(id));
            }
            catch (Exception)
            {
                return RedirectToAction("Managers");
            }
        }

        public ActionResult ConfirmDeleteManager(int id)
        {
            userRepo.Delete(id);
            return RedirectToAction("Managers");
        }
    }
}