using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Database
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public int ClientId { get; set; }
        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }
        [Required]
        public int TypeDeliveryId { get; set; }
        [Required]
        [DefaultValue(false)]
        public bool IsStatusPaid { get; set; }
        [DefaultValue(false)]
        public bool? IsStatusDelivery { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Description { get; set; }
        [Required]
        public int From { get; set; }
        [ForeignKey("From")]
        public DeliveryPoint FromDeliveryPoint { get; set; }
        [Required]
        public int To { get; set; }
        [ForeignKey("To")]
        public DeliveryPoint ToDeliveryPoint { get; set; }
        public Client Client { get; set; }
        public TypeDelivery TypeDelivery { get; set; }

        [NotMapped]
        public string StatusPaid
        {
            get => IsStatusPaid == true ? "Оплачено" : "Не оплачено";
        }
        [NotMapped]
        public string StatusDelivery
        {
            get
            {
                if (IsStatusDelivery==null)
                {
                    return "Не доставлен";
                }
                return IsStatusDelivery == true ? "Доставлен" : "В работе";
            }
        }
    }
}
