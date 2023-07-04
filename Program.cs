using dotnet_api_test.Exceptions;
using dotnet_api_test.Persistence;
using dotnet_api_test.Persistence.Repositories;
using dotnet_api_test.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(opt => opt.Filters.Add(new ExceptionFilter()));
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "dotnet_api_test", Version = "v1"}); });
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("DishDb"));
builder.Services.AddScoped<IDishRepository, DishRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "dotnet_api_test v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Seeding db
Seed.PopulateDb(app);
app.Run();