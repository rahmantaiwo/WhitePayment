using Asp.Versioning;
using WhitePayment.API.Middleware;
using WhitePayment.Application;
using WhitePayment.Application.Common.Options;
using WhitePayment.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Services
// --------------------
builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Application & Infrastructure DI
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Paystack Settings
builder.Services.Configure<PaystackOptions>(
    builder.Configuration.GetSection("Paystack"));


// Logging
builder.Services.AddLogging();

// --------------------
// Build
// --------------------
var app = builder.Build();

// --------------------
// Middleware
// --------------------
//app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
