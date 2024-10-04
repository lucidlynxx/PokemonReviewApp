using System;
using System.ComponentModel.DataAnnotations;
using PokemonReviewApp.Models.Enum;

namespace PokemonReviewApp.Dto.Validation;

public class ValidateRolesAttribute : ValidationAttribute
{
    public ValidateRolesAttribute()
    {
        const string defaultErrorMessage = "Roles format is invalid.";
        ErrorMessage ??= defaultErrorMessage;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        List<string>? rolesToList = value as List<string>;

        string[] rolesToArray = rolesToList!.ToArray();

        if (rolesToArray == null || rolesToArray.Length == 0)
            return new ValidationResult("Roles cannot be empty.");

        if (!rolesToArray.GetType().IsArray)
            return new ValidationResult("Roles must be an array.");

        foreach (var role in rolesToArray)
        {
            if (!Enum.IsDefined(typeof(Role), role))
                return new ValidationResult($"Invalid role: {role}. Allowed roles are: superAdmin, admin, moderator, user.");
        }

        return ValidationResult.Success;
    }
}
