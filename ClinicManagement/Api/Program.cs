using Api.Middlewares;
using ApplicationCore.Extensions;
using ApplicationCore.Seeders;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddBusinessLogicLayer(builder.Configuration);
builder.Services.AddScoped<GlobalErrorHandlingMiddleware>();

builder.Host.UseSerilog((context, conf) =>
{
    conf.ReadFrom.Configuration(context.Configuration);
});


var app = builder.Build();
await RoleSeeder.SeedRolesAsync(app.Services);
await AdminSeeder.SeedAdminAsync(app.Services, builder.Configuration);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseSerilogRequestLogging();


app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

