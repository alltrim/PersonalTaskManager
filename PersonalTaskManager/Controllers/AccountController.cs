using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PersonalTaskManager.Models;
using PersonalTaskManager.DataModels;

namespace PersonalTaskManager.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel login)
        {
            bool userIsOK = false;

            if (ModelState.IsValid)
            {
                using (TasksContext context = new TasksContext())
                {
                    var query = from users in context.Users
                                where users.UserName == login.UserName
                                select users;

                    User user = query.FirstOrDefault();
                    if (user != null)
                    {
                        if (LoginModel.CalcHash(login.Password) == user.Password)
                        {
                            userIsOK = true;
                        }
                    }
                    else
                    {
                        if (login.Password != null && login.UserName != null && login.Password != string.Empty && login.UserName != string.Empty)
                        {
                            User newUser = new DataModels.User()
                            {
                                UserName = login.UserName,
                                Password = LoginModel.CalcHash(login.Password)
                            };

                            context.Users.Add(newUser);
                            context.SaveChanges();

                            userIsOK = true;
                        }
                    }
                }
            }

            if (userIsOK)
            {
                FormsAuthentication.SetAuthCookie(login.UserName, false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                login.Message = "Invalid User name or Password";
                login.Password = "";
                return View(login);
            }
        }
    }
}