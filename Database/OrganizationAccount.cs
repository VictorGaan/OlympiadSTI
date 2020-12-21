using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database
{
    public class OrganizationAccount
    {
        public int Id { get; set; }
        [Required]
        public decimal Wallet { get; set; }
        [Required]
        public string PublicKey { get; set; }
        [Required]
        public string PrivateKey { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
