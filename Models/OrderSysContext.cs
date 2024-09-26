using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class OrderSysContext : IdentityDbContext {
  protected readonly IConfiguration Configuration;
  public OrderSysContext(IConfiguration configuration) {
    Configuration = configuration;
  }

  protected override void
  OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    if (!optionsBuilder.IsConfigured)
      optionsBuilder.UseNpgsql(
          "Host=localhost;Port=5432;Username=postgres;Password=1q9o8i2w;" +
          "Database=test");
  }
  public DbSet<Order> Orders { get; set; } = null!;
}
