using IsaProjekat.DataAcces.Model;
using IsaProjekat.WebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IsaProjekat.WebApp.Controllers
{
    public class ManagerController : Controller
    {

        // GET: Manager
        public ActionResult Index()
        {
            List<ManagerModel> managers = null;
            try
            {
                using (isadbEntities context = new isadbEntities())
                {
                    managers = context.Users.Where(x => x.AspNetUser.AspNetRoles.Any(z => z.Name.Equals("manager"))).Select(x => new ManagerModel
                    {
                        Id = x.Id,
                        Email = x.AspNetUser.Email,
                        FirstName = x.FirstName,
                        LastName = x.LastName
                    }).ToList();
                }
            }
            catch (Exception)
            {

                managers = new List<ManagerModel>();
            }
            return View(managers);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Register()
        {
            return View();
        }

       

      
    }
}