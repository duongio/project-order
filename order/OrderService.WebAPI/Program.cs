using Microsoft.EntityFrameworkCore;
using OrderService.WebAPI.BackgroundServices;
using OrderService.WebAPI.Data;
using OrderService.WebAPI.Events.Interface;
using OrderService.WebAPI.Events.RabbitMQ;
using OrderService.WebAPI.Repository;
using OrderService.WebAPI.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")!));

builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();

builder.Services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();
builder.Services.AddScoped<OrderProcessor>();
builder.Services.AddHostedService<PaymentSucceededListener>();
builder.Services.AddHostedService <PaymentFailedListener>();

builder.Services.AddHttpClient();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
