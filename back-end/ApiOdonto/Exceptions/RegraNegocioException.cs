namespace ApiOdonto.Exceptions;

public sealed class RegraNegocioException : Exception
{
    public RegraNegocioException(string message)
        : base(message)
    {
    }
}
