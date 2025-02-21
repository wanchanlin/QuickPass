namespace QuickPass.Models.ViewModels
{
    public class EventDetails
    {
        public required  Event Event { get; set; }
        public List<TicketDTO>? EventTickets { get; set; }
    }
}
