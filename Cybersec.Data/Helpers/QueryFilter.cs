using Cybersec.Domain.Commons;
using Cybersec.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Cybersec.Data.Helpers;
public class QueryFilter
{
    public static void AddQueryFilters(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(Auditable).IsAssignableFrom(entityType.ClrType))
                modelBuilder.Entity(entityType.ClrType).AddQueryFilter<Auditable>(e => e.Status == Status.Active);
        }
    }
}
