using Mats.Edvardsen.TestingStuff.Data;
using Mats.Edvardsen.TestingStuff.Data.AuditFeature;
using Mats.Edvardsen.TestingStuff.Data.UserFeature;
using Mats.Edvardsen.TestingStuff.Web.UserFeature.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Mats.Edvardsen.TestingStuff.Web.UserFeature;

public static class UserEntityExtensions
{
    public static UserDisplayViewModel MapToDisplayViewModel(this UserEntity entity)
    {
        return new UserDisplayViewModel
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }

    public static IEnumerable<UserDisplayViewModel> MapToDisplayViewModel(this IEnumerable<UserEntity> entities)
        => entities.Select(x => x.MapToDisplayViewModel());

    public static UserFullViewModel MapToFullViewModel(this UserEntity entity, IEnumerable<EntityAudit> audits)
    {
        var entityAudits = audits.ToList();
        return new UserFullViewModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Age = entity.Age,
            Created = entityAudits.Single(a => a.EntityState is EntityState.Added).TimeStamp,
            Modified = entityAudits.OrderByDescending(a => a.TimeStamp).First().TimeStamp,
        };
    }

    public static IEnumerable<UserDisplayViewModel> MapToFullViewModel
    (
        this IEnumerable<UserEntity> entities,
        IEnumerable<EntityAudit> audits
    ) => entities.Select(x => x.MapToDisplayViewModel());
}