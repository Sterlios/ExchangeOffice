using CurrencyUpdateService.Models;
using System.Text;
using System.Xml.Linq;

namespace CurrencyUpdateService
{
    public class Worker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public Worker(IServiceProvider serviceProvider, ILogger<Worker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _httpClient = new HttpClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Currency update worker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await UpdateCurrenciesAsync();
                }
                catch (Exception exception)
                {
                    _logger.LogError($"Error updating currencies: {exception.Message}");
                }

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }

        private async Task UpdateCurrenciesAsync()
        {
            _logger.LogInformation("Fetching currency rates...");
            var response = await FetchXmlAsync();

            var currencies = ParseCurrenciesFromXml(response);

            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            foreach (var currency in currencies)
            {
                var existingCurrency = dbContext.Currencies.FirstOrDefault(c => c.Name == currency.Name);

                if (existingCurrency != null)
                    existingCurrency.Rate = currency.Rate;
                else
                    dbContext.Currencies.Add(currency);
            }

            await dbContext.SaveChangesAsync();
            _logger.LogInformation("Currency rates updated successfully.");
        }

        private IEnumerable<Currency> ParseCurrenciesFromXml(string xml)
        {
            var doc = XDocument.Parse(xml);

            return doc.Descendants("Valute").Select(valute => new Currency
            {
                Id = int.Parse(valute.Element("NumCode")?.Value),
                Name = valute.Element("Name")?.Value ?? string.Empty,
                Rate = decimal.Parse(valute.Element("Value")?.Value)
            });
        }

        private async Task<string> FetchXmlAsync()
        {
            var response = await _httpClient.GetAsync("http://www.cbr.ru/scripts/XML_daily.asp");
            response.EnsureSuccessStatusCode();

            var bytes = await response.Content.ReadAsByteArrayAsync();
            var content = Encoding.GetEncoding("windows-1251").GetString(bytes);

            return content;
        }
    }
}
