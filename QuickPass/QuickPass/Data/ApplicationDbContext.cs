using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuickPass.Models;

namespace QuickPass.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //create a Accounts table from the model
        public DbSet<Account> Accounts { get; set; }
        //create a Tickets table from the model

        public DbSet<Ticket> Tickets { get; set; }

        //create an Events table from the model

        public DbSet<Event> Events { get; set; }
    }
}

