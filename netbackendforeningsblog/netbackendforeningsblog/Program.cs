using netbackendforeningsblog.DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ForeningsblogContext>(opt =>
    opt.UseSqlServer("Data Source=DESKTOP-EJMEKGQ;Initial Catalog=ForeningsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));

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
            .AllowCredentials()
            .AllowAnyMethod()
            .WithOrigins("https://localhost:7282")
            .AllowAnyHeader());

//app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Get}/{id?}");


app.Run();
