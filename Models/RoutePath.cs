using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TmsApp.Models
{
    public partial class RoutePath
    {
        public RoutePath()
        {
            Allocations = new HashSet<Allocation>();
        }
        [Required, Range(0, 200, ErrorMessage = "Please enter a number between 0 and 200")]
        public int RouteNumber { get; set; }
        public int? VehicleNumber { get; set; }
        [Required, Display(Name = "Pickup Location", ShortName = "Pickup"), RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Please enter characters only")]
        public string Pickup { get; set; } = null!;
        [Required, Display(Name = "Stop Location", ShortName = "Stop"), RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Please enter characters only")]
        public string VehicleStop { get; set; } = null!;

        public virtual Vehicle? VehicleNumberNavigation { get; set; }
        public virtual ICollection<Allocation> Allocations { get; set; }
    }
}
