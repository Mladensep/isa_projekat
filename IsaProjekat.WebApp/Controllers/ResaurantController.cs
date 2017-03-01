using IsaProjekat.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IsaProjekat.DataAcces.Model;
using System.Data.Entity.Spatial;
using Microsoft.AspNet.Identity;

namespace IsaProjekat.WebApp.Controllers
{
    public class ResaurantController : Controller
    {
        // GET: Resaurant
        [Authorize]
        public ActionResult Index()
        {
            List<RestaurantModel> restaurants = null;
            try
            {
                using (isadbEntities context = new isadbEntities())
                {
                    restaurants = context.Restaurants.Select(x => new RestaurantModel
                    {
                        Id = x.Id,
                        Latitude = x.Location != null ? x.Location.Latitude : null,
                        Longitude = x.Location != null ? x.Location.Longitude : null,
                        ManagareName = x.User.FirstName + " " + x.User.LastName,
                        Name = x.Name
                    }).ToList();
                }
            }
            catch (Exception)
            {

                restaurants = new List<RestaurantModel>();
            }
            return View(restaurants);
        }

        public ActionResult AddResaurant()
        {
            RestaurantModel model = null;
            try
            {
                using (isadbEntities context = new isadbEntities())
                {
                    List<SelectListItem> users = context.Users.Where(x => x.AspNetUser.AspNetRoles.Any(z => z.Name.Equals("manager"))).Select(x => new SelectListItem
                    {
                        Text = x.FirstName + " " + x.LastName,
                        Value = x.Id.ToString()
                    }).ToList();
                    model = new RestaurantModel();
                    model.Managers = users;
                }
            }
            catch (Exception)
            {
                model = new RestaurantModel();
                model.Managers = new List<SelectListItem>();
                
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult AddResaurant(RestaurantModel model)
        {

            try
            {
                using (isadbEntities context = new isadbEntities())
                {
                    if (ModelState.IsValid)
                    {
                        Restaurant restaurant = new Restaurant
                        {
                            Location = CreatePoint(model.Latitude, model.Longitude),
                            ManagerId = model.ManagerId,
                            Name = model.Name
                        };
                        context.Restaurants.Add(restaurant);
                        context.SaveChanges();
                        for (int i = 0; i < 5; i++)
                        {
                            Table t = new Table
                            {
                                local_number = i + 1,
                                restaurant_id = restaurant.Id
                            };
                            context.Tables.Add(t);
                        }
                        context.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    else
                    {

                        List<SelectListItem> users = context.Users.Where(x => x.AspNetUser.AspNetRoles.Any(z => z.Name.Equals("manager"))).Select(x => new SelectListItem
                        {
                            Text = x.FirstName + " " + x.LastName,
                            Value = x.Id.ToString()
                        }).ToList();
                        model = new RestaurantModel();
                        model.Managers = users;

                        return View(model);
                    }
                }
            }
            catch (Exception)
            {
                model = new RestaurantModel();
                model.Managers = new List<SelectListItem>();

            }
            return View(model);
        }


        public ActionResult EditResaurant(long id)
        {
            RestaurantModel model = null;
            try
            {
                using (isadbEntities context = new isadbEntities())
                {
                    Restaurant restaurant = context.Restaurants.Find(id);

                    model = new RestaurantModel
                    {
                        Id = restaurant.Id,
                        Latitude = restaurant.Location != null ? restaurant.Location.Latitude : null,
                        Longitude = restaurant.Location != null ? restaurant.Location.Longitude : null,
                        Name = restaurant.Name,
                        ManagerId = restaurant.ManagerId
                    };

                    List<SelectListItem> users = context.Users.Where(x => x.AspNetUser.AspNetRoles.Any(z => z.Name.Equals("manager"))).Select(x => new SelectListItem
                    {
                        Text = x.FirstName + " " + x.LastName,
                        Value = x.Id.ToString()
                    }).ToList();
                    model.Managers = users;
                }
            }
            catch (Exception)
            {
                model = new RestaurantModel();
                model.Managers = new List<SelectListItem>();

            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditResaurant(RestaurantModel model)
        {

            try
            {
                using (isadbEntities context = new isadbEntities())
                {
                    if (ModelState.IsValid)
                    {
                        Restaurant restaurant = context.Restaurants.Find(model.Id);

                        restaurant.Location = CreatePoint(model.Latitude, model.Longitude);
                        restaurant.ManagerId = model.ManagerId;
                        restaurant.Name = model.Name;
                       
                        context.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    else
                    {

                        List<SelectListItem> users = context.Users.Where(x => x.AspNetUser.AspNetRoles.Any(z => z.Name.Equals("manager"))).Select(x => new SelectListItem
                        {
                            Text = x.FirstName + " " + x.LastName,
                            Value = x.Id.ToString()
                        }).ToList();
                        model = new RestaurantModel();
                        model.Managers = users;

                        return View(model);
                    }
                }
            }
            catch (Exception)
            {
                model = new RestaurantModel();
                model.Managers = new List<SelectListItem>();

            }
            return View(model);
        }


        [Authorize]
        public ActionResult RestaurantDetail(long id) {
            try
            {
                string aspUserId = User.Identity.GetUserId();

                using (isadbEntities context = new isadbEntities())
                {
                    Restaurant res = context.Restaurants.Find(id);
                    RestaurantModel model = new RestaurantModel
                    {
                        Id = res.Id,
                        Latitude = res.Location.Latitude,
                        Longitude = res.Location.Longitude,
                        ManagareName = res.User.FirstName + " " + res.User.LastName,
                        Name = res.Name,
                        Tables = res.Tables.Select(x => new TableModel
                        {
                            Id = x.Id,
                            IsEnabled = !x.Reservations.Any(z => z.Date_to > DateTime.Now && z.Date_from < DateTime.Now),
                            Position = x.local_number
                        }).ToList(),
                        IsAdmin = User.IsInRole("admin"),
                        IsManager = context.Users.FirstOrDefault(x => x.AspUserId.Equals(aspUserId)).Restaurants.Any(x => x.Id == id)
                    };
                    return View(model);
                }


            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }
        }

        public ActionResult Reservation(long id)
        {
            List<string> reservations = new List<string>();
            using (isadbEntities context = new isadbEntities())
            {
                reservations = context.Tables.Find(id).Reservations.Where(x => x.Date_to > DateTime.Now).Select(x => x.Date_from.ToString() + " - " + x.Date_to.ToString()).ToList();
            }

            ReservationModel model = new ReservationModel
            {
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now.AddHours(1),
                Id = id,
                Reservations = reservations
            };
            return View(model);

        }

        [Authorize]
        [HttpPost]
        public ActionResult Reservation(ReservationModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (isadbEntities context = new isadbEntities())
                    {
                        Table table = context.Tables.Find(model.Id);
                        bool isEnableReservation = !table.Reservations.Any() || !table.Reservations.Any(x => !(x.Date_from > model.DateTo || x.Date_to < model.DateFrom ));
                            //Any(x => x.Date_to < model.DateTo && x.Date_from > model.DateFrom);
                        if (isEnableReservation)
                        {
                            
                            string aspUserId = User.Identity.GetUserId();
                            User user = context.Users.FirstOrDefault(x => x.AspUserId == aspUserId);
                            Reservation reservation = new Reservation
                            {
                                Table_id = model.Id,
                                Date_from = model.DateFrom,
                                Date_to = model.DateTo,
                                User_id = user.Id
                            };

                            context.Reservations.Add(reservation);
                            context.SaveChanges();

                            return RedirectToAction("RestaurantDetail", new { id = context.Tables.Find(model.Id).restaurant_id });
                        }
                        else
                        {
                            List<string> reservations = new List<string>();
                   
                                reservations = context.Tables.Find(model.Id).Reservations.Where(x => x.Date_to > DateTime.Now).Select(x => x.Date_from.ToString() + " - " + x.Date_to.ToString()).ToList();


                            model.Reservations = reservations;

                            ModelState.AddModelError("NotEnable", "Table is not enabled for selected period");
                            return View(model);
                        }
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception)
            {
                List<string> reservations = new List<string>();
                using (isadbEntities context = new isadbEntities())
                {
                    reservations = context.Tables.Find(model.Id).Reservations.Where(x => x.Date_to > DateTime.Now).Select(x => x.Date_from.ToString() + " - " + x.Date_to.ToString()).ToList();
                }
                model.Reservations = reservations;
                return View(model);

            }
        }
        //id - restaurantId
        public ActionResult Menu(long id)
        {
            ViewBag.RestaurantId = id;
            List<MenuModel> menus = null;
            try
            {
                using (isadbEntities context = new isadbEntities())
                {
                    menus = context.Restaurants.Find(id).FoodManus.Select(x => new MenuModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Descriptiom = x.Description,
                        Price = x.Price,
                    }).ToList();
                }
            }
            catch (Exception)
            {

                menus = new List<MenuModel>();
            }
            return View(menus);
        }

        //id-restaurantId
        public ActionResult AddMenu(long id)
        {
            MenuModel model = new MenuModel();
            model.RestaurantId = id;
            return View(model);
        }

        [HttpPost]
        public ActionResult AddMenu(MenuModel model)
        {

            try
            {
                using (isadbEntities context = new isadbEntities())
                {
                    if (ModelState.IsValid)
                    {
                        FoodManu menu = new FoodManu
                        {
                            Description = model.Descriptiom,
                            Name = model.Name,
                            Price = model.Price,
                            RestaurantId = model.RestaurantId
                        };
                        context.FoodManus.Add(menu);
                        context.SaveChanges();
                       
                        return RedirectToAction("Menu", new { id = model.RestaurantId});
                    }

                    else
                    {

                        return View(model);
                    }
                }
            }
            catch (Exception)
            {

            }
            return View(model);
        }


        public ActionResult RestaurantDelete(long id)
        {
            try
            {
                using (isadbEntities context = new isadbEntities())
                {
                    Restaurant r = context.Restaurants.Find(id);
                    context.Restaurants.Remove(r);
                    context.SaveChanges();

                }
            }
            catch (Exception e)
            {

            }

                    return RedirectToAction("Index");
        }





        #region helper
        public DbGeography CreatePoint(double? lat, double? lon, int srid = 4326)
        {
            if (lat.HasValue && lon.HasValue)
            {
                string wkt = String.Format("POINT({0} {1})", lon, lat);

                return DbGeography.PointFromText(wkt, srid);
            }

            return null;
        }
        #endregion
    }



}