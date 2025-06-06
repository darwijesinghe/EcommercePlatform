using EP.Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace EP.Inventory.Data.Persistence
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
        public DbSet<InventoryItem> InventoryItem { get; set; }
    }
}
