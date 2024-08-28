using System.ComponentModel.DataAnnotations;

namespace PSV.Models.DTOs;

public class UniqueOrderNumberValidator : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var dbContext = (Repository?)validationContext.GetService(typeof(Repository));
        string? orderNumber = value as string;
        if (dbContext != null && dbContext.Orders.Any(o => o.OrderNumber == orderNumber))
        {
            return new ValidationResult("Numer zamówienia musi być unikalny");
        }

        return ValidationResult.Success;
    }
}