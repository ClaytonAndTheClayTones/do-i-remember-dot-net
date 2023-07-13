namespace WebApi.Validators.Common; 

public interface ICommonValidators
{
    public bool IsNullableGuid(object? value);
    public string GenerateErrorMessage(string propertyName, ErrorMessageTypes errorMessageType);
}

public enum ErrorMessageTypes
{
    MissingRequiredField,
    TypeError_Guid
}

public class CommonValidators : ICommonValidators
{
    public bool IsNullableGuid(object? value)
    {
        if (value != null)
        {
            return Guid.TryParse(value.ToString(), out _);
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