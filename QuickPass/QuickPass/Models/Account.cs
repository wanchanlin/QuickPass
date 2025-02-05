using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


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


        // one account can have many events
        public ICollection<Event> Events { get; set; }

        [JsonIgnore]
        public int TicektID { get; set; }



    }
}
