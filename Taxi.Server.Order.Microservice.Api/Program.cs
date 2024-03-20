using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.SignalR;
using Taxi.Server.Order.Microdervice.Application.Services;
using Taxi.Server.Order.Microdervice.Domain.Interfaces.Services;
using Taxi.Server.Order.Microservice.Api.Configuration;
using Taxi.Server.Order.Microservice.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

builder.Services.AddAuthenticateConfiguration(builder);

builder.Services.AddScoped<IPriceService, PriceService>();

builder.Services.AddSingleton<ITrackerService, TrackerService>();
builder.Services.AddSingleton<IOrderService, OrderService>();

builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
        .WithOrigins("http://localhost:3000")
        .WithOrigins("http://localhost:3001")
        .WithOrigins("http://localhost:3002")
        .WithOrigins("http://localhost:7255")
        .AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.Use(async (context, next) =>
{
    var token = context.Request.Cookies[".AspNetCore.Application.Id"];
    if (!string.IsNullOrEmpty(token))
        context.Request.Headers.Add("Authorization", "Bearer " + token);

    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<OrderHub>("/orderHub");

app.Run();



