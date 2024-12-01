using Data;
using Microsoft.EntityFrameworkCore;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache(); // Register MemoryCache

builder.Services.AddDbContext<CountryDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("CountryDb"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,          // Number of retries 
            maxRetryDelay: TimeSpan.FromSeconds(5),  // Delay between retries
            errorNumbersToAdd: null    // You can specify specific SQL error numbers if needed
        )
    )
);
builder.Services.AddScoped<CountryService>(); // Add the CountryService as a scoped service
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
