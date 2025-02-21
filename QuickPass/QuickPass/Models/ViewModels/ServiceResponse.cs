namespace QuickPass.Models.ViewModels
{
    public class ServiceResponse
    {
        public enum ServiceStatus
        {
            Created,
            Updated,
            Deleted,
            Error
        }
        public ServiceStatus Status { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
        public int CreatedId { get; set; } 
    }
}