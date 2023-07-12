namespace WebApi.Models.Validators;
 
using System.ComponentModel.DataAnnotations; 

public class IsNullableGuid : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value != null)
        {
            return Guid.TryParse(value.ToString(), out _);
        }

        else return true;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"Property {name} must be a valid GUID in the format {Guid.NewGuid}.";
    }
}