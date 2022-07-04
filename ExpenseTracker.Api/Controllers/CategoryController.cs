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

        /// <summary>
        /// URL: https://localhost:6600/api/expense-tracker/categories/{key}
        /// </summary>
        /// <param name="key">Primary key of the entity.</param>
        [HttpGet]
        [Route(RouteConstants.CategoriesByKey + "{key}")]
        public async Task<IActionResult> ReadCategoryByKey(int key) 
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest);

                var category = await context.Categories.FindAsync(key);

                if(category == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                return Ok(category);

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
