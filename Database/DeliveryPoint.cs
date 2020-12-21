using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database
{
    public class DeliveryPoint
    {
        public int Id { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        public string Name { get; set; }
        public List<ClientPoint> ClientPoints { get; set; }
    }
}
