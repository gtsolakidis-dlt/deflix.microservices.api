using deflix.subscriptions.api.Interfaces;
using deflix.subscriptions.api.Services;
using deflix.subscriptions.api.Types;
using Microsoft.Extensions.Options;
using Refit;

var corsPolicy = "CorsPolicy";

var apiEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToUpper() ?? "PRODUCTION";

var isDevelopment = apiEnvironment is "DEVELOPMENT" or "DEBUG";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<ISubscriptionsService, SubscriptionsService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(corsPolicy, policy =>
    {
        policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions().Configure<AppSettings>(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (isDevelopment)
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Deflix API V1");
    });
}
else
{
    // Production-specific middleware (e.g., exception handler)
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseCors(corsPolicy);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
