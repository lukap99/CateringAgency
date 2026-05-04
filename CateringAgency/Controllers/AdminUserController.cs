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

        // GET: AdminUser/Users
        public ActionResult Users(string sortOrder, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentFilter = searchString;
            int pageNumber = page ?? 1;
            int pageSize = 10;

            var users = userRepo.GetAllUsers(); // returns List<UserBo> or IEnumerable

            // Filter: search by name, email, username
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u =>
                    u.FirstName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    u.LastName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    u.Email.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    u.Username.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                );
            }

            // Sorting
            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(u => u.LastName).ThenByDescending(u => u.FirstName);
                    break;
                case "email_asc":
                    users = users.OrderBy(u => u.Email);
                    break;
                case "email_desc":
                    users = users.OrderByDescending(u => u.Email);
                    break;
                case "points_asc":
                    users = users.OrderBy(u => u.Points);
                    break;
                case "points_desc":
                    users = users.OrderByDescending(u => u.Points);
                    break;
                default: // name_asc
                    users = users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName);
                    break;
            }

            int totalItems = users.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var pagedUsers = users.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;
            ViewBag.PageSize = pageSize;

            return View(pagedUsers);
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

        public ActionResult Managers(string sortOrder, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentFilter = searchString;
            int pageNumber = page ?? 1;
            int pageSize = 10;

            var managers = userRepo.GetAllManagers();

            if (!string.IsNullOrEmpty(searchString))
            {
                managers = managers.Where(m =>
                    m.FirstName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    m.LastName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    m.Email.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    m.Username.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                );
            }

            switch (sortOrder)
            {
                case "name_desc":
                    managers = managers.OrderByDescending(m => m.LastName).ThenByDescending(m => m.FirstName);
                    break;
                case "email_asc":
                    managers = managers.OrderBy(m => m.Email);
                    break;
                case "email_desc":
                    managers = managers.OrderByDescending(m => m.Email);
                    break;
                default:
                    managers = managers.OrderBy(m => m.LastName).ThenBy(m => m.FirstName);
                    break;
            }

            int totalItems = managers.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var paged = managers.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;
            return View(paged);
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