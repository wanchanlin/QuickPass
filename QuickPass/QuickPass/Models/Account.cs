using System.ComponentModel.DataAnnotations;
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


    }
}
