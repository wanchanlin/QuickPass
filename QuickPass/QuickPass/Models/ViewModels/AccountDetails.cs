namespace QuickPass.Models.ViewModels
{
    public class AccountDetails
    {
        public required AccountDTO Account { get; set; }
        public List<TicketDTO>? AccountTickets { get; set; }
    }
}