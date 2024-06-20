

using DisputeResolutionInfrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace DisputeResolutionInfrastructure.Context
{
    public class DisputeContext : DbContext
    {
        public DisputeContext(DbContextOptions<DisputeContext> options) : base(options)
        {

        }
        public DbSet<DisputeResquestLog> DisputeRequestLogs { get; set; }
        public DbSet<DisputeResponseLog> DisputeResponseLogs { get; set; }
    }
}
