using CivilEngineerCMS.Data;
using CivilEngineerCMS.Data.Models;
using CivilEngineerCMS.Services.Data.Interfaces;
using CivilEngineerCMS.Web.Infrastructure.Extensions;
using CivilEngineerCMS.Web.Infrastructure.ModelBinders;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using static CivilEngineerCMS.Common.GeneralApplicationConstants;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CivilEngineerCmsDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount =
            builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
        options.Password.RequireDigit = builder.Configuration.GetValue<bool>("Identity:Password:RequireDigit");
        options.Password.RequireLowercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
        options.Password.RequireNonAlphanumeric =
            builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
        options.Password.RequireUppercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
        options.Password.RequiredLength = builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
        options.Lockout.AllowedForNewUsers =
            builder.Configuration.GetValue<bool>("Identity:Lockout:AllowedForNewUsers");
        options.Lockout.DefaultLockoutTimeSpan =
            TimeSpan.FromMinutes(builder.Configuration.GetValue<int>("Identity:Lockout:DefaultLockoutTimeSpan"));
        options.Lockout.MaxFailedAccessAttempts =
            builder.Configuration.GetValue<int>("Identity:Lockout:MaxFailedAccessAttempts");
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<CivilEngineerCmsDbContext>();

//builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddApplicationServices(typeof(IHomeService));

builder.Services.AddRecaptchaService();

builder.Services.ConfigureApplicationCookie(config => 
{
    config.LoginPath = "/User/Login"; 
    config.AccessDeniedPath = "/Home/Error/401";
});

builder.Services.AddControllersWithViews()
    .AddMvcOptions(options =>
    {
        options.ModelBinderProviders.Insert(0, new DecimalModeBinderProvider());
        options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
    });


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error/500");
    app.UseStatusCodePagesWithRedirects("/Home/Error?StatusCode={0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.SeedAdministrator(DevelopmentAdminEmail);
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
               name: "areaRoute",
                      pattern: "/{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
               name: "default",
                      pattern: "/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapDefaultControllerRoute();

    endpoints.MapRazorPages();
});



app.Run();