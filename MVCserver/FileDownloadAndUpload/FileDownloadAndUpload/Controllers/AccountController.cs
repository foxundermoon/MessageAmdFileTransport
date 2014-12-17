using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FileDownloadAndUpload.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {

            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Models.User user)
        {
            return View(user);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Models.User user)
        {
            Models.MessageEntities1 entity = Core.Common.MessageEntityFictory.ModelsEntities;
            if (entity.User.Where(u => u.Name == user.Name && u.Password == user.Password).Count() > 0)
            {
                FormsAuthentication.SetAuthCookie(user.Name, true);
                return RedirectToAction("index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "账号或者密码不正确");
                return View(user);
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}
