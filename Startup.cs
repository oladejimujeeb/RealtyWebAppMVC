using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RealtyWebApp.Context;
using RealtyWebApp.Implementation.Repositories;
using RealtyWebApp.Implementation.Services;
using RealtyWebApp.Implementation.Services.PropertyMethod;
using RealtyWebApp.Interface.IRepositories;
using RealtyWebApp.Interface.IServices;
using RealtyWebApp.Interface.IServices.IPropertyMethod;
using RealtyWebApp.MailFolder.EmailService;
using RealtyWebApp.MailFolder.EmailSettings;

namespace RealtyWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddScoped<IPropertyServiceMethod, PropertyServiceMethod>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IMailService,MailService>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IRealtorRepository, RealtorRepository>();
            services.AddScoped<IBuyerRepository, BuyerRepository>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IPropertyDocument, PropertyDocumentRepository>();
            services.AddScoped<IVisitationRepository, VisitationRepository>();
            services.AddScoped<IPropertyImage, PropertyImageRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleRepository,RoleRepository>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IBuyerService,BuyerService>();
            services.AddScoped<IRealtorService, RealtorService>();
            services.AddScoped<IPropertyImageService,PropertyImageService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.Configure<MailSetting>(Configuration.GetSection("MailSetting"));
            services.AddDbContext<ApplicationContext>(options => options.UseMySql
                (Configuration.GetConnectionString("ApplicationContext"),
                ServerVersion.AutoDetect(Configuration.GetConnectionString("ApplicationContext"))));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(config =>
                {
                    config.LoginPath = "/User/Login";
                    config.Cookie.Name = "RealtyApp";
                    config.LogoutPath = "/User/LogOut";
                    config.ExpireTimeSpan = TimeSpan.FromMinutes(40);
                    config.AccessDeniedPath = "/User/Login";
                    config.ClaimsIssuer = "Realty Mulad ";

                });
            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    );
                
            });
        }
    }
}