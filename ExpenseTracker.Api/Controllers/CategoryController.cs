using ExpenseTracker.Infrastructure;
using ExpenseTracker.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Controllers
{
    /// <summary>
    /// URL: http://localhost:6600/api/expense-tracker/
    /// </summary>
    [Route(RouteConstants.CategoriesController)]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ExpenseTrackerContext context;

        public CategoryController(ExpenseTrackerContext context) 
        { 
            this.context = context; 
        }

        /// <summary>
        /// URL: https://localhost:6600/api/expense-tracker/categories
        /// </summary>
        /// <returns>All categories</returns>
        [HttpGet]
        [Route(RouteConstants.Categories)]
        public async Task<IActionResult> ReadCategories()
        {
            try
            {
                var categories = await context.Categories
                    .AsNoTracking()
                    .OrderBy(c => c.CategoryName)
                    .ToListAsync();
                
                return Ok(categories);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
