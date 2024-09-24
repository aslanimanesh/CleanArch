using System.ComponentModel.DataAnnotations;

namespace MyApp.Domain.Models.Common
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
