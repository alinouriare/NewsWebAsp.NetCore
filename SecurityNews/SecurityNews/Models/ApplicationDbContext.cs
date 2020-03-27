using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using SecurityNews.Models;
using System.Threading.Tasks;
using SecurityNews.Models.SpiderModel;

namespace SecurityNews.Models
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUsers,ApplicationRoles,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {




        }

        public DbSet<CategoryImpact> CategoryImpact_Tbl { get; set; }

        public DbSet<CategoryPlatform> CategoryPlatform_Tbl { get; set; }

        public DbSet<News> News_Tbl { get; set; }

        public DbSet<Comment> Comments_Tbl { get; set; }

        public DbSet<Advertise> Advertis_Tbl { get; set; }

        public DbSet<Poll> Poll_Tbl { get; set; }

        public DbSet<PollOption> Polloption_Tbl { get; set; }

        public DbSet<SiteSetting> SiteSetting_Tbl { get; set; }

        public DbSet<VisitorHits> Visit_Tbl { get; set; }

        public DbSet<MainLink> MainLink_tbl { get; set; }

        public DbSet<InternalLink> InternalLink_tbl { get; set; }

        public DbSet<NewsSpider> NewsSpider_tbl { get; set; }
    }
}
