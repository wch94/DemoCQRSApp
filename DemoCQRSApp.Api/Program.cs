using Azure.Identity;
using Api;
using Application.Services;
using Application.Validators;
using Domain.Interfaces;
using Infrastructure.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Logging
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://login.microsoftonline.com/5243421f-fa6a-42bb-9456-b62fc4bee840/v2.0";
        options.Audience = "a1edb4c2-a6fe-4e42-9425-73765b057d13";
    });

// Global authorization and FluentValidation
builder.Services.AddFastEndpoints()
                .AddAuthorization();

// Register validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// Response Compression
builder.Services.AddResponseCompression();

bool.TryParse(builder.Configuration["LOCAL_DEVELOPMENT"], out bool isLocalDev);
var connectionString = "Data Source=my-sql-server-wch94.database.windows.net;Initial Catalog=MyDatabase;User ID=wch94_aol.com#EXT#@wch94aol.onmicrosoft.com;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Authentication=ActiveDirectoryInteractive;Application Intent=ReadWrite;Multi Subnet Failover=False";

// Health Checks
if (isLocalDev)
{
    builder.Services.AddHealthChecks().AddSqlServer(connectionString);
}
else
{
    var sqlServer = builder.Configuration["Database:Server"];
    var database = builder.Configuration["Database:Name"];
    connectionString = $"Server={sqlServer};Database={database};";

    builder.Services.AddHealthChecks().AddCheck("AzureSqlManagedIdentity", new AzureSqlManagedIdentityHealthCheck(connectionString));
}

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

// Dependency Injection
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddAutoMapper(typeof(Application.MappingProfiles.ProductProfile).Assembly);

var app = builder.Build();

// Error handling
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "v1/api";
});
app.Run();