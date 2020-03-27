using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.AspNetCore;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.ConnectionParameters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using SecurityNews.Models;
using SecurityNews.Models.Repository;
using SecurityNews.Models.Services;
using SecurityNews.Models.UnitOfWork;
using SecurityNews.Services;

namespace SecurityNews
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(env.ContentRootPath)
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                 .AddEnvironmentVariables();
            FileProvider = env.ContentRootFileProvider;

            Configuration = builder.Build();




        }
      
        public IFileProvider FileProvider { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //سرویس connection string 
            services.AddDbContext<ApplicationDbContext>(option =>
            option.UseSqlServer(Configuration.GetConnectionString("MyConnectionString")));

            //سرویس Identity Service
            services.AddIdentity<ApplicationUsers, ApplicationRoles>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();











            //Repository


            services.AddScoped<IAspNetUserRoleRepository, AspNetUserRoleRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUploadfile, UploadFile>();
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IAdvertiseRepository, AdvertiseRepository>();
            services.AddScoped<IPollRepository, PollRepository>();
            services.AddScoped<IVisitRepository, VisitRepository>();


            //کاربر غیر مجاز اینجا بیاد


            services.ConfigureApplicationCookie(option => option.LoginPath = "/Home");

            //Data grid

            services.AddDevExpressControls();
            //services.AddDevExpressControls(options =>
            //{
            //    options.Bootstrap(bootstrap =>
            //    {
            //        bootstrap.Mode = BootstrapMode.Bootstrap3;
            //    });
            //});



            // Add a DashboardController class descendant with a specified dashboard storage 
            // and a connection string provider. 
            services
            .AddMvc()
            .AddDefaultDashboardController(configurator => {
                configurator.SetDashboardStorage(new DashboardFileStorage(FileProvider.GetFileInfo("App_Data/Dashboards").PhysicalPath));


                configurator.SetConnectionStringsProvider(new DashboardConnectionStringsProvider(Configuration));
               





            });


            // Add the third-party (JQuery, Knockout, etc.) and DevExtreme libraries. 
            //services.AddDevExpressControls(settings => settings.Resources = ResourcesType.ThirdParty | ResourcesType.DevExtreme);


            services.AddDevExpressControls(options =>
            {
                options.Resources = ResourcesType.DevExtreme | ResourcesType.ThirdParty;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //env.EnvironmentName = EnvironmentName.Production;
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            app.UseStaticFiles();
            // Register the DevExpress middleware. 
            app.UseDevExpressControls();
            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapDashboardRoute();
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });




            // For Area
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "AdminPanel",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
