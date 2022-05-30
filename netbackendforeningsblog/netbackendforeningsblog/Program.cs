using netbackendforeningsblog.DAL;
using Microsoft.EntityFrameworkCore;
using netbackendforeningsblog.Authorization;
using netbackendforeningsblog.Helpers;
using System.Text.Json.Serialization;
using netbackendforeningsblog.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the dependency injection container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ForeningsblogContext>(opt =>
 opt.UseSqlServer("Data Source=DESKTOP-EJMEKGQ;Initial Catalog=Foreningsblog;Integrated Security=True"));


// add services to dependency injection container
{
    var services = builder.Services;
    var env = builder.Environment;

   
    services.AddCors();
    services.AddControllers().AddJsonOptions(x =>
    {
        // serialize enums as strings in api responses (Roles)
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    // configure strongly typed settings object
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

    // configure dependency injection for application services
    services.AddScoped<IJwtUtils, JwtUtils>();
    services.AddScoped<IUserService, UserService>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseAuthorization();
    // global cors policy
    app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
             );
// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();

// custom jwt auth middleware
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

// blueprint how the api/controller gets called
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Get}/{id?}");

app.Run();
