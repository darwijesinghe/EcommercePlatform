using Microsoft.EntityFrameworkCore;

namespace EP.Product.Data.Persistence
{
    /// <summary>
    /// Base db context class.
    /// </summary>
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        // Tables -----------------------------------------
        public DbSet<Models.Product> Products { get; set; }
    }
}
