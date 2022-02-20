using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SayBergETrade.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
       
        public string Address { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string PostalCode { get; set; }

        [NotMapped] //karşılığı olmayan 
        public string Role { get; set; }
    }
}
