using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Domain.Entities
{
    public class BaseModel
    {
        [Column(TypeName = "smalldatetime")]
        public DateTime? DateCreated { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DateModified { get; set; }
    }
}