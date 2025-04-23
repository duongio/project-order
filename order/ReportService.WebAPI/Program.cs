using Microsoft.EntityFrameworkCore;
using ReportService.WebAPI.BackgroundServices;
using ReportService.WebAPI.Data;
using ReportService.WebAPI.Repository;
using ReportService.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ReportDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")!));

builder.Services.AddScoped<IReportRepository, ReportRepository>();

builder.Services.AddScoped<ReportProcessor>();
builder.Services.AddHostedService<CreateOrderSucceededListener>();
builder.Services.AddHostedService<CreatePaymentSucceededListener>();

builder.Services.AddHttpClient();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ReportDbContext>();
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
