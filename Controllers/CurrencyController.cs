using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class CurrencyController : Controller
{
    private readonly CurrencyService _currencyService;

    public CurrencyController(CurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    [HttpGet]
    [Route("api/currency/usd-to-uyu")]
    public async Task<IActionResult> GetUsdToUyuRate()
    {
        var rate = await _currencyService.GetUsdToUyuRateAsync();
        return Ok(new { rate });
    }
}