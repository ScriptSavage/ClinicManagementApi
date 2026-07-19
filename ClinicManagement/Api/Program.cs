using Api.Middlewares;
using ApplicationCore.Extensions;
using ApplicationCore.Seeders;
using Scalar.AspNetCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddBusinessLogicLayer(builder.Configuration);
builder.Services.AddScoped<GlobalErrorHandlingMiddleware>();



var app = builder.Build();
await RoleSeeder.SeedRolesAsync(app.Services);
await AdminSeeder.SeedAdminAsync(app.Services, builder.Configuration);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

