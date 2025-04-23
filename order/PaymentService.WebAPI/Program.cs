using Microsoft.EntityFrameworkCore;
using PaymentService.WebAPI.BackgroundServices;
using PaymentService.WebAPI.Data;
using PaymentService.WebAPI.Events.Interface;
using PaymentService.WebAPI.Events.RabbitMQ;
using PaymentService.WebAPI.Repository;
using PaymentService.WebAPI.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")!));

builder.Services.AddScoped<IPaymentsRepository, PaymentsRepository>();

builder.Services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();
builder.Services.AddScoped<PaymentProcessor>();
builder.Services.AddHttpClient();
builder.Services.AddHostedService<OrderCreatedListenerService>();

builder.Services.AddHttpClient();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
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
