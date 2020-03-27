using SecurityNews.PublicClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Models.ViewModels
{
    public class ApplicationRoleViewModel
    {

        public string Id { get; set; }

        [Display(Name = "عنوان نقش(انگلیسی)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        public string Name { get; set; }

        [Display(Name = "نام بخش")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        public string Description { get; set; }

        [Display(Name = "زیردسته ها")]
        public string RoleLevel { get; set; }

        public int NumberOfUsers { get; set; }
    }
}
