﻿using Microsoft.EntityFrameworkCore;
using PaymentService.WebAPI.Models;

namespace PaymentService.WebAPI.Data
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) { }

        public DbSet<Payment> Payments { get; set; } = null!;
    }
}
