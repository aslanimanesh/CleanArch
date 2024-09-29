using System.ComponentModel.DataAnnotations;

namespace MyApp.Domain.ViewModels.Users
{
    public class CreateUserViewModel
    {

        #region Properties

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
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        public bool IsActive { get; set; } = true;

        #endregion

    }
}
