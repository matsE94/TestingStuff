using Mats.Edvardsen.TestingStuff.Data.UserFeature;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Mats.Edvardsen.TestingStuff.Data.AuditFeature;

public class EntityAuditInterceptor(IGuidProvider guidProvider) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync
    (
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancelToken = new()
    )
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancelToken);
        }

        var audits = eventData.Context.ChangeTracker
            .Entries()
            .Where(x => x.Entity is not EntityAudit &&
                        x.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .Select(x => new EntityAudit
            {
                Id = guidProvider.NewGuid(),
                EntityId = ((IIdentifiableEntity)x.Entity).Id,
                TimeStamp = DateTime.UtcNow,
                EntityState = x.State
            }).ToList();
        if (!audits.Any())
        {
            return base.SavingChangesAsync(eventData, result, cancelToken);
        }

        eventData.Context.Set<EntityAudit>().AddRange(audits);
        return base.SavingChangesAsync(eventData, result, cancelToken);
    }
}