namespace WebApi.Validators.Common; 

public interface ICommonValidators
{
    public bool IsGuid(object? value);
    public bool IsDateOnly(object? value);
    public string GenerateErrorMessage(string propertyName, ErrorMessageTypes errorMessageType);
}

public enum ErrorMessageTypes
{
    MissingRequiredField,
    TypeError_Guid,
    TypeError_DateOnly
}

public class CommonValidators : ICommonValidators
{
    public bool IsGuid(object? value)
    {
        if (value != null)
        {
            return Guid.TryParse(value.ToString(), out _);
        }

        else return true;
    }

    public bool IsDateOnly(object? value)
    {
        if (value != null)
        {
            return DateOnly.TryParse(value.ToString(), out _);
        }

        else return true;
    }

    public string GenerateErrorMessage(string propertyName, ErrorMessageTypes errorMessageType)
    {
        switch(errorMessageType)
        {
            case ErrorMessageTypes.MissingRequiredField:
                {
                    return $"Required property {propertyName} is missing.";
                }
            case ErrorMessageTypes.TypeError_Guid:
                {
                    return $"Property {propertyName} must be a valid GUID in the format {Guid.Empty.ToString().Replace("0","x")}.";
                }
            default:
                {
                    return $"Property {propertyName} is invalid.";
                }

        }
    }
}