using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Models
{
    public class SiteSetting
    {
        [Key]
        public int id { get; set; }

        [Display(Name = "تگ ها")]
        public string MetaTag { get; set; }

        [Display(Name = "متای توضیحات")]
        public string MetaDescription { get; set; }

        [Display(Name = "عنوان سایت")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicClass.PublicConst.EnterMessage)]
        public string SiteTitle { get; set; }
    }
}
