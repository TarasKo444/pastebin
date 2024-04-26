using ErrorOr;

namespace Pastebin.Api.Extensions;

public static class CustomResults
{
    public static IResult ErrorJson(int code, List<Error> errors)
    {
        return Results.Json(statusCode: code, data: new
        {
            status = code,
            errors = errors.Select(e => e.Description)
        });
    }
    
    public static IResult ErrorJson(ErrorType errorType, List<Error> errors)
    {
        var code = errorType switch
        {
            ErrorType.Failure => 400,
            ErrorType.Unauthorized => 401,
            ErrorType.NotFound => 404,
            _ => 500
        };
        
        return Results.Json(statusCode: code, data: new
        {
            status = code,
            errors = errors.Select(e => e.Description)
        });
    }
}