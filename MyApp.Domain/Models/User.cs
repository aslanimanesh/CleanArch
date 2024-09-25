using System.ComponentModel.DataAnnotations;
using MyApp.Domain.Models.Common;

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
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        public bool IsActive { get; set; }


        // Many-to-many relationship with Discount
        public ICollection<UserDiscount> UserDiscounts { get; set; }
    }
}
