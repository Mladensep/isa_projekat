using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IsaProjekat.WebApp.Models
{
    public class ReservationModel
    {
        public long Id { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public List<string> Reservations { get; set; } 
    }
}