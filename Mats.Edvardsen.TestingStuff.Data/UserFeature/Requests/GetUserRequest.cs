using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Mats.Edvardsen.TestingStuff.Data.UserFeature.Requests;

public record GetUserRequest(Guid Id) : IRequest<UserEntity?>;

public class GetUserRequestHandler(DataContext context) : IRequestHandler<GetUserRequest, UserEntity?>
{
    public async Task<UserEntity?> Handle(GetUserRequest request, CancellationToken cancelToken)
    {
        return await context.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancelToken);
    }
}