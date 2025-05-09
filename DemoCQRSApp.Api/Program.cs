using Azure.Identity;
using DemoCQRSApp.Application.Services;
using DemoCQRSApp.Domain.Interfaces;
using DemoCQRSApp.Infrastructure.Persistence;
using FastEndpoints;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

bool.TryParse(builder.Configuration["LOCAL_DEVELOPMENT"], out bool isLocalDev);
var connectionString = "Data Source=my-sql-server-wch94.database.windows.net;Initial Catalog=MyDatabase;User ID=wch94_aol.com#EXT#@wch94aol.onmicrosoft.com;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Authentication=ActiveDirectoryInteractive;Application Intent=ReadWrite;Multi Subnet Failover=False";

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    SqlConnection connection;
    if (isLocalDev)
    {
        connection = new SqlConnection(connectionString);
    }
    else
    {
        var credential = new DefaultAzureCredential();
        var token = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/" }));
        connection = new SqlConnection(connectionString) { AccessToken = token.Token };
    }
    options.UseSqlServer(connection);
});

builder.Services.AddFastEndpoints();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductService>();

var app = builder.Build();

app.UseFastEndpoints();

app.Run();
