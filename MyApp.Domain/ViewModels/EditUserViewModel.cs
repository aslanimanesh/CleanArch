using MyApp.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Domain.ViewModels
{
    public class EditUserViewModel : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
