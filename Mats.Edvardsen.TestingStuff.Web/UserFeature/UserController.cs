using Mats.Edvardsen.TestingStuff.Data.AuditFeature;
using Mats.Edvardsen.TestingStuff.Data.AuditFeature.Requests;
using Mats.Edvardsen.TestingStuff.Data.UserFeature.Requests;
using Mats.Edvardsen.TestingStuff.Web.Common;
using Mats.Edvardsen.TestingStuff.Web.UserFeature.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mats.Edvardsen.TestingStuff.Web.UserFeature;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class UserController
(
    IUserServiceContext serviceContext,
    IMediator mediator
) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, [FromQuery(Name = "viewModelType")] ViewModelType viewModelType)
    {
        var entity = await mediator.Send(new GetUserRequest(id));

        if (entity is null) return NotFound("User with id {id} was not found");

        if (viewModelType is not ViewModelType.Full)
            return viewModelType is ViewModelType.Display
                ? Ok(entity.MapToDisplayViewModel())
                : Ok(entity.Id);

        var audit = await mediator.Send(new GetAuditsRequest(id));
        var aggregate = entity.MapToFullViewModel(audit);
        return Ok(aggregate);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery(Name = "viewModelType")] ViewModelType viewModelType)
    {
        var entities = await mediator.Send(new GetUsersRequest());

        if (viewModelType is not ViewModelType.Full)
            return viewModelType is ViewModelType.Display
                ? Ok(entities.MapToDisplayViewModel())
                : Ok(entities.Select(x => x.Id));

        var audits = await mediator.Send(new GetAuditsRequest());
        var aggregates = entities.MapToFullViewModel(audits);
        return Ok(aggregates);
    }

    [HttpPost]
    public async Task<IActionResult> Upsert([FromBody] UserInsertDtoJson dto)
    {
        var typedDto = await serviceContext.CreateStronglyTypedEntityFromJsonDtoAsync(dto);
        var entity = await mediator.Send(new InsertUserRequest(typedDto));
        var audits = await mediator.Send(new GetAuditsRequest(entity.Id));
        var viewModel = entity.MapToFullViewModel(audits);
        return Ok(viewModel);
    }
}