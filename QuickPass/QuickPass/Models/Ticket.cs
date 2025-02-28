﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Text.Json.Serialization; // Prevents circular references


namespace QuickPass.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        public decimal Price { get; set; }

        public string SeatNumber { get; set; }

        public DateTime BookingDate { get; set; }

        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public virtual Account? Account { get; set; }


        //[JsonIgnore]
        //public virtual Account Account { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }
        public virtual Event? Event { get; set; }
        //[JsonIgnore]
        //public virtual Event Event { get; set; }
    }
}
