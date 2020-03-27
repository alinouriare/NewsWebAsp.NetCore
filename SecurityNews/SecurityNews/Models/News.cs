using SecurityNews.PublicClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Models
{
    public class News
    {

        //اخبار
        [Key]
        public int NewsId { get; set; }

        [Display(Name = "عنوان خبر")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        [StringLength(250, MinimumLength = 4, ErrorMessage = PublicConst.LengthMessage)]
        //[RegularExpression(@"[0-9A-Zا-یa-z_\s\-\(\)\.]+", ErrorMessage = PublicConst.DangrouseMessageForBadCharachter)]
        public string Title { get; set; }


        [Display(Name = "متن خبر")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        public string Content { get; set; }


        [Display(Name = "چکیده")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        //[StringLength(400, MinimumLength = 5, ErrorMessage = PublicConst.LengthMessage)]
        //[RegularExpression(@"[0-9A-Zا-یa-z_\s\-\(\)\.]+", ErrorMessage = PublicConst.DangrouseMessageForBadCharachter)]
        public string Abstract { get; set; }

        [Display(Name = "تعداد بازدید")]
        public int VisitCount { get; set; }


      //time date


        [Display(Name = "تاریخ خبر")]
        public string NewsDate { get; set; }

        [Display(Name = "زمان خبر")]
        public string NewsTime { get; set; }




        //time date 2


        [Display(Name = "تاریخ درج خبر")]
        public DateTime NewsDateEN { get; set; }

       





        [Display(Name = "تصویر شاخص")]
        public string IndexImage { get; set; }


        [Display(Name = "محل ارسال خبر")]
        public byte PlaceNewsID { get; set; }


        [Display(Name = "نوع خبر")]
        public byte NewsType { get; set; }

        [Display(Name = "CVE")]
        public string CVE { get; set; }


        [Display(Name = "patch")]
        public string Path { get; set; }


        [Display(Name = "سطح تاثیر")]
        public int CategoryImpactID { get; set; }
        [Display(Name = "پلتفرم")]
        public int CategoryPlatformID { get; set; }



        //SEO Property
        [Display(Name = "متاتگ ها")]
        public string MetaTag { get; set; }
        [Display(Name = "متای توضیحات")]
        public string MetaDescription { get; set; }
        //

        public string UserID { get; set; }



        [ForeignKey(nameof(CategoryImpactID))]
        public virtual CategoryImpact tblCategoryImpact { get; set; }

        [ForeignKey(nameof(CategoryPlatformID))]
        public virtual CategoryPlatform tblCategoryPlatform { get; set; }

        [ForeignKey(nameof(UserID))]
        public virtual ApplicationUsers Users { get; set; }
    }
}
