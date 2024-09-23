using MyApp.Domain.ViewModels.Common;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Domain.ViewModels
{
    public class FilterUserViewModel : BasePaging<UserDetailViewModel>
    {
        [Display(Name = "نام")]
        public string? FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string? LastName { get; set; }
    }
}
