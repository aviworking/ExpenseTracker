using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure;
using ExpenseTracker.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Controllers
{
    /// <summary>
    /// URL: https://localhost:6600/api/expense-tracker/
    /// </summary>
    [Route(RouteConstants.BasePath)]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ExpenseTrackerContext context;

        public CategoryController(ExpenseTrackerContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// URL: https://localhost:6600/api/expense-tracker/categories/
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
        /// <param name="key">Primary key of the category entity.</param>
        [HttpGet]
        [Route(RouteConstants.Categories + "{key}")]
        public async Task<IActionResult> ReadCategoryByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest);

                var category = await context.Categories.FindAsync(key);

                if (category == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                return Ok(category);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// URL: https://localhost:6600/api/expense-tracker/categories/create
        /// </summary>
        /// <param name="category">Category object.</param>
        [HttpPost]
        [Route(RouteConstants.CreateCategory)]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest);

                if (await IsCategoryDuplicate(category))
                    return StatusCode(StatusCodes.Status400BadRequest);

                context.Categories.Add(category);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadCategoryByKey", new { key = category.CategoryID }, category);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// URL: https://localhost:6600/api/expense-tracker/categories/update
        /// </summary>
        /// <param name="id">Primary key of the category entity.</param>
        /// <param name="category">Category object.</param>
        [HttpPut]
        [Route(RouteConstants.UpdateCategory)]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            try
            {
                if (id != category.CategoryID)
                    return StatusCode(StatusCodes.Status400BadRequest);

                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest);

                if (await IsCategoryDuplicate(category))
                    return StatusCode(StatusCodes.Status400BadRequest);

                if (!await IsCategoryExistant(id))
                    return StatusCode(StatusCodes.Status404NotFound);

                context.Entry(category).State = EntityState.Modified;
                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// URL: https://localhost:6600/api/expense-tracker/categories/delete/{key}
        /// </summary>
        /// <param name="key">Primary key of the category entity.</param>
        [HttpDelete]
        [Route(RouteConstants.DeleteCategory + "{key}")]
        public async Task<IActionResult> DeleteCategory(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest);

                var category = await context.Categories.FindAsync(key);

                if (category == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                if (await IsCategoryInUse(category))
                    return StatusCode(StatusCodes.Status400BadRequest);

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Verifying whether the category name is a duplicate or not.
        /// </summary>
        /// <param name="category">Category object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsCategoryDuplicate(Category category)
        {
            try
            {
                var categoryInDb = await context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == category.CategoryName.ToLower());

                if (categoryInDb != null)
                    return true;

                return false;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Verifying whether the category exists or not.
        /// </summary>
        /// <param name="id">Primary key of the category entity.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsCategoryExistant(int id)
        {
            try
            {
                var categoryInDb = await context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CategoryID == id);

                if (categoryInDb != null)
                    return true;

                return false;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Verifying if the category is in use or not.
        /// </summary>
        /// <param name="category">Category object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsCategoryInUse(Category category)
        {
            try
            {
                var expense = await context.Expenses
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.CategoryID == category.CategoryID);

                if (expense != null)
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