
using Microsoft.Data.SqlClient;
using Dapper;
using ProductApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register repository with DI
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Provide connection string factory
builder.Services.AddTransient<SqlConnection>(_ => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

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
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Minimal repository interfaces/implementations placed in same file for simplicity of scaffold

public record PagedRequest(int Page, int PageSize);

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
}

public class ProductRepository : IProductRepository
{
    private readonly SqlConnection _connection;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(SqlConnection connection, ILogger<ProductRepository> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task<IEnumerable<Product>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        try
        {
            const string proc = "dbo.GetProductsPaged";
            var parameters = new { Page = page, PageSize = pageSize };

            if (_connection.State != System.Data.ConnectionState.Open)
                await _connection.OpenAsync(cancellationToken);

            var items = await _connection.QueryAsync<Product>(new CommandDefinition(proc, parameters, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken));
            return items;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to query products");
            throw;
        }
    }
}
