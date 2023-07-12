namespace WebApi.Validators.Common; 

public interface ICommonValidators
{
    public bool IsNullableGuid(object? value);
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
     
}