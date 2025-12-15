using System.Linq.Expressions;
using System.Threading.Tasks;
using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.ResponseDTOs.Shared;
using API.Services;
using API.Services.ServicesModels;
using Common.Entities;
using Common.Entities.Other;
using Common.Services;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public delegate Task<ResultSet<K>> TwoIdsDelegate<K>(int id1, int id2);

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
    public async Task<IActionResult> Get([FromBody]EGetRequest filterModel)
    {
        filterModel.Pager ??= new Pager();

        filterModel.SortProperty ??= "Id";
        filterModel.SortAscending = true;

        Expression<Func<E, bool>> filter = GetFilter(filterModel);

        filterModel.SortProperty = typeof(E).GetProperty(filterModel.SortProperty) != null
            ? filterModel.SortProperty
            : "Id";

        EService _service = new EService();
        var items = await _service.GetAllFiltered(
            filter,
            filterModel.SortProperty,
            filterModel.SortAscending,
            filterModel.Pager.PageNumber,
            filterModel.Pager.PageSize
        );

        EntityGetResponse<E, EGetRequest> response = new EntityGetResponse<E, EGetRequest>
        {
            Items = items,
            FilterInfo = filterModel
        };

        return Ok(ResultSetGenerator<EntityGetResponse<E, EGetRequest>>.Success(response));
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        EService _service = new EService();
        var item = await _service.GetById(id);

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

        EResponse response = new EResponse();
        PopulateResponse(item, response);

        return Ok(ResultSetGenerator<EResponse>.Success(response));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ERequest model)
    {
        ValidationResult result = ValidationService<ERequest>.Validate(model);

        if (!result.IsValid)
        {
            return BadRequest(ResultSetGenerator<ERequest>.Failure(model, result));
        }

        E item = new E();
        PopulateRequest(item, model);

        EService _service = new EService();
        await _service.Create(item);

        EResponse responseItem = new EResponse();
        PopulateResponse(item, responseItem);
        

        return Ok(ResultSetGenerator<EResponse>.Success(responseItem));
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Put([FromQuery] int id, [FromBody] ERequest model)
    {
        ValidationResult result = ValidationService<ERequest>.Validate(model);

        if (!result.IsValid)
        {
            return BadRequest(ResultSetGenerator<ERequest>.Failure(model, result));
        }

        E item = new E();
        PopulateRequest(item, model);
      
        EService service = new EService();
        await service.Update(item);

        EResponse responseItem = new EResponse();
        PopulateResponse(item, responseItem);

        return Ok(ResultSetGenerator<EResponse>.Success(responseItem));
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromQuery] int id)
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

        E deletedItem = await _service.Delete(id);

        EResponse response = new EResponse();
        PopulateResponse(deletedItem, response);

        return Ok(ResultSetGenerator<EResponse>.Success(response));
    }

    protected Task<IActionResult> ReturnFromValidationModel<V, R>(ValidationModel<V> validationModel, TwoIdsDelegate<R> delegate_)
    {
        if (validationModel.ResponseType == "BadRequest")
        {
            return Task.FromResult<IActionResult>(BadRequest(
                ResultSetGenerator<V>.Failure(
                    validationModel.Data,
                    validationModel.Errors)));
        }
        else if (validationModel.ResponseType == "NotFound")
        {
            return Task.FromResult<IActionResult>(NotFound(
                ResultSetGenerator<V>.Failure(
                    validationModel.Data,
                    validationModel.Errors)));
        }

        R result = (R)delegate_.DynamicInvoke();
        
        return Task.FromResult<IActionResult>(Ok(ResultSetGenerator<R>.Success(result)));
    }
}
