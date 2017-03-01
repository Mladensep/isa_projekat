using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsaProjekat.WebApp.Models
{
    public class RestaurantModel
    {
        public long Id { get; set; }

        [Required] 
        public string Name { get; set; }

        public double? Longitude { get; set; }
        public double? Latitude { get; set; }

        [Display(Name = "Manager")] 
        public long ManagerId { get; set; }
        public List<SelectListItem> Managers { get; set; }

        public string ManagareName { get; set; }
        public List<TableModel> Tables { get; set; }

        public bool IsManager { get; set; }

        public bool IsAdmin { get; set; }
    }
    public class TableModel
    {
        public long Id { get; set; }
        public int? Position { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class MenuModel
    {
        public long Id { get; set; }
        public long RestaurantId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Descriptiom { get; set; }

        [Required]
        public decimal? Price { get; set; }

    }
}