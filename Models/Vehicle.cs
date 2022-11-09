using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TmsApp.Models
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            Allocations = new HashSet<Allocation>();
            RoutePaths = new HashSet<RoutePath>();
        }
        [Required, Range(1000, 9999, ErrorMessage = "Please enter a four digit number")]
        public int VehicleNumber { get; set; }
        [Required, Range(0, 10, ErrorMessage = "Seats should be within 0 and 10")]
        public int Capacity { get; set; }
        [Required, Range(0, 10, ErrorMessage = "Seats should be within 0 and 10")]
        public int AvailableSeats { get; set; }
        public bool Operable { get; set; }

        public virtual ICollection<Allocation> Allocations { get; set; }
        public virtual ICollection<RoutePath> RoutePaths { get; set; }
    }
}
