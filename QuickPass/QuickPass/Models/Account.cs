using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using QuickPass.Data;


namespace QuickPass.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public required string Email { get; set; }
        public string Password { get; set; }


        public List<Ticket> Tickets { get; set; }
        //public ICollection<Ticket> Tickets { get; set; }


    }

    /// <summary>
    /// Data Transfer Object (DTO) for Account.
    /// </summary>
    public class AccountDTO
    {
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // ✅ Add this
        public List<TicketDTO> Tickets { get; set; } = new List<TicketDTO>();
    }

    /// <summary>
    /// Data Transfer Object (DTO) for Ticket.
    /// </summary>
    public class TicketDTO
    {
        public int TicketId { get; set; }
        public decimal Price { get; set; }
        public string SeatNumber { get; set; }
        public DateTime BookingDate { get; set; }
        public int EventId { get; set; }
    }


}


