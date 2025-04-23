using Microsoft.EntityFrameworkCore;
using ReportService.WebAPI.Models;

namespace ReportService.WebAPI.Data
{
    public class ReportDbContext : DbContext
    {
        public ReportDbContext(DbContextOptions<ReportDbContext> options) : base(options) { }

        public DbSet<OrderReport> OrdersReport { get; set; } = null!;
        public DbSet<OrderItemReport> OrderItemsReport { get; set; } = null!;
        public DbSet<PaymentReport> PaymentReports { get; set; } = null!; 
    }
}
