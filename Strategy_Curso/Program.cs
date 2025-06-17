using Strategy_Curso.Context;
using Strategy_Curso.Models;
using Strategy_Curso.Strategy;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache(); // Para armazenar os dados da sessão em memória do servidor
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    
});

// MySQLStrategy precisa da MySqlConnection da configuração
builder.Services.AddTransient<MySQLStrategy>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    string connectionString = configuration.GetConnectionString("MySqlConnection");
    return new MySQLStrategy(connectionString);
});

// SQLServerStrategy precisa da SqlServerConnection da configuração
builder.Services.AddTransient<SQLServerStrategy>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    string connectionString = configuration.GetConnectionString("SqlServerConnection");
    return new SQLServerStrategy(connectionString);
});


builder.Services.AddScoped<IDataBaseStrategy>(provider => // AQUI: Registrando a INTERFACE
{
    var httpContextAccessor = provider.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
    var session = httpContextAccessor.HttpContext?.Session;

    string dbType = session?.GetString("DbType") ?? "MySQL"; // Pega da sessão, default para MySQL
    Console.WriteLine($"[Program.cs Factory DEBUG] Valor da sessão LIDO: {dbType}");
    // Retorna a instância da estratégia CONCRETA que o DI já sabe como criar
    if (dbType == "SQLServer")
    {
        Console.WriteLine("[Program.cs - IDataBaseStrategy Factory] Retornando SQLServerStrategy.");
        return provider.GetRequiredService<SQLServerStrategy>();
    }
    else // Default ou MySQL
    {
        Console.WriteLine("[Program.cs - IDataBaseStrategy Factory] Retornando MySQLStrategy.");
        return provider.GetRequiredService<MySQLStrategy>();
    }
});

builder.Services.AddScoped<CursoCRUDContext>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cursos}/{action=Index}/{id?}");

app.Run();
