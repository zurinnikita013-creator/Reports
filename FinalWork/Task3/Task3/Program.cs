using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Task3ProductDto;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(@"Server=.;Database=ProductDB;Trusted_Connection=True;TrustServerCertificate=True;"));

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
