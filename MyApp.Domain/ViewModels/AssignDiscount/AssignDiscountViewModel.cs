using System.ComponentModel.DataAnnotations;

namespace MyApp.Domain.ViewModels.AssignDiscount
{
    public class AssignDiscountViewModel
    {
        [Required(ErrorMessage = "تخفیف الزامی است.")]
        public int DiscountId { get; set; }

        [Required(ErrorMessage = "حداقل یک محصول باید انتخاب شود.")]
        public List<int> ProductIds { get; set; } = new List<int>();
    }
}
