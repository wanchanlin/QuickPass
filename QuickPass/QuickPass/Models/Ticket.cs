using System.ComponentModel.DataAnnotations; 
namespace QuickPass.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        
        public decimal Price { get; set; }

        public int SeatNumber { get; set; }

        public DateTime BookingDate { get; set; }


    }
}
