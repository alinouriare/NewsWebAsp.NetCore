using Microsoft.AspNetCore.Identity;
using SecurityNews.PublicClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Models
{
    public class ApplicationUsers:IdentityUser
    {

        [Display(Name ="نام")]
        //[Required(AllowEmptyStrings =false,ErrorMessage = PublicConst.EnterMessage)]
        public string Name { get; set; }

        [Display(Name ="فامیلی")]
        //[Required(AllowEmptyStrings =false,ErrorMessage =PublicConst.EnterMessage)]
        public string  Family { get; set; }

      
        [Display(Name = "جنسیت")]
        public byte gender { get; set; }

        [Display(Name = "تلفن")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        public override string PhoneNumber { get; set; }


        [Display(Name = "تصویر")]
        public string UserImagePath { get; set; }

        [Display(Name = "تاریخ تولد")]
        public string BirthDayDate { get; set; }
    }
}
