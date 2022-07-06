using ExpenseTracker.Domain.Entities;
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

        /// <summary>
        /// URL: http://localhost:6600/api/expense-tracker/expenses/create
        /// </summary>
        /// <param name="expense">Expense object.</param>>
        [HttpPost]
        [Route(RouteConstants.CreateExpense)]
        public async Task<IActionResult> CreateExpense(Expense expense)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest);

                if (IsNotValidExpense(expense))
                    return StatusCode(StatusCodes.Status400BadRequest);

                context.Expenses.Add(expense);
                await context.SaveChangesAsync();
                return CreatedAtAction("ReadExpenseByKey", new { key = expense.CategoryID }, expense);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route(RouteConstants.UpdateExpense)]
        public async Task<IActionResult> UpdateExpense(int id, Expense expense)
        {
            try
            {
                if(id != expense.ExpenseID)
                    return StatusCode(StatusCodes.Status400BadRequest);

                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest);

                if (IsNotValidExpense(expense))
                    return StatusCode(StatusCodes.Status400BadRequest);

                if (await IsExpenseUnexistant(expense))
                    return StatusCode(StatusCodes.Status404NotFound);

                context.Entry(expense).State = EntityState.Modified;
                context.Expenses.Update(expense);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Verifying whether the expense date is a future date or if the amount is less than or equal to zero.
        /// </summary>
        /// <param name="expense">Expense object.</param>
        /// <returns>Boolean</returns>
        private static bool IsNotValidExpense(Expense expense) 
        {
            if (expense.ExpenseDate > DateTime.Now || expense.Amount <= 0)
                return true;

            return false;
        }

        /// <summary>
        /// Verifying whether the expense is existant or not.
        /// </summary>
        /// <param name="expense">Expense object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsExpenseUnexistant(Expense expense)
        {
            try
            {
                var expenseInDb = await context.Expenses
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.ExpenseID == expense.ExpenseID);

                if(expenseInDb == null)
                    return true;

                return false;
            }
            catch
            {
                throw;
            }
        }
    }
}