namespace SmartStay.Domain.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string resourceName, object key) 
        : base($"Resource '{resourceName}' with key '{key}' was not found.")
    {
    }
}
