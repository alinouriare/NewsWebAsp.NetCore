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
    public class UnitOfWork: IDisposable, IUnitOfWork
    {


        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        private CrudGenericMethod<CategoryImpact> _CategoryImpactRepositoryUW;

        private CrudGenericMethod<CategoryPlatform> _CategoryPlatformRepositoryUW;

        private CrudGenericMethod<News> _NewsRepositoryUW;

        private CrudGenericMethod<ApplicationUsers> _userManager;

        private CrudGenericMethod<Comment> _commentRepositoryUW;

        private CrudGenericMethod<Advertise> _AdvertiseRepositoryUW;

        private CrudGenericMethod<Poll> _PollRepositoryUW;

        private CrudGenericMethod<PollOption> _PolloptionRepositoryUW;

        private CrudGenericMethod<SiteSetting> _SiteSettingRepositoryUW;

        private CrudGenericMethod<VisitorHits> _VisitRepositoryUW;



        private CrudGenericMethod<InternalLink> _InternalLinkRepositoryUW;



        private CrudGenericMethod<NewsSpider> _NewsSpiderRepositoryUW;


        
        private CrudGenericMethod<MainLink> _MainLinkRepositoryUW;


        public CrudGenericMethod<MainLink> MainLinkRepositoryUW
        {
            get
            {

                if (_MainLinkRepositoryUW == null)
                {
                    _MainLinkRepositoryUW = new CrudGenericMethod<MainLink>(_context);
                }
                return _MainLinkRepositoryUW;
            }
        }






        public CrudGenericMethod<NewsSpider> NewsSpiderRepositoryUW
        {
            get
            {

                if (_NewsSpiderRepositoryUW == null)
                {
                    _NewsSpiderRepositoryUW = new CrudGenericMethod<NewsSpider>(_context);
                }
                return _NewsSpiderRepositoryUW;
            }
        }




        public CrudGenericMethod<InternalLink> InternalLinkRepositoryUW
        { get {

                if (_InternalLinkRepositoryUW == null)
                {
                    _InternalLinkRepositoryUW = new CrudGenericMethod<InternalLink>(_context);
                }
                return _InternalLinkRepositoryUW;
            } }


        //آمار بازدید از سایت 
        public CrudGenericMethod<VisitorHits> VisitRepositoryUW
        {
            //فقط خواندنی
            get
            {
                if (_VisitRepositoryUW == null)
                {
                    _VisitRepositoryUW = new CrudGenericMethod<VisitorHits>(_context);
                }
                return _VisitRepositoryUW;
            }
        }

        public CrudGenericMethod<SiteSetting> SiteSettingRepositoryUW { get {

                if (_SiteSettingRepositoryUW==null)
                {
                    _SiteSettingRepositoryUW = new CrudGenericMethod<SiteSetting>(_context);
                }
                return _SiteSettingRepositoryUW;
            }}


        public CrudGenericMethod<Poll> PollRepositoryUW { get {


                if (_PollRepositoryUW==null)
                {
                    _PollRepositoryUW = new CrudGenericMethod<Poll>(_context);
                }

                return _PollRepositoryUW;
            } }



        public CrudGenericMethod<PollOption> PollOptionRepositoryUW { get {


                if (_PolloptionRepositoryUW==null)
                {
                    _PolloptionRepositoryUW = new CrudGenericMethod<PollOption>(_context);
                }
                return _PolloptionRepositoryUW;
            } }


        public CrudGenericMethod<Advertise> AdvertisRepositoryUW
        { get {


                if (_commentRepositoryUW==null)
                {
                    _AdvertiseRepositoryUW = new CrudGenericMethod<Advertise>(_context);
                }
                return _AdvertiseRepositoryUW;
            }
            
        }

        public CrudGenericMethod<Comment> commentRepositoryUW { get {

                if (_commentRepositoryUW==null)
                {
                    _commentRepositoryUW = new CrudGenericMethod<Comment>(_context);
                }
                return _commentRepositoryUW;
            } }


        public CrudGenericMethod<ApplicationUsers> UserManagerUW
        { get {

                if (_userManager==null)
                {
                    _userManager = new CrudGenericMethod<ApplicationUsers>(_context);
                }
                return _userManager;
            } }

        public CrudGenericMethod<News> NewsRepositoryUW { get {


                if (_NewsRepositoryUW==null)
                {
                    _NewsRepositoryUW = new CrudGenericMethod<News>(_context);
                }
                return _NewsRepositoryUW;
            }}

        public CrudGenericMethod<CategoryPlatform> CategoryPlatformRepositoryUW { get {

                if (_CategoryPlatformRepositoryUW==null)
                {
                    _CategoryPlatformRepositoryUW = new CrudGenericMethod<CategoryPlatform>(_context);
                }
                return _CategoryPlatformRepositoryUW;
            } }

        public CrudGenericMethod<CategoryImpact> CategoryImpactRepositoryUW { get {

                if (_CategoryImpactRepositoryUW==null)
                {
                    _CategoryImpactRepositoryUW=new CrudGenericMethod<CategoryImpact>(_context);
                }
                return _CategoryImpactRepositoryUW;
            } }



        //مدیریت تراکنش
        public IEntityDataBaseTransaction BeginTransaction()
        {
            return new EntityDataBaseTransaction(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
