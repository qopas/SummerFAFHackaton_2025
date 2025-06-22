using Microsoft.OpenApi.Models;
using Ediki.Application;
using Ediki.Infrastructure;
using Serilog;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Add Application and Infrastructure layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Make Swagger conditional for Railway
var enableSwagger = builder.Configuration.GetValue<bool>("ApiSettings:EnableSwagger", 
    builder.Environment.IsDevelopment());

if (enableSwagger)
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Ediki API",
            Version = "v1",
            Description = "API for Ediki Team Management System",
            Contact = new OpenApiContact
            {
                Name = "Ediki Team",
                Email = "support@ediki.com"
            }
        });

        // Include XML comments
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
        }

        // Add JWT Bearer authentication
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                []
            }
        });
    });
}

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure port for Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://*:{port}");

var app = builder.Build();

// Initialize database
try
{
    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.InitializeDatabaseAsync();
    Log.Information("Database initialization completed successfully");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An error occurred while initializing the database");
    if (app.Environment.IsDevelopment())
    {
        throw;
    }
    Log.Warning("Continuing startup despite database initialization failure");
}

// Configure the HTTP request pipeline
app.UseCors("AllowAll");

if (enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ediki API V1");
        // Don't set as root in production
        c.RoutePrefix = app.Environment.IsDevelopment() ? string.Empty : "swagger";
    });
}

// Only use HTTPS redirect in production with proper certificate
if (!app.Environment.IsDevelopment())
{
    // Railway handles HTTPS termination, so we don't need this
    // app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health check for Railway
app.MapGet("/health", () => new
{
    status = "healthy",
    timestamp = DateTime.UtcNow,
    version = "1.0.0",
    environment = app.Environment.EnvironmentName
});

// Add a simple status endpoint
app.MapGet("/", () => new
{
    service = "Ediki API",
    version = "1.0.0",
    status = "Running",
    timestamp = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName,
    endpoints = new
    {
        health = "/health",
        version = "/version",
        api = "/api/v1",
        swagger = enableSwagger ? "/swagger" : null
    }
});

// Add version endpoint
app.MapGet("/version", () => new
{
    version = "1.0.0",
    buildDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
    commit = Environment.GetEnvironmentVariable("GIT_COMMIT") ?? 
             Environment.GetEnvironmentVariable("RAILWAY_GIT_COMMIT_SHA")?.Substring(0, 7) ?? "local",
    environment = app.Environment.EnvironmentName,
    port = port
});

try
{
    Log.Information("Starting Ediki API on {Environment} at port {Port}", 
        app.Environment.EnvironmentName, port);
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}