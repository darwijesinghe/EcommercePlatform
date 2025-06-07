using Microsoft.EntityFrameworkCore;
namespace EP.User.Data.Persistence
{
    /// <summary>
    /// Base db context class.
    /// </summary>
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        // Tables -----------------------------------
        public DbSet<Models.User> Users { get; set; }
    }
}
