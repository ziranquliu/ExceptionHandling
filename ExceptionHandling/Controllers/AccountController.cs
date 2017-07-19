using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExceptionHandling.Models;

namespace ExceptionHandling.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController()
            : base("myPolicy")
        { }

        public ActionResult SignIn()
        {
            return View(new LoginInfo());
        }
        [HttpPost]
        public ActionResult SignIn(LoginInfo loginInfo)
        {
            if (!ModelState.IsValid)
            {
                return this.View(new LoginInfo { UserName = loginInfo.UserName });
            }

            if (loginInfo.UserName != "Foo")
            {
                throw new InvalidUserNameException();
            }

            if (loginInfo.Password != "password")
            {
                throw new UserNamePasswordNotMatchException();
            }

            ViewBag.Message = "Authentication Succeeds!";
            return this.View(new LoginInfo { UserName = loginInfo.UserName });
        }

        public ActionResult OnSignInError(string userName)
        {
            return this.View(new LoginInfo { UserName = userName });
        }
    }
}
