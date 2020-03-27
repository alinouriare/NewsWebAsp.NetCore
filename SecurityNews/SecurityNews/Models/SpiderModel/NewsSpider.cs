using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Models.SpiderModel
{
    public class NewsSpider
    {
        [Key]
        public int IdNews { get; set; }

        public string Titel { get; set; }

        public string Content { get; set; }

        public DateTime NewsDate { get; set; }

        public bool Status { get; set; }

        public int IntrnalLinkID { get; set; }

        [ForeignKey(nameof(IntrnalLinkID))]
        public virtual InternalLink tblInternalLink { get; set; }
    }
}
