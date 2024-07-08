using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ObligatorioProgram3.Models;

public class CurrencyService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _apiKey = "68ffa533c801b33066ecdb3209c78b17"; // Coloca tu API Key de CurrencyLayer
    private readonly ObligatorioProgram3Context _context;

    public CurrencyService(IHttpClientFactory httpClientFactory, ObligatorioProgram3Context context)
    {
        _httpClientFactory = httpClientFactory;
        _context = context;
    }

    public async Task<decimal> GetUsdToUyuRateAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetStringAsync($"http://api.currencylayer.com/live?access_key={_apiKey}&currencies=UYU&source=USD");
        var data = JObject.Parse(response);
        var rate = data["quotes"]["USDUYU"].ToObject<decimal>();

        // Registrar la cotización en la base de datos
        var cotizacion = new Cotizacion
        {
            Fecha = DateOnly.FromDateTime(DateTime.Now),
            Moneda = "USD",
            Monto = rate
        };
        _context.Cotizacions.Add(cotizacion);
        await _context.SaveChangesAsync();


        return rate;
    }
}