using SecurityNews.PublicClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Models
{
    public class Advertise
    {

        [Key]
        public int AdId { get; set; }

        [Display(Name = "تصویر")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        public string gifPath { get; set; }

        [Display(Name = "از تاریخ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        public string FromDate { get; set; }

        [Display(Name = "تا تاریخ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        public string ToDate { get; set; }

        [Display(Name = "لینک تبلیغ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        public string Link { get; set; }

        [Display(Name = "وضعیت")]
        public byte flag { get; set; }

        [Display(Name = "محل نمایش")]
        public byte Advlocation { get; set; }

    }


    public class AdvertisePlace
    {
        public int AdvId { get; set; }
        public string AdvLocationName { get; set; }

        public List<AdvertisePlace> AdvertiseDescription()
        {
            var model = new List<AdvertisePlace>
            {
                new AdvertisePlace {AdvId = 1, AdvLocationName = "سمت چپ"},
                new AdvertisePlace {AdvId = 2, AdvLocationName = "زیر اسلایدر"},
                new AdvertisePlace {AdvId = 3, AdvLocationName = "زیر اخبار ویژه"},
                new AdvertisePlace {AdvId = 4, AdvLocationName = "زیر آخرین ویدیوها"},
            };
            return model;
        }
    }
}
