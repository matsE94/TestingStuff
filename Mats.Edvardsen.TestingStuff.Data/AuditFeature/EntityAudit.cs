using Microsoft.EntityFrameworkCore;

namespace Mats.Edvardsen.TestingStuff.Data.AuditFeature;

public class EntityAudit
{
    public Guid Id { get; set; }
    public Guid EntityId { get; set; }
    public DateTime TimeStamp { get; set; }
    public EntityState EntityState { get; set; }
}