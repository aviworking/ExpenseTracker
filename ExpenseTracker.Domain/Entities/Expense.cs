using ExpenseTracker.Utilities.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Domain.Entities
{
    /// <summary>
    /// Expense Entity.
    /// </summary>
    public class Expense : BaseModel
    {
        /// <summary>
        /// Primary Key of the Expense Entity.
        /// </summary>
        [Key]
        public int ExpenseID { get; set; }

        /// <summary>
        /// Date of expenditure.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredError)]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Expense Date")]
        public DateTime ExpenseDate { get; set; }

        /// <summary>
        /// Expense amount.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredError)]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Navigation properties.
        /// </summary>
        [ForeignKey("CategoryID")]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
    }
}