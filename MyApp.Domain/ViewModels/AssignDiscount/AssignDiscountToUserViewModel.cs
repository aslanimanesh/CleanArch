namespace MyApp.Domain.ViewModels.AssignDiscount
{
    public class AssignDiscountToUserViewModel
    {
        public int DiscountId { get; set; }
        public List<int> UserIds { get; set; } = new List<int>();
        public List<UserViewModel> Users { get; set; } = new List<UserViewModel>();
    }
}
