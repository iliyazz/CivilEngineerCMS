using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Unicode;

using CivilEngineerCMS.Data;
using CivilEngineerCMS.Data.Models;
using CivilEngineerCMS.Services.Data;
using CivilEngineerCMS.Services.Data.Interfaces;
using CivilEngineerCMS.Web;
using CivilEngineerCMS.Web.Localization;
using CivilEngineerCMS.Web.Infrastructure.Extensions;
using CivilEngineerCMS.Web.Infrastructure.Localization;
using CivilEngineerCMS.Web.Infrastructure.Middlewares;
using CivilEngineerCMS.Web.Infrastructure.ModelBinders;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.WebEncoders;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

using static CivilEngineerCMS.Common.GeneralApplicationConstants;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

Account cloudinaryCredentials = new Account(
    builder.Configuration.GetValue<string>("Cloudinary:CloudName"),
    builder.Configuration.GetValue<string>("Cloudinary:ApiKey"),
    builder.Configuration.GetValue<string>("Cloudinary:ApiSecret"));
Cloudinary cloudinary = new Cloudinary(cloudinaryCredentials);
builder.Services.AddSingleton(cloudinary);
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();


builder.Services.AddDbContext<CivilEngineerCmsDbContext>(options =>
    options.UseSqlite(connectionString));

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
    .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
    .AddEntityFrameworkStores<CivilEngineerCmsDbContext>();

//builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddApplicationServices(typeof(IHomeService));

builder.Services.AddRecaptchaService();

builder.Services.AddMemoryCache();

builder.Services.ConfigureApplicationCookie(config => 
{
    config.LoginPath = "/User/Login"; 
    config.AccessDeniedPath = "/Home/Error/401";
});

// Localization: Bulgarian is the default language, English is available through the language switcher.
// Texts are keyed by their English wording in Resources/SharedResource.bg.resx.
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    CultureInfo[] supportedCultures = SupportedCultures.Select(c => new CultureInfo(c)).ToArray();
    options.DefaultRequestCulture = new RequestCulture(DefaultCulture);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    // Only an explicit choice (?culture=en or the language cookie) changes the language,
    // so visitors get the Bulgarian site regardless of their browser settings.
    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    };
});

// Render Cyrillic as-is instead of HTML-encoding every character.
builder.Services.Configure<WebEncoderOptions>(options =>
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

builder.Services.AddControllersWithViews()
    .AddMvcOptions(options =>
    {
        options.ModelBinderProviders.Insert(0, new DecimalModeBinderProvider());
        options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
        options.ModelMetadataDetailsProviders.Add(new DefaultValidationMessagesMetadataProvider());
    })
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
        options.DataAnnotationLocalizerProvider = (_, factory) => factory.Create(typeof(SharedResource)));

// Localize the model binding messages (e.g. "The value 'x' is not valid for Amount.").
builder.Services.AddOptions<MvcOptions>()
    .Configure<IStringLocalizerFactory>((options, factory) =>
    {
        IStringLocalizer localizer = factory.Create(typeof(SharedResource));
        var messages = options.ModelBindingMessageProvider;
        messages.SetAttemptedValueIsInvalidAccessor((value, field) => localizer["The value '{0}' is not valid for {1}.", value, field]);
        messages.SetMissingBindRequiredValueAccessor(field => localizer["A value for the '{0}' parameter or property was not provided.", field]);
        messages.SetMissingKeyOrValueAccessor(() => localizer["A value is required."]);
        messages.SetMissingRequestBodyRequiredValueAccessor(() => localizer["A non-empty request body is required."]);
        messages.SetNonPropertyAttemptedValueIsInvalidAccessor(value => localizer["The value '{0}' is not valid.", value]);
        messages.SetNonPropertyUnknownValueIsInvalidAccessor(() => localizer["The supplied value is invalid."]);
        messages.SetNonPropertyValueMustBeANumberAccessor(() => localizer["The field must be a number."]);
        messages.SetUnknownValueIsInvalidAccessor(field => localizer["The supplied value is invalid for {0}.", field]);
        messages.SetValueIsInvalidAccessor(value => localizer["The value '{0}' is invalid.", value]);
        messages.SetValueMustBeANumberAccessor(field => localizer["The field {0} must be a number.", field]);
        messages.SetValueMustNotBeNullAccessor(value => localizer["The value '{0}' is invalid.", value]);
    });


WebApplication app = builder.Build();

// Create the SQLite database (or apply pending migrations) on startup,
// so the app runs without a committed .db file.
using (IServiceScope scope = app.Services.CreateScope())
{
    CivilEngineerCmsDbContext dbContext = scope.ServiceProvider.GetRequiredService<CivilEngineerCmsDbContext>();
    dbContext.Database.Migrate();
}

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

app.UseRequestLocalization();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.EnableOnlineUsersCheck();

if (app.Environment.IsDevelopment())
{
    app.SeedAdministrator(DevelopmentAdminEmail);
}

app.MapControllerRoute(
    name: "areaRoute",
    pattern: "/{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "/{controller=Home}/{action=Index}/{id?}");

app.MapDefaultControllerRoute();

app.MapRazorPages();



app.Run();