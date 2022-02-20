using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SayBergETrade.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            ProductCount = 1;
        }
        [Key]
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser{ get; set; }
        public int ProductId{ get; set; }
        [ForeignKey("ProductId")]
        public Product Product{ get; set; }
        public int ProductCount{ get; set; }
        [NotMapped]
        public double ProductPrice{ get; set; }
    }
}
