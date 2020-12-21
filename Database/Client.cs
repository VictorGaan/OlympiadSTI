using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database
{
    public class Client
    {
        public int Id { get; set; }
        [Required]
        public int PersonId { get; set; }
        [Required]
        public decimal Wallet { get; set; }
        [Required]
        public string PublicKey { get; set; }
        [Required]
        public string PrivateKey { get; set; }
        public Person Person { get; set; }
        public List<ClientPoint> ClientPoints { get; set; }
    }
}
