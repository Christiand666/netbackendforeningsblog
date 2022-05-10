using netbackendforeningsblog.DAL;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Microsoft.Extensions.Options;
using netbackendforeningsblog.Authorization;
using netbackendforeningsblog.Helpers;
using netbackendforeningsblog.Models.Users;
using netbackendforeningsblog.Controllers;
using System.Text.Json.Serialization;
using netbackendforeningsblog.Services;
using netbackendforeningsblog.Models;
using BCryptNet = BCrypt.Net.BCrypt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ForeningsblogContext>(opt =>
    opt.UseSqlServer("Data Source=mssql1.unoeuro.com;Initial Catalog=thomasblok_dk_db_softwareudvikling;Persist Security Info=True;User ID=thomasblok_dk;Password=Ea2Rrpz5GDmF"));


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


//app.UseHttpsRedirection();

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
//        new User { Email = "vk",  Password= "123", FullName = "chrisser", PasswordHash = BCryptNet.HashPassword("123"), Role = Role.User }

//    };

//    using var scope = app.Services.CreateScope();
//    var dataContext = scope.ServiceProvider.GetRequiredService<ForeningsblogContext>();
//    dataContext.Users.AddRange(testUsers);
//    dataContext.SaveChanges();
//}
app.Run();
