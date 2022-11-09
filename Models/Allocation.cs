using System;
using System.Collections.Generic;

namespace TmsApp.Models
{
    public partial class Allocation
    {
        public int AllocationId { get; set; }
        public int? EmployeeId { get; set; }
        public int? VehicleNumber { get; set; }
        public int? RouteNumber { get; set; }

        public virtual Employee? Employee { get; set; }
        public virtual RoutePath? RouteNumberNavigation { get; set; }
        public virtual Vehicle? VehicleNumberNavigation { get; set; }
    }
}
