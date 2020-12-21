using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        public int PersonId { get; set; }
        [Required]
        public int RoleId { get; set; }
        [Required]
        public int OrganizationAccountId { get; set; }
        public OrganizationAccount OrganizationAccount { get; set; }
        public Person Person { get; set; }
        public Role Role { get; set; }
        public List<Delivery> Deliveries { get; set; }
    }
}
