namespace SoftGym.Web
{
    using System.Reflection;

    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using SendGrid;
    using SoftGym.Data;
    using SoftGym.Data.Common;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Data.Repositories;
    using SoftGym.Data.Seeding;
    using SoftGym.Services.Data;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Services.Mapping;
    using SoftGym.Services.Messaging;
    using SoftGym.Web.ViewModels;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            // Add Facebook auth
            services.AddAuthentication()
                .AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = this.configuration["Facebook:AppId"];
                facebookOptions.AppSecret = this.configuration["Facebook:AppSecret"];
                facebookOptions.Scope.Add("public_profile");
                facebookOptions.Fields.Add("name");
                facebookOptions.Fields.Add("picture");
            });

            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });
            services.AddRazorPages();

            services.AddSingleton(this.configuration);

            // Add clodinary
            var cloudinary = new Cloudinary(new Account()
            {
                Cloud = this.configuration["Cloudinary:CloudName"],
                ApiKey = this.configuration["Cloudinary:ApiKey"],
                ApiSecret = this.configuration["Cloudinary:ApiSecret"],
            });
            services.AddSingleton(cloudinary);

            // Add sendgrid
            var sendGrid = new SendGridClient(this.configuration["SendGrid:ApiKey"]);
            services.AddSingleton(sendGrid);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, SendGridEmailSender>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<IQrCodeService, QrCodeService>();
            services.AddTransient<ICardsService, CardsService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IRolesService, RolesService>();
            services.AddTransient<IFacilitiesService, FacilitiesService>();
            services.AddTransient<IMealsService, MealsService>();
            services.AddTransient<ITrainersService, TrainersService>();
            services.AddTransient<IEatingPlansService, EatingPlansService>();
            services.AddTransient<IExercisesService, ExercisesService>();
            services.AddTransient<IWorkoutPlansService, WorkoutPlansService>();
            services.AddTransient<IPaypalService, PaypalService>();
            services.AddTransient<IExportsService, ExportsService>();
            services.AddTransient<INotificationsService, NotificationsService>();
            services.AddTransient<IAppointmentsService, AppointmentsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (env.IsDevelopment())
                {
                    dbContext.Database.Migrate();
                }

                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithRedirects("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapRazorPages();
                });
        }
    }
}