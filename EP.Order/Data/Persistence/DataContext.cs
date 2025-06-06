using Microsoft.EntityFrameworkCore;

namespace EP.Order.Data.Persistence
{
    /// <summary>
    /// Base db context class.
    /// </summary>
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        // Tables --------------------------------------------
        public DbSet<Models.Order> Order         { get; set; }
        public DbSet<Models.OrderItem> OrderItem { get; set; }
    }
}
