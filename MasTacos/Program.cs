using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// === Add Services ===

// Add CORS with named policy (recommended for maintainability)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DB context
builder.Services.AddDbContext<MasTacos.Data.MasTacosContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ),
    ServiceLifetime.Scoped
);

// Register repositories
builder.Services.AddScoped<MasTacos.Repository.Interfaces.ICustomerRepository, MasTacos.Repository.CustomerRepository>();
builder.Services.AddScoped<MasTacos.Repository.Interfaces.IMenuItemRepository, MasTacos.Repository.MenuItemRepository>();
// builder.Services.AddScoped<MasTacos.Repository.Interfaces.IOrderRepository, MasTacos.Repository.OrderRepository>();
// builder.Services.AddScoped<MasTacos.Repository.Interfaces.IReservationRepository, MasTacos.Repository.ReservationRepository>();
// builder.Services.AddScoped<MasTacos.Repository.Interfaces.ISurveyRepository, MasTacos.Repository.SurveyRepository>();
// builder.Services.AddScoped<MasTacos.Repository.Interfaces.ITimeSlotRepository, MasTacos.Repository.TimeSlotRepository>();

var app = builder.Build();

// === Configure Middleware ===

// Enable CORS globally (before Authorization)
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    // Disable caching of Swagger UI in dev
    app.Use(async (context, next) =>
    {
        context.Response.Headers["Cache-Control"] = "no-store";
        await next();
    });

    app.UseSwagger();
    app.UseSwaggerUI();
}

// TODO: Only use HTTPS redirection in non-Docker, production environments
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
