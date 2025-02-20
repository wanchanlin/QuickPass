using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using QuickPass.Data;

namespace QuickPass.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Venue { get; set; }

        public DateTime Date { get; set; }
        public int TotalTickets { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

    }
}

