using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Mats.Edvardsen.TestingStuff.Data.UserFeature.Requests;

public record GetUsersRequest : IRequest<IEnumerable<UserEntity>>;

public class GetUsersRequestHandler(DataContext context) : IRequestHandler<GetUsersRequest, IEnumerable<UserEntity>>
{
    public async Task<IEnumerable<UserEntity>> Handle(GetUsersRequest request, CancellationToken cancelToken)
    {
        return await context.Users.ToListAsync(cancelToken);
    }
}