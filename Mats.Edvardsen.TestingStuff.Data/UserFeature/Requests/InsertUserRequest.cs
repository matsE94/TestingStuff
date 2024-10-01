using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Mats.Edvardsen.TestingStuff.Data.UserFeature.Requests;
public class UserInsertDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}
public record InsertUserRequest(UserInsertDto Dto) : IRequest<UserEntity>;

public class InsertUserRequestHandler
(
    DataContext context,
    IGuidProvider guidProvider
) : IRequestHandler<InsertUserRequest, UserEntity>
{
    public async Task<UserEntity> Handle(InsertUserRequest request, CancellationToken cancelToken)
    {
        var dto = request.Dto;
        UserEntity? user;

        if (dto.Id is null)
        {
            user = new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Age = dto.Age,
            };
            await context.Users.AddAsync(user, cancelToken);
            await context.SaveChangesAsync(cancelToken);
            return user;
        }

        //update if exists
        user = await context.Users.FirstOrDefaultAsync(x => x.Id == dto.Id, cancelToken);
        if (user is null)
        {
            user = new UserEntity
            {
                Id = guidProvider.NewGuid(),
                Name = dto.Name,
                Age = dto.Age,
            };
            await context.Users.AddAsync(user, cancelToken);
            await context.SaveChangesAsync(cancelToken);
            return user;
        }

        user.Name = dto.Name;
        user.Age = dto.Age;
        context.Users.Update(user);
        await context.SaveChangesAsync(cancelToken);
        return user;
    }
}