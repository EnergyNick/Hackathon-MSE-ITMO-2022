using System.Net;
using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace StudentManager.Service.Controllers;

public abstract class ExtendedMappingController : Controller
{
    protected readonly IMapper Mapper;

    protected ExtendedMappingController(IMapper mapper)
    {
        Mapper = mapper;
    }

    protected ActionResult<TOut> CreateResponseByResult<TIn, TOut>(Result<TIn> result,
        HttpStatusCode errorStatusCode = HttpStatusCode.BadRequest)
        => result.IsSuccess
            ? Mapper.Map<TOut>(result.ValueOrDefault)
            : CreateFailResult(result.Errors, errorStatusCode);

    protected ActionResult CreateResponseByResult<TIn>(Result<TIn> result, Type destType,
        HttpStatusCode errorStatusCode = HttpStatusCode.BadRequest)
    {
        if (result.IsFailed)
            return CreateFailResult(result.Errors, errorStatusCode);
        var item = Mapper.Map(result.ValueOrDefault, typeof(TIn), destType);
        return Ok(item);
    }

    protected ActionResult<T> CreateResponseByResult<T>(Result<T> result,
        HttpStatusCode errorStatusCode = HttpStatusCode.BadRequest)
        => result.IsSuccess ? Ok(result.ValueOrDefault) : CreateFailResult(result.Errors, errorStatusCode);

    protected ActionResult CreateResponseByResult(Result result,
        HttpStatusCode errorStatusCode = HttpStatusCode.BadRequest)
        => result.IsSuccess ? Ok() : CreateFailResult(result.Errors, errorStatusCode);

    protected ActionResult<T> CreateResponseByResult<T>(Result result,
        HttpStatusCode errorStatusCode = HttpStatusCode.BadRequest)
        => result.IsSuccess ? Ok() : CreateFailResult(result.Errors, errorStatusCode);

    protected static ActionResult CreateFailResult(IEnumerable<IError> errors, HttpStatusCode errorStatusCode = HttpStatusCode.BadRequest)
    {
        var message = errors.Select(x => x.Message).FirstOrDefault(x => x != null);
        if (message is not null)
            return new ObjectResult(message) { StatusCode = (int) errorStatusCode };
        return new StatusCodeResult((int) errorStatusCode);
    }

    protected static ActionResult CreateFailResultWithMultiplyMessages(IEnumerable<IError> errors,
        HttpStatusCode errorStatusCode = HttpStatusCode.BadRequest)
    {
        var messages = errors.Select(x => x.Message).Where(x => x != null);
        if (messages.Any())
            return new ObjectResult(messages) { StatusCode = (int) errorStatusCode };
        return new StatusCodeResult((int) errorStatusCode);
    }

    private static ObjectResult CreateFailResult(IEnumerable<IdentityError> errors, HttpStatusCode errorStatusCode)
    {
        var errs = errors.Select(x => x.Description).Where(x => x != null);
        return new ObjectResult(errs) { StatusCode = (int) errorStatusCode };
    }
}