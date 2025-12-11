using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.Shared;
using API.Services;
using Common.Entities;
using Common.Entities.Other;
using Common.Services;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController<E, EService, ERequest, EGetRequest, EResponse> : ControllerBase
where E : BaseEntity, new()
where EService : BaseService<E>, new()
where ERequest : new()
where EGetRequest : BaseFilterModel, new()
where EResponse : new()
{
    protected virtual void PopulateRequest(E entity, ERequest request)
    { }

    protected virtual void PopulateResponse(E entity, EResponse response)
    { }

    protected virtual Expression<Func<E, bool>> GetFilter(EGetRequest request)
    { return e => true; }
    [HttpGet]
    public IActionResult Get([FromBody]EGetRequest filterModel)
    {
        filterModel.Pager ??= new Pager();

        filterModel.SortProperty ??= "Id";
        filterModel.SortAscending = true;

        Expression<Func<E, bool>> filter = GetFilter(filterModel);

        filterModel.SortProperty = typeof(E).GetProperty(filterModel.SortProperty) != null
            ? filterModel.SortProperty
            : "Id";

        EService _service = new EService();
        var items = _service.GetAll(
            filter,
            filterModel.SortProperty,
            filterModel.SortAscending,
            filterModel.Pager.PageNumber,
            filterModel.Pager.PageSize
        );

        return Ok(ResultSetGenerator<List<E>>.Success(items));
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult Get([FromRoute] int id)
    {
        EService _service = new EService();
        var item = _service.GetById(id);

        if (item == null)
        {
            return Ok(ResultSetGenerator<E>
                        .Failure(
                            item,
                            new List<ResultSetError>
                            {
                                new ResultSetError
                                {
                                    Name = typeof(E).Name,
                                    Mesages = new List<string>() { $"{typeof(E).Name} not found" }
                                }
                            }));
        }

        return Ok(ResultSetGenerator<E>.Success(item));
    }

    [HttpPost]
    public IActionResult Post([FromBody] ERequest model)
    {
        ValidationResult result = ValidationService<ERequest>.Validate(model);

        if (!result.IsValid)
        {
            return BadRequest(ResultSetGenerator<ERequest>.Failure(model, result));
        }

        E item = new E();
        PopulateRequest(item, model);

        EService _service = new EService();
        _service.Create(item);

        EResponse responseItem = new EResponse();
        PopulateResponse(item, responseItem);
        

        return Ok(ResultSetGenerator<EResponse>.Success(responseItem));
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult Post([FromQuery] int id, [FromBody] ERequest model)
    {
        ValidationResult result = ValidationService<ERequest>.Validate(model);

        if (!result.IsValid)
        {
            return BadRequest(ResultSetGenerator<ERequest>.Failure(model, result));
        }

        E item = new E();
        PopulateRequest(item, model);
      
        EService service = new EService();
        service.Update(item);

        EResponse responseItem = new EResponse();
        PopulateResponse(item, responseItem);

        return Ok(ResultSetGenerator<EResponse>.Success(responseItem));
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete([FromQuery] int id)
    {
        if (id < 0)
            return BadRequest(ResultSetGenerator<int>
                        .Failure(
                            id,
                            new List<ResultSetError>
                            {
                                new ResultSetError
                                {
                                    Name = "Id",
                                    Mesages = new List<string>
                                    {
                                        "Invalid Id!"
                                    }
                                }
                            }));
        EService _service = new EService();
        var selectedItem = _service.GetById(id);
        if (selectedItem == null)
            return BadRequest(ResultSetGenerator<int>
                        .Failure(
                            id,
                            new List<ResultSetError>
                            {
                                new ResultSetError
                                {
                                    Name = "Id",
                                    Mesages = new List<string>
                                    {
                                        $"No {typeof(E).Name} with this id!"
                                    }
                                }
                            }));

        var deletedItem = _service.Delete(id);

        return Ok(ResultSetGenerator<E>.Success(deletedItem));
    }
}
