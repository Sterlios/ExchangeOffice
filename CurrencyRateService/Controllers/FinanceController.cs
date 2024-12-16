using CurrencyRateService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateService.Controllers
{
    [ApiController]
    public class FinanceController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public FinanceController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("currencies")]
        public async Task<IActionResult> Index()
        {
            return Ok(_dbContext.Currencies.ToList());
        }

        [HttpGet("user/{userId}/currencies")]
        public async Task<IActionResult> GetUserCurrencies(int userId)
        {
            var userCurrencies = _dbContext.UserCurrencies
                .Where(userCurrency => userCurrency.UserId == userId);

            if (userCurrencies == null)
                return NotFound(new { message = "User has not currencies" });

            var currencies = userCurrencies.Select(userCurrency => new CurrencyDTO
            {
                Name = userCurrency.Currency.Name,
                Rate = userCurrency.Currency.Rate
            }).ToList();

            return Ok(currencies);
        }

        [HttpPost("user/{userId}/currencies/add/{currencyId}")]
        public async Task<IActionResult> AddUserCurrency(int userId, int currencyId)
        {
            var currency = await _dbContext.Currencies
                .Include(currency => currency.UserCurrencies)
                .FirstOrDefaultAsync(currency => currency.Id == currencyId);

            if (currency == null)
                return NotFound(new { message = "Currency not found" });

            var foundUserCurrency = await _dbContext.UserCurrencies
                .FirstOrDefaultAsync(userCurrency => userCurrency.CurrencyId == currencyId && userCurrency.UserId == userId);

            if (foundUserCurrency != null)
                return Conflict(new { message = "Currency has already been added earlier" });

            await _dbContext.UserCurrencies.AddAsync(new UserCurrency
            {
                UserId = userId,
                CurrencyId = currencyId,
            });

            await _dbContext.SaveChangesAsync();

            return Ok(new { Message = "Currency added successfully.", CurrencyId = currencyId });
        }

        [HttpDelete("user/{userId}/currencies/delete/{currencyId}")]
        public async Task<IActionResult> DeleteUserCurrency(int userId, int currencyId)
        {
            var currency = await _dbContext.Currencies
                .Include(currency => currency.UserCurrencies)
                .FirstOrDefaultAsync(currency => currency.Id == currencyId);

            if (currency == null)
                return NotFound(new { message = "Currency not found" });

            var foundUserCurrency = await _dbContext.UserCurrencies
                .FirstOrDefaultAsync(userCurrency => userCurrency.CurrencyId == currencyId && userCurrency.UserId == userId);

            if (foundUserCurrency == null)
                return NotFound(new { message = "The user does not have this currency among his favorites." });

            _dbContext.UserCurrencies.Remove(foundUserCurrency);
            await _dbContext.SaveChangesAsync();

            return Ok(new { Message = "Currency deleted successfully.", CurrencyId = currencyId });
        }
    }
}
