using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Domain.Entities
{
    /// <summary>
    /// BaseModel Entity.
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// Date created.
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? DateCreated { get; set; }

        /// <summary>
        /// Date modified.
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? DateModified { get; set; }
    }
}