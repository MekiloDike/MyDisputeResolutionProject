using Microsoft.EntityFrameworkCore;

namespace DisputeResolutionCore.DisputeContext
{
    public class DisputeDbContext : DbContext
    {
        public DisputeDbContext(DbContextOptions<DisputeDbContext> options) : base(options)
        {

        }
    }
}
