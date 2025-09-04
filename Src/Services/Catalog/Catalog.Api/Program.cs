using Catalog.Applications.Repository;
using Catalog.Domain.Entity;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Settings;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using static Catalog.Api.Dtos.Product_DTO;

var builder = WebApplication.CreateBuilder(args);

// Serilog Config
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Seq("http://seq:5341") // 👈 لاگ‌ها می‌رن تو Seq
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("Mongo"));
builder.Services.AddSingleton<MongoContext>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();


builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();




builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("Mongo"));

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyService"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddJaegerExporter(options =>
            {
                options.AgentHost = builder.Configuration["Jaeger:Host"] ?? "localhost";
                options.AgentPort = int.Parse(builder.Configuration["Jaeger:Port"] ?? "6831");
            });
    });

var config = TypeAdapterConfig.GlobalSettings;

config.NewConfig<Product, ProductDto>();
config.NewConfig<CreateProductDto, Product>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
