using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarInsQuote.Models
{
    public class Insuree
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CarYear { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public string DUI { get; set; }
        public int SpeedingTicket { get; set; }
        public string CoverageType { get; set; }
        public double Quote { get; set; }
    }
}