using API.Infrastructure.RequestDTOs;
using FluentValidation.Results;

namespace API.Services;

public static class ValidationService<T>
{
    public static ValidationResult Validate(T model)
    {
        if (typeof(T) == typeof(UserRequest))
        {
            UserRequestValidator validator = new UserRequestValidator();
            return validator.Validate(model as UserRequest);
        }
        return null;
    }
}
