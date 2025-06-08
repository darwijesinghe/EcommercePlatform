using EP.Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace EP.Auth.Data.Persistence
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
        public DbSet<UserAuth> UserAuth { get; set; }
    }
}
