using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DB context
builder.Services.AddDbContext<MasTacos.Data.MasTacosContext>();

// Register repositories and services
builder.Services.AddScoped<MasTacos.Repository.Interfaces.ICustomerRepository, MasTacos.Repository.CustomerRepository>();
//builder.Services.AddScoped<MasTacos.Repository.Interfaces.IMenuItemRepository, MasTacos.Repository.MenuItemRepository>();
//builder.Services.AddScoped<MasTacos.Repository.Interfaces.IOrderRepository, MasTacos.Repository.OrderRepository>();
//builder.Services.AddScoped<MasTacos.Repository.Interfaces.IReservationRepository, MasTacos.Repository.ReservationRepository>();
//builder.Services.AddScoped<MasTacos.Repository.Interfaces.ISurveyRepository, MasTacos.Repository.SurveyRepository>();
//builder.Services.AddScoped<MasTacos.Repository.Interfaces.ITimeSlotRepository, MasTacos.Repository.TimeSlotRepository>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //force no-cache on swagger ui during development
    app.Use(async (context, next) =>
    {
        context.Response.Headers["Cache-Control"] = "no-store";
        await next();
    });

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
