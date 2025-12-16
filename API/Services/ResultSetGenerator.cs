using Common.Entities.Other;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Services;

public static class ResultSetGenerator<T>
{
    public static ResultSet<T> Success(T item)
    {
        ResultSet<T> success = new ResultSet<T>
        {
            IsSuccess = true,
            Data = item,
            ErrorMessages = null
        };

        return success;
    }

    public static ResultSet<T> Failure(T item, ValidationResult modelState)
    {
        var groupedErrors = modelState.Errors
                                .GroupBy(x => x.PropertyName)
                                .Select(g => new ResultSetError
                                {
                                    Name = g.Key,
                                    Mesages = g.Select(e => e.ErrorMessage)
                                                .ToList()
                                });

        ResultSet<T> failure = new ResultSet<T>
        {
            IsSuccess = false,
            Data = item,
            ErrorMessages = groupedErrors.ToList()
        };

        return failure;
    }

    public static ResultSet<T> Failure<T>(T item, List<ResultSetError> errors)
    {
        ResultSet<T> failure = new ResultSet<T>
        {
            IsSuccess = false,
            Data = item,
            ErrorMessages = errors
        };

        return failure;
    }
}
