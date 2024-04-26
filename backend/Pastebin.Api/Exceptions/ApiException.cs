using FluentValidation;

namespace Pastebin.Api.Exceptions;

public record ApiError(string Details, string? Property = null)
{
    public string Message => Property is null ? Details : string.Format(Details, Property);
}

public class ApiException : Exception
{
    public IEnumerable<ApiError> Errors { get; set; }
    public int Status { get; set; }

    public ApiException(int status, IEnumerable<ApiError> errors)
    {
        Status = status;
        Errors = errors;
    }

    public ApiException(int status, string error)
    {
        Status = status;
        Errors = [new(error)];
    }

    public static async Task ValidateAsync<T>(IValidator<T> validator, T? model)
    {
        if (model is null)
        {
            throw new ApiException(400, $"{typeof(T).Name} is null");
        }
        
        var result = await validator.ValidateAsync(model);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(e => new ApiError(e.ErrorMessage, e.PropertyName));
            throw new ApiException(400, errors);
        }
    }
}