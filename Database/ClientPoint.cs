using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database
{
    public class ClientPoint
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int DeliveryPointId { get; set; }
        public Client Client { get; set; }
        public DeliveryPoint DeliveryPoint { get; set; }
    }
}
