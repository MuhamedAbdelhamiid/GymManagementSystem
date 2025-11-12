using GymManagementBLL.BusinessServices.Implementation;
using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.Helper;
using GymManagementBLL.Mapping;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Data.Seed;
using GymManagementDAL.Repositories.Implementation;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementDAL.Unit_Of_Work;
using Microsoft.EntityFrameworkCore;

namespace GymManagmentPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region DI Registeration
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // CLR now can add object from GymDbContext when needed
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                //options.UseSqlServer(
                //    builder.Configuration.GetSection("ConnectionStrings")["DefaultConnection"]
                // );
                //options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                );
            });

            // Clr will assign object from type UnitOfWork when it asked to inject object from class that
            // implement IUnitOfWork
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<IAnaltyicalService, AnaltyicalService>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();
            builder.Services.AddScoped<IMembershipService, MembershipService>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IBookingService, BookingService>();

            builder.Services.AddAutoMapper(x => x.AddProfile(typeof(MappingProfile)));
            #endregion

            var app = builder.Build();

            #region Data Seeding
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();

            var pendingMigrations = dbContext.Database.GetPendingMigrations();

            if (pendingMigrations?.Any() ?? false)
                dbContext.Database.Migrate();

            GymDbContextSeeding.SeedData(dbContext);

            #endregion

            #region Configure Middleware
            // Configure the middleware pipeline for handling HTTP requests
            if (app.Environment.IsDevelopment())
            {
                // Enable developer-friendly error pages in development environment
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Redirect to custom error page in production
                app.UseExceptionHandler("/Home/Error");
                // Enable HTTP Strict Transport Security (HSTS) for secure connections
                app.UseHsts();
            }

            // Redirect HTTP requests to HTTPS
            app.UseHttpsRedirection();

            // Enable routing for request matching
            app.UseRouting();

            // Enable authorization middleware for securing endpoints
            app.UseAuthorization();

            app.MapStaticAssets();

            // Configure endpoint routing for MVC controllers
            app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                )
                .WithStaticAssets();
            #endregion

            app.Run();
        }
    }
}
