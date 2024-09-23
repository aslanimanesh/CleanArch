using System.ComponentModel.DataAnnotations;

namespace MyApp.Domain.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
