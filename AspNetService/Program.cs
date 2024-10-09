using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using WcfCoreService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from appsettings.json
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

int port = configuration.GetValue<int>("ServicePort");

if (port == 0)
{
    Console.WriteLine("Error: ServicePort is missing or set to 0 in appsettings.json.");
    Environment.Exit(1);
}

Console.WriteLine($"ASP.NET Core Service starting on port: {port}");

// Set up Kestrel and URLs
builder.WebHost.UseKestrel().UseUrls($"http://localhost:{port}");

// Add CoreWCF and metadata services
builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();

// Configure logging for diagnostics
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

var app = builder.Build();

// Diagnostic endpoint to verify ASP.NET Core is running on the expected port
app.MapGet("/test", () => $"Service is running on port {port}");

// Configure WCF service with metadata publishing
app.UseServiceModel(serviceBuilder =>
{
    // Set up WCF service endpoint
    serviceBuilder.AddService<WcfService>(serviceOptions =>
    {
        serviceOptions.BaseAddresses.Add(new Uri($"http://localhost:{port}"));
    });

    // Add the BasicHttpBinding endpoint for the WCF service
    serviceBuilder.AddServiceEndpoint<WcfService, IWcfService>(new BasicHttpBinding(), $"/BasicHttp");

    // Enable HTTP metadata publishing
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpGetEnabled = true;
});

app.Run();
