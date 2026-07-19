namespace ApplicationCore.Exceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException()
    {
    }

    public AlreadyExistsException(string? message) : base(message)
    {
    }
}