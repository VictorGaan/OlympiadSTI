using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database
{
    public class Delivery
    {
        public int Id { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        public int OrderId { get; set; }
        public Employee Employee { get; set; }
    }
}
