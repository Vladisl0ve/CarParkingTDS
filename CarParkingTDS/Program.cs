using CarParkingTDS.Models.DbContexts;
using CarParkingTDS.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ParkingService>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddDbContext<ParkingContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ParkingDatabase"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();