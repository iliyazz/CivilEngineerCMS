using CivilEngineerCMS.Data;
using CivilEngineerCMS.Services.Data;
using CivilEngineerCMS.Services.Data.Interfaces;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddDbContext<CivilEngineerCmsDbContext>(option => option.UseSqlServer(connectionString));

//builder.Services.AddTransient<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
//builder.Services.AddApplicationServices(typeof(IProjectService));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CivilEngineerCms", policyBuilder =>
    {
        policyBuilder
            .WithOrigins("https://localhost:7255")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("CivilEngineerCms");

app.Run();
