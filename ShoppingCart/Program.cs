using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ShoppingCart.Application.Services.CartService).Assembly));

// Register dependencies (DIP)
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartAdder, CartService>();
builder.Services.AddScoped<ICartRemover, CartService>();
builder.Services.AddScoped<ICartReader, CartService>();

// Add DbContext
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
builder.Services.AddDbContext<ShoppingCart.Domain.Repositories.DataContext>(options =>
    options.UseSqlServer(connectionString, b =>
    {
        b.MigrationsAssembly("ShoppingCart.Domain");
        b.MigrationsHistoryTable("__EFMigrationsHistory_Cart");
    }));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Uruchom migracje
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ShoppingCart.Domain.Repositories.DataContext>();
    await dbContext.Database.MigrateAsync();
}

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();
