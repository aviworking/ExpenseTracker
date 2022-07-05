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

        /// <summary>
        /// URL: http://localhost:6600/api/expense-tracker/expenses/{key}
        /// </summary>
        /// <param name="key">Primary key of expense entity.</param>
        [HttpGet]
        [Route(RouteConstants.Expenses + "{key}")]
        public async Task<IActionResult> ReadExpenseByKey(int key) 
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest);

                var expense = await context.Expenses.FindAsync(key);

                if (expense == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                return Ok(expense);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}