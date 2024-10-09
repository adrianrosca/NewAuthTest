using Microsoft.AspNetCore.Authentication.Negotiate;

var builder = WebApplication.CreateBuilder(args);

// Add Negotiate Authentication and Authorization
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

var app = builder.Build();

// Use HTTPS, Authentication, and Authorization
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Sample secure endpoint
app.MapGet("/secure-data", () => new { Message = "This is secured data" })
    .RequireAuthorization();

app.Run();
