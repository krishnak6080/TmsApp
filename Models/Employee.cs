using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TmsApp.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Allocations = new HashSet<Allocation>();
        }
        [Required, Range(1000, 9999, ErrorMessage = "Please enter four digit number"), Display(Name = "Employee ID")]
        public int EmployeeId { get; set; }
        [Required, Display(Name = "Employee Name"), RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Please enter characters only")]
        public string EmployeeName { get; set; } = null!;
        [Required, Display(Name = "Age"), Range(18, 60, ErrorMessage = "Please make sure that the age is between 18 and 61")]
        public int Age { get; set; }
        [Required, RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Please enter characters only"), Display(Name = "Employee Location", ShortName = "Location")]
        public string EmpLocation { get; set; } = null!;
        [Required, Range(1000000000, 9999999999, ErrorMessage = "Please enter a valid phone number"), Display(Name = "Phone Number", ShortName = "Phone")]
        public long PhoneNumber { get; set; }

        public virtual ICollection<Allocation> Allocations { get; set; }
    }
}
