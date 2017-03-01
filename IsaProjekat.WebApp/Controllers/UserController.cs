using IsaProjekat.DataAcces.Model;
using IsaProjekat.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsaProjekat.WebApp.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            List<UserViewModel> users = null;
            try
            {
                using (isadbEntities context = new isadbEntities())
                {
                    users = context.Users.Select(x => new UserViewModel
                    {
                        Id = x.Id,
                        Name = x.FirstName,
                        Surname = x.LastName,
                        Mail = x.AspNetUser.Email
                    }).ToList();
                }
            }

            catch (Exception)
            {
                users = new List<UserViewModel>();
            }
            return View(users);
        }
    }
}

   

