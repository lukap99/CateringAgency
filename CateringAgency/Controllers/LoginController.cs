using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CateringAgency.Controllers
{
    public class LoginController : Controller
    {
        //readonly AuthRepository authRepository = new authRepository();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserBo userBo)
        {

        }
    }
}