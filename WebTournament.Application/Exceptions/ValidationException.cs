using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace WebTournament.Application.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }
        
    public ValidationException(string propertyName, string error)
        : this()
    {
        Errors = new Dictionary<string, string[]>
        {
            [propertyName] = new[] { error }
        };
    }
        
    public ValidationException(string propertyName, IEnumerable<string> errors)
        : this()
    {
        Errors = new Dictionary<string, string[]>
        {
            [propertyName] = errors.ToArray()
        };
    }
        
    public ValidationException(IEnumerable<IdentityError> failures)
        : this()
    {
        Errors = failures
            .GroupBy(_ => string.Empty, e => e.Description)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}