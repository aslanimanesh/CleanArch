using System.ComponentModel.DataAnnotations;

namespace MyApp.Domain.Models
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        public int Age { get; set; }

        // Many-to-many relationship with Discount
        public ICollection<UserDiscount> UserDiscounts { get; set; }
    }
}
