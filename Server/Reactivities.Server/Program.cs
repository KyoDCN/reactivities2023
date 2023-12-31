using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Reactivities.Server.Core.Extensions;
using Reactivities.Server.Core.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(o =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

    // applies authorization policy to all API controllers
    o.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddCors(x =>
{
    x.AddPolicy("CorsPolicy", x =>
    {
        x.AllowAnyHeader();
        x.AllowAnyMethod();
        x.AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

await app.MigrateEFDatabaseAsync();
await app.SeedTestDataAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // app.UseExceptionHandler("/Error");
    // app.UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
}

// app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
