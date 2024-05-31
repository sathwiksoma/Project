using System.ComponentModel.DataAnnotations.Schema;

namespace HotPotProject.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? UserName { get; set; }

        [ForeignKey("UserName")]
        public User? User { get; set; }

        public Customer()
        {

        }

        public Customer(int id, string name, string email, string phone)
        {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
        }

        public Customer(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            Phone = phone;
        }
    }
}
