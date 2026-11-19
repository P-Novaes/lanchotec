using System.Globalization;
using Microsoft.AspNetCore.Localization;
using TechBurguer.DesignPatterns.Iterator;
using TechBurguer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// O catálogo (coleção do Iterator) e o serviço de pedidos vivem durante toda a aplicação:
// assim o cardápio é montado uma única vez e a fila de pedidos não se perde entre requisições.
builder.Services.AddSingleton<ICatalogo, Catalogo>();
builder.Services.AddSingleton<PedidoService>();

var app = builder.Build();

// Moeda e números em português do Brasil (R$ 18,00), independente do idioma do computador.
var ptBr = new CultureInfo("pt-BR");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(ptBr),
    SupportedCultures = new List<CultureInfo> { ptBr },
    SupportedUICultures = new List<CultureInfo> { ptBr }
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
