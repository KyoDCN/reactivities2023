using Reactivities.Server.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.MigrateEFDatabase();
app.SeedTestData();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // app.UseExceptionHandler("/Error");
    // app.UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
}

// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors();

app.Run();
