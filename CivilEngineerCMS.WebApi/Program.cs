using CivilEngineerCMS.Data;
using CivilEngineerCMS.Data.Models;
using CivilEngineerCMS.Services.Data;
using CivilEngineerCMS.Services.Data.Interfaces;
using CivilEngineerCMS.Web.Infrastructure.ModelBinders;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


// Add services to the container.
builder.Services.AddDbContext<CivilEngineerCmsDbContext>(option => option.UseSqlServer(connectionString));

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IClientService, ClientService>();





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





builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CivilEngineerCmsDev", policyBuilder =>
    {
        policyBuilder
            .WithOrigins("https://localhost:7255")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

//builder.Services.AddControllersWithViews()
//    .AddMvcOptions(options =>
//    {
//        options.ModelBinderProviders.Insert(0, new DecimalModeBinderProvider());
//        options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
//    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.UseCors("CivilEngineerCmsDev");

app.Run();