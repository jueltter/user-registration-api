using AutoMapper;
using user_registration_api.DefaultDomain.Dtos;
using user_registration_api.DefaultDomain.Models;
using user_registration_api.DefaultDomain.Services;
using EscuelaPolitecnicaNacional.DgipCommonsLib.Exceptions;
using EscuelaPolitecnicaNacional.DgipCommonsLib.Models;
using EscuelaPolitecnicaNacional.DgipCommonsLib.Models.Controllers;
using EscuelaPolitecnicaNacional.DgipCommonsLib.Models.Validators;
using FluentValidation;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using static LanguageExt.Prelude;
using InvalidDataException = EscuelaPolitecnicaNacional.DgipCommonsLib.Exceptions.InvalidDataException;

namespace user_registration_api.DefaultDomain.Controllers;

[Route("api/users")]
[ApiController]
public class UserController(
    IUserService service,
    IMapper mapper,
    IJsonService jsonService,
    IValidator<UserDto> validator,
    IValidator<UserIdDto> idValidator,
    ILogger<UserController> logger) : ControllerBase
{
    [HttpPost("search")]
    [ProducesResponseType(200, Type = typeof(ControllerResult<Page<UserDto>>))]
    public async Task<ActionResult<ControllerResult<Page<UserDto>>>> FindAll([FromBody] UserSearchDto? body)
    {
        logger.Log(LogLevel.Debug, "executing POST /search with body: {}", jsonService.Stringify(body));
        var search = mapper.Map<UserSearch>(body);
        var page = await service.SearchAsync(search);
        var items = page.Items;
        var pagination = page.Pagination;
        var dtoItems = mapper.Map<ICollection<UserDto>>(items);

        var responseBody = ControllerResult<Page<UserDto>>
            .GetSuccessResult(Page<UserDto>.Create(dtoItems, pagination));

        return Ok(responseBody);
    }

    [HttpPost("fetch")]
    [ProducesResponseType(200, Type = typeof(ControllerResult<UserDto>))]
    public async Task<ActionResult<ControllerResult<UserDto>>> FindById([FromBody] UserIdDto body)
    {
        logger.Log(LogLevel.Debug, "executing POST /fetch with body: {}", jsonService.Stringify(body));

        var validationResult = await idValidator.ValidateAsync(body);
        if (!validationResult.IsValid) throw InvalidDataException.GetInstance(validationResult.Errors);

        var item = await service
            .FindByIdAsync(body.IdUser??
                           throw new InvalidOperationException("El campo IdUser no puede ser nulo después de la validación."));

        var dtoItem = item.Match(
            Some: x => Optional(mapper.Map<UserDto>(x)),
            None: () => Option<UserDto>.None
        );

        return dtoItem.Match(
            Some: x => Ok(ControllerResult<UserDto>.GetSuccessResult(x)),
            None: () => throw NotFoundException.GetInstance("IdUser","El campo IdUser no corresponde a ningún registro.")
        );
    }

    [HttpPost("create")]
    [ProducesResponseType(201, Type = typeof(ControllerResult<UserDto>))]
    public async Task<ActionResult<ControllerResult<UserDto>>> Save([FromBody] UserDto body)
    {
        logger.Log(LogLevel.Debug, "executing POST /create with body: {}", jsonService.Stringify(body));

        var validationResult = await validator.ValidateAsync(body);
        if (!validationResult.IsValid) throw InvalidDataException.GetInstance(validationResult.Errors);

        var model = mapper.Map<User>(body);
        var entity = await service.SaveAsync(model);
        var entityDto = mapper.Map<UserDto>(entity);
        return Created(string.Empty, ControllerResult<UserDto>.GetSuccessResult(entityDto));
    }

    [HttpPost("update")]
    [ProducesResponseType(200, Type = typeof(ControllerResult<UserDto>))]
    public async Task<ActionResult<ControllerResult<UserDto>>> Update([FromBody] UserDto body)
    {
        logger.Log(LogLevel.Debug, "executing POST /update with body: {}", jsonService.Stringify(body));

        var multipleValidator = new ModelMultipleValidator();
        multipleValidator.AddValidator(idValidator, UserIdDto.From(body.IdUser));
        multipleValidator.AddValidator(validator, body);
        await multipleValidator.ExecuteValidatorsAsync();
        var validationResult = multipleValidator.GetResult();
        if (!validationResult.IsValid) throw InvalidDataException.GetInstance(validationResult.Errors);

        var model = mapper.Map<User>(body);
        var entity = await service.UpdateAsync(model);

        var dtoItem = entity.Match(
            Some: x => Optional(mapper.Map<UserDto>(x)),
            None: () => Option<UserDto>.None
        );

        return dtoItem.Match(
            Some: x => Ok(ControllerResult<UserDto>.GetSuccessResult(x)),
            None: () => throw NotFoundException.GetInstance("IdUser","El campo IdUser no corresponde a ningún registro.")
        );
    }

    [HttpPost("delete")]
    [ProducesResponseType(200, Type = typeof(ControllerResult<UserDto>))]
    public async Task<ActionResult<ControllerResult<UserDto>>> Delete([FromBody] UserIdDto body)
    {
        logger.Log(LogLevel.Debug, "executing POST /delete with body: {}", jsonService.Stringify(body));

        var validationResult = await idValidator.ValidateAsync(body);
        if (!validationResult.IsValid) throw InvalidDataException.GetInstance(validationResult.Errors);

        var entity = await service.DeleteAsync(body.IdUser??
                                               throw new InvalidOperationException("El campo IdUser no puede ser nulo después de la validación."));

        var dtoItem = entity.Match(
            Some: x => Optional(mapper.Map<UserDto>(x)),
            None: () => Option<UserDto>.None
        );

        return dtoItem.Match(
            Some: x => Ok(ControllerResult<UserDto>.GetSuccessResult(x)),
            None: () => throw NotFoundException.GetInstance("IdUser","El campo IdUser no corresponde a ningún registro.")
        );
    }
}