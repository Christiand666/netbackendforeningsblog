using netbackendforeningsblog.DAL;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Microsoft.Extensions.Options;
using WebApi.Authorization;
using WebApi.Helpers;
using WebApi.Models.Users;
using netbackendforeningsblog.Controllers;
using System.Text.Json.Serialization;
using WebApi.Services;
using netbackendforeningsblog.Models;
using BCryptNet = BCrypt.Net.BCrypt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ForeningsblogContext>(opt =>
    opt.UseSqlServer("Data Source=DESKTOP-EJMEKGQ;Initial Catalog=ForeningsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));


// add services to DI container
{
    var services = builder.Services;
    var env = builder.Environment;

   
    services.AddCors();
    services.AddControllers().AddJsonOptions(x =>
    {
        // serialize enums as strings in api responses (e.g. Role)
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    // configure strongly typed settings object
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

    // configure DI for application services
    services.AddScoped<IJwtUtils, JwtUtils>();
    services.AddScoped<IUserService, UserService>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
             );

app.UseMiddleware<ErrorHandlerMiddleware>();

// custom jwt auth middleware
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

//app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Get}/{id?}");

//{
//    var testUsers = new List<User>
//    {
//        new User { Email = "hej",  Password= "1234", FullName = "chris", PasswordHash = BCryptNet.HashPassword("1234"), Role = Role.Admin }
        
//    };

//    using var scope = app.Services.CreateScope();
//    var dataContext = scope.ServiceProvider.GetRequiredService<ForeningsblogContext>();
//    dataContext.Users.AddRange(testUsers);
//    dataContext.SaveChanges();
//}
app.Run();
