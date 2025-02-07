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


public static class AccountEndpoints
{
	public static void MapAccountEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Account").WithTags(nameof(Account));

        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            return await db.Accounts.ToListAsync();
        })
        .WithName("GetAllAccounts")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Account>, NotFound>> (int accountid, ApplicationDbContext db) =>
        {
            return await db.Accounts.AsNoTracking()
                .FirstOrDefaultAsync(model => model.AccountId == accountid)
                is Account model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetAccountById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int accountid, Account account, ApplicationDbContext db) =>
        {
            var affected = await db.Accounts
                .Where(model => model.AccountId == accountid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.AccountId, account.AccountId)
                  .SetProperty(m => m.FirstName, account.FirstName)
                  .SetProperty(m => m.LastName, account.LastName)
                  .SetProperty(m => m.Email, account.Email)
                  .SetProperty(m => m.Password, account.Password)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateAccount")
        .WithOpenApi();

        group.MapPost("/", async (Account account, ApplicationDbContext db) =>
        {
            db.Accounts.Add(account);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Account/{account.AccountId}",account);
        })
        .WithName("CreateAccount")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int accountid, ApplicationDbContext db) =>
        {
            var affected = await db.Accounts
                .Where(model => model.AccountId == accountid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteAccount")
        .WithOpenApi();
    }
}}
