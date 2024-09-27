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
          "Host=db;Port=5432;Username=postgres;Password=123;" +
          "Database=test");
  }
  public DbSet<Order> Orders { get; set; } = null!;
}
