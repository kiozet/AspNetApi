using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at
// https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<OrderSysContext>();
// opt => opt.UseInMemoryDatabase("OrderList"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
  options.AddSecurityDefinition(
      "oauth2", new OpenApiSecurityScheme { In = ParameterLocation.Header,
                                            Name = "Authorization",
                                            Type = SecuritySchemeType.ApiKey });
  options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<OrderSysContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.MapIdentityApi<IdentityUser>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/orders",
           async (OrderSysContext db) => await db.Orders.ToListAsync());

app.MapGet("/orders/arrived",
           async (OrderSysContext db) => await db.Orders
                                             .Where(order => order.status ==
                                                             "Приехал")
                                             .ToListAsync());

app.MapGet("/orders/{id}",
           async (int id, OrderSysContext db) => await db.Orders.FindAsync(id)
                                                         is Order order
                                                     ? Results.Ok(order)
                                                     : Results.NotFound());

app.MapPost("/orders", async (Order order, OrderSysContext db) => {
  db.Orders.Add(order);
  await db.SaveChangesAsync();

  return Results.Created($"/orders/{order.Id}", order);
});

app.MapPut("/orders/{id}/",
           async (int id, string status, OrderSysContext db) => {
             Order order = await db.Orders.FindAsync(id);
             if (order != null) {
               order.status = status;
               await db.SaveChangesAsync();

               return Results.Ok(order);
             } else {
               return Results.NotFound();
             }
           });
app.Run();
