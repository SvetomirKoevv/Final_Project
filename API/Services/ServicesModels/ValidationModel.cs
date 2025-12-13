using Common.Entities.Other;

namespace API.Services.ServicesModels;

public class ValidationModel<T>
{
    public string ResponseType { get; set; }
    public T Data { get; set; } 
    public List<ResultSetError> Errors { get; set; }
}
