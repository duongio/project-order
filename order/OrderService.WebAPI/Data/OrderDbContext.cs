using Microsoft.EntityFrameworkCore;
using OrderService.WebAPI.Models;

namespace OrderService.WebAPI.Data
{
    public class OrderDbContext: DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<OrderEvent> OderEvents { get; set; } = null!;

    }
}
