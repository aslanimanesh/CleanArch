using MyApp.Domain.ViewModels.Common;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Domain.ViewModels.Users
{
    public class FilterUserViewModel : BasePaging<UserDetailViewModel>
    {

        #region Properties

        [Display(Name = "نام")]
        public string? FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string? LastName { get; set; }

        #endregion

    }
}
