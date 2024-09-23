using MyApp.Domain.Models;

namespace MyApp.Domain.ViewModels
{
    public class UserViewModel
    {
        public IEnumerable<User> Users { get; set; }
    }
}
