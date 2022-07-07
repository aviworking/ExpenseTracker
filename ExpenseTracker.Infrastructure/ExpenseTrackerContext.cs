using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure
{
    /// <summary>
    /// Expense tracker API data context. 
    /// </summary>
    public class ExpenseTrackerContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Data Source=localhost; Database=ExpenseTracker; Trusted_Connection=true;";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}