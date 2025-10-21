using IFICamarAPI.Application.IoC;
using IFICamarAPI.Insfrastructure.Data;
using IFICamarAPI.Insfrastructure.IoC;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add HttpContextAccessor service
builder.Services.AddHttpContextAccessor();

// Use the configuration from the builder
IConfiguration Configuration = builder.Configuration;

// Configure MySQL Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        mysqlOptions =>
        {
            // Add retry on failure for MySQL
            mysqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);

            // Enable detailed logging (optional, remove in production)
            mysqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        }
    )
    // Optional: Enable sensitive data logging for debugging
    .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
    .EnableDetailedErrors(builder.Environment.IsDevelopment());
});

// Register custom services
builder.Services.AddInfrastructure(Configuration);
builder.Services.AddApplication();

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
