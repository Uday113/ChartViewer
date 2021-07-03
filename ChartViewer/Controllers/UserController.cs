using ChartViewer.Helpers;
using ChartViewer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using DataAccessLayer;
using UserContract;
using UserModule;

namespace ChartViewer.Controllers
{
    public class UserController : Controller
    {
        readonly IUserModule userservice;

        public UserController(IUserModule userservice)
        {
            this.userservice = userservice;
        }

        // GET: User
        [AllowAnonymous]
        public ActionResult Login()
        {
            if(TempData["Error"] != null)
            {
                ViewBag.Error = Convert.ToString(TempData["Error"]);
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            CryptographicHelper crypto = new CryptographicHelper();
            
            var result = userservice.ValidateUser(model.Email);

            switch (result)
            {
                case "ERR_MultiLoginFailure":
                    ViewBag.Error = "You Account is blocked for multiple failed login attempts. Please try again tomorrow.";
                    return View(model);
                case "ERR_NoUserFound":
                    ViewBag.Error = "No User found with this username.";
                    return View(model);
                default:
                    var decryptedPassword = crypto.DecryptData(result);
                    if (decryptedPassword.Equals(model.Password))
                    {
                        //login success
                        Session["userId"] = model.Email;
                        return RedirectToAction("ViewChart", "Home");
                    }
                    else
                    {
                        //login failed
                        //update failure count in db
                        userservice.UpdateLoginFailure(model.Email);
                        ViewBag.Error = "Invalid username or password.";
                        return View(model);
                    }
                    
            }
            
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "User");
        }
    }
}