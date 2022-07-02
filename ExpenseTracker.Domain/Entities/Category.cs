using ExpenseTracker.Utilities.Constants;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Domain.Entities
{
    /// <summary>
    /// Category Entity.
    /// </summary>
    public class Category : BaseModel
    {
        /// <summary>
        /// Primary Key of the Category Entity.
        /// </summary>
        [Key]
        public int CategoryID { get; set; }

        /// <summary>
        /// Name of the expense category.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredError)]
        [StringLength(60)]
        public string CategoryName { get; set; }
    }
}