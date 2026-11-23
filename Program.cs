using Microsoft.EntityFrameworkCore;
using wt_lab8_8razor_postgresql_rabtsevich.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Укажите строку подключения ConnectionStrings:DefaultConnection.");
}

builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStatusCodePagesWithReExecute("/Error", "?code={0}");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

var port = Environment.GetEnvironmentVariable("PORT") ?? "10001";
app.Run($"http://0.0.0.0:{port}");