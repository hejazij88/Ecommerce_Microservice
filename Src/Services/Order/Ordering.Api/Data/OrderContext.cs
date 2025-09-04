using Microsoft.EntityFrameworkCore;
using Ordering.Api.Entity;

namespace Ordering.Api.Data;

public class OrderContext: DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }

    public DbSet<Order> Orders { get; set; }
}