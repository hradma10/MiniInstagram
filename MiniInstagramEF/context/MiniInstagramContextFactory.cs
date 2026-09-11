
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MiniInstagramEF.context
{
    public class MiniInstagramContextFactory : IDesignTimeDbContextFactory<MiniInstagramContext>
    {
        public MiniInstagramContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MiniInstagramContext>();
            optionsBuilder.UseSqlite("Data Source=D:/Other/MiniInstagram/data.db"); // nebo builder.Configuration

            return new MiniInstagramContext(optionsBuilder.Options);
        }
    }
}
