﻿using ExpenseTracker.Utilities.Constants;
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
        public DateTime ExpenseDate { get; set; }

        /// <summary>
        /// Expense amount.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredError)]
        [Column(TypeName = "decimal(18, 2)")]
        public Decimal Amount { get; set; }

        /// <summary>
        /// Foreign key from the Category Entity.
        /// </summary>
        [ForeignKey("CategoryID")]
        public int CategoryID { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        public virtual Category Category { get; set; }
    }
}