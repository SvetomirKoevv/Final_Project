using API.Services.ServicesModels;
using Common.Entities;
using Common.Entities.Other;
using Common.Services;

namespace API.Services;

public class FullEntityRequestValidator<T, TService, E, EService>
where T : BaseEntity, new()
where E : BaseEntity, new() 
where TService : BaseService<T>, new()
where EService : BaseService<E>, new()
{
    public async Task<ValidationModel<int>> Validate(int tId, int eId)
    {
        if (tId <= 0)
        {
            return new ValidationModel<int>
            {
                Data = tId,
                Errors = new List<ResultSetError>
                {
                    new ResultSetError
                    {
                        Name = "Id",
                        Mesages = new List<string>() { $"Invalid {typeof(T).Name} Id!" }
                    }
                },
                ResponseType = "BadRequest"
            };
        }

        if (eId <= 0)
        {
            return new ValidationModel<int>
            {
                Data = eId,
                Errors = new List<ResultSetError>
                {
                    new ResultSetError
                    {
                        Name = "Id",
                        Mesages = new List<string>() { $"Invalid {typeof(T).Name} Id!" }
                    }
                },
                ResponseType = "BadRequest"
            };
        }

        TService tService = new TService();
        T t = await tService.GetById(tId);

        if (t == null)
        {
            return new ValidationModel<int>
            {
                Data = tId,
                Errors = new List<ResultSetError>
                {
                    new ResultSetError
                    {
                        Name = "Id",
                        Mesages = new List<string>() { $"{typeof(T).Name} not found!" }
                    }
                },
                ResponseType = "NotFound"
            };
        }

        EService eService = new EService();
        E e = await eService.GetById(eId);

        if (e == null)
        {
            return new ValidationModel<int>
            {
                Data = eId,
                Errors = new List<ResultSetError>
                {
                    new ResultSetError
                    {
                        Name = "Id",
                        Mesages = new List<string>() { $"{typeof(E).Name} not found!" }
                    }
                },
                ResponseType = "NotFound"
            };
        }
        
        return new ValidationModel<int>
        {
            Data = 0,
            Errors = null,
            ResponseType = "Success"
        };
    }
}