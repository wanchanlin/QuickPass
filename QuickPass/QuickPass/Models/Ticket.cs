using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuickPass.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        public decimal Price { get; set; }

        public string SeatNumber { get; set; }

        public DateTime BookingDate { get; set; }

        ////User AccountId FK
        [ForeignKey("Account")]

        //[JsonIgnore]
        public int AccountId { get; set; }
        //public virtual Account Account { get; set; }

        [ForeignKey("Event")]

        //[JsonIgnore]
        public int EventId { get; set; }
        //public virtual Account Account { get; set; }
    }
}
