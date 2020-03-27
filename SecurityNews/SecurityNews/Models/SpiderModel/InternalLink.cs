using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Models.SpiderModel
{
    public class InternalLink
    {
        [Key]
        public int InternalLinkId { get; set; }

        public string IntrnalUrl { get; set; }

        public DateTime SpiderDate { get; set; }

        public bool SpiderStatus { get; set; }

        public int MainlinkID { get; set; }

        [ForeignKey(nameof(MainlinkID))]
        public virtual MainLink tblMainLink { get; set; }

    }
}
