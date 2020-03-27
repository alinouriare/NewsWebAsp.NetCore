using SecurityNews.Models.Repository;
using SecurityNews.Models.SpiderModel;
using SecurityNews.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SecurityNews.Models.Poll;

namespace SecurityNews.Models.UnitOfWork
{
    public interface IUnitOfWork
    {

        CrudGenericMethod<CategoryImpact> CategoryImpactRepositoryUW { get; }

        CrudGenericMethod<CategoryPlatform> CategoryPlatformRepositoryUW { get; }

        CrudGenericMethod<News> NewsRepositoryUW { get; }

        CrudGenericMethod<ApplicationUsers> UserManagerUW { get; }

        CrudGenericMethod<Comment> commentRepositoryUW { get; }

        CrudGenericMethod<Advertise> AdvertisRepositoryUW { get;}

        CrudGenericMethod<Poll> PollRepositoryUW { get; }

        CrudGenericMethod<PollOption> PollOptionRepositoryUW { get; }

        CrudGenericMethod<SiteSetting> SiteSettingRepositoryUW { get; }

        CrudGenericMethod<VisitorHits> VisitRepositoryUW { get; }




        CrudGenericMethod<MainLink> MainLinkRepositoryUW { get; }

        CrudGenericMethod<InternalLink> InternalLinkRepositoryUW { get; }

        CrudGenericMethod<NewsSpider> NewsSpiderRepositoryUW { get; }



        IEntityDataBaseTransaction BeginTransaction();

        void Save();
    }
}
