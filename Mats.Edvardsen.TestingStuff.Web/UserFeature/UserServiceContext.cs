using FluentValidation;
using Mats.Edvardsen.TestingStuff.Data;
using Mats.Edvardsen.TestingStuff.Data.UserFeature;
using Mats.Edvardsen.TestingStuff.Data.UserFeature.Requests;
using Mats.Edvardsen.TestingStuff.Web.UserFeature.RequestModels;
using Mats.Edvardsen.TestingStuff.Web.UserFeature.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Mats.Edvardsen.TestingStuff.Web.UserFeature;

public interface IUserServiceContext
{
    Task<UserInsertDto> CreateStronglyTypedEntityFromJsonDtoAsync(UserInsertDtoJson dto);
}

public class UserServiceContext
(
    IValidator<UserInsertDtoJson> validator,
    IGuidProvider guidProvider
) : IUserServiceContext
{
    public IGuidProvider GuidProvider { get; set; } = guidProvider;

    public async Task<UserInsertDto> CreateStronglyTypedEntityFromJsonDtoAsync(UserInsertDtoJson dto)
    {
        var validationResult = await validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors); // catch by global exception handler?
        return new UserInsertDto
        {
            Id = dto.Id, // if null someone else should set it be fore creating
            Name = dto.Name ?? throw new NotSupportedException("Expected the property to be validated already."),
            Age = dto.Age ?? throw new NotSupportedException("Expected the property to be validated already."),
        };
    }
}