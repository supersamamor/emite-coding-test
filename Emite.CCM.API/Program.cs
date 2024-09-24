using Emite.Common.API;
using Emite.Common.Web.Utility.Logging;
using Emite.CCM.Application;
using Emite.CCM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using System.Configuration;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration).ReadFrom
                          .Services(services).Enrich
                          .FromLogContext());

// Add services to the container.

var configuration = builder.Configuration;
builder.Services.AddControllers();
builder.Services.AddDefaultApiServices(configuration);
builder.Services.AddApplicationServices();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationContext>();
builder.Services.AddDbContext<IdentityContext>(options
       => options.UseSqlServer(configuration.GetConnectionString("ApplicationContext")));
builder.Services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));
builder.Services.AddMemoryCache();
// Configure Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("SimpleRateLimitPolicy", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = configuration.GetValue<int>("RateLimiter:NumberOfRequest"),
                Window = TimeSpan.FromMinutes(configuration.GetValue<int>("RateLimiter:WindowTimeInMinutes")),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));

    // You can add more policies here
});

if (configuration.GetValue<bool>("UseInMemoryDatabase"))
{
    builder.Services.AddDbContext<ApplicationContext>(options
        => options.UseInMemoryDatabase("ApplicationContext"));
}
else
{
    builder.Services.AddDbContext<ApplicationContext>(options
        => options.UseSqlServer(configuration.GetConnectionString("ApplicationContext")));
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.EnableSwagger();
app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.EnrichDiagnosticContext();
app.MapControllers();
app.MapHealthChecks("/health").AllowAnonymous();

app.Run();
