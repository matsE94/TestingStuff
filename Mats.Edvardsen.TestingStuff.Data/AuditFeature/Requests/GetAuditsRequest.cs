using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Mats.Edvardsen.TestingStuff.Data.AuditFeature.Requests;

public record GetAuditsRequest(Guid? EntityId = null) : IRequest<IEnumerable<EntityAudit>>;

public class GetAuditsRequestHandler(DataContext context) : IRequestHandler<GetAuditsRequest, IEnumerable<EntityAudit>>
{
    public async Task<IEnumerable<EntityAudit>> Handle(GetAuditsRequest request, CancellationToken cancelToken)
    {
        if (request.EntityId is null)
        {
            return await context.Audits.ToListAsync(cancelToken);
        }

        return await context.Audits.Where(x => x.EntityId == request.EntityId).ToListAsync(cancelToken);
    }
}