using FluentValidation;

namespace Mats.Edvardsen.TestingStuff.Web.UserFeature.RequestModels;

public class UserInsertDtoJson // never trust json o.O
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public int? Age { get; set; }
}

public class UserInsertDtoJsonValidator : AbstractValidator<UserInsertDtoJson>
{
    public UserInsertDtoJsonValidator()
    {
        RuleFor(x => x.Name).NotNull();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Age).NotNull();
    }
}