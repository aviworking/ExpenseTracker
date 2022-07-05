using ExpenseTracker.Infrastructure;
using ExpenseTracker.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Controllers
{
    /// <summary>
    /// URL: http://localhost:6600/api/expense-tracker/
    /// </summary>
    [Route(RouteConstants.ApiController)]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ExpenseTrackerContext context;

        public ExpenseController(ExpenseTrackerContext context) 
        { 
            this.context = context; 
        }

        /// <summary>
        /// URL: http://localhost:6600/api/expense-tracker/expenses/
        /// </summary>
        [HttpGet]
        [Route(RouteConstants.Expenses)]
        public async Task<IActionResult> ReadExpenses() 
        {
            try
            {
                var expenses = await context.Expenses
                    .AsNoTracking()
                    .OrderBy(e => e.ExpenseDate)
                    .ToListAsync();

                return Ok(expenses);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}