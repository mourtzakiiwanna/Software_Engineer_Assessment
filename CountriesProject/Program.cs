using Data;
using Microsoft.EntityFrameworkCore;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ArrayService>();
builder.Services.AddScoped<CountryService>();
builder.Services.AddControllers()
    .AddNewtonsoftJson();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache(); 
builder.Services.AddDbContext<CountryDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("CountryDb"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,         
            maxRetryDelay: TimeSpan.FromSeconds(5), 
            errorNumbersToAdd: null 
        )
    )
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
