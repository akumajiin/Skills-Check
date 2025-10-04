# ProductApi - Simple Product API

This is a minimal ASP.NET Core Web API that exposes a single endpoint to fetch products from a SQL Server database.

.NET version: .NET 8.0 (TargetFramework net8.0)

Assumptions:
- A SQL Server instance is available and reachable from the machine running the API.
- The `Products` table exists (see SQL script in `sql/create_products.sql`).
- Connection string is set in `appsettings.json`.

How to run (PowerShell):

```powershell
cd "C:\Users\Tyler\Desktop\Skills Check\ProductApi"
dotnet restore
dotnet run
```

Endpoints:
- GET /api/products - returns JSON array of products

If you don't have a SQL Server handy for quick testing, you can modify `Program.cs` to return mock data or use an in-memory provider.
