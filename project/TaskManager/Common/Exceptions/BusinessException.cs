public class BusinessException : Exception
{
    public IEnumerable<string>? Errors { get; }

    public BusinessException(string message, IEnumerable<string>? errors = null) : base(message)
    {
        Errors = errors;
    }
}