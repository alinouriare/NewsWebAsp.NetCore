using SecurityNews.PublicClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Models.SpiderModel
{
    public class MainLink
    {
        [Key]
        public int MainLinkId { get; set; }

        public string Url { get; set; }

        public string Titel { get; set; }

        public DateTime DateNews { get; set; }

        public string XpathTitle { get; set; }

       public string XpathContent { get; set; }

        public bool Status { get; set; }


    }
}
