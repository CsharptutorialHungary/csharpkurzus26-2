
using System.Diagnostics.CodeAnalysis;

namespace Calculator.Core;

public sealed class Either<TSuccess, TError>
    where TError : Exception
{
    private readonly TSuccess? _success;
    private readonly TError? _error;
    private readonly bool _isSuccess;

    private Either(TSuccess success)
    {
        _success = success;
        _isSuccess = true;
    }

    private Either(TError error)
    {
        _error = error;
        _isSuccess = false;
    }
    
    public static implicit operator Either<TSuccess, TError>(TSuccess success)
    {
        return new Either<TSuccess, TError>(success);
    }

    public static implicit operator Either<TSuccess, TError>(TError error)
    {
        return new Either<TSuccess, TError>(error);
    }

    public bool TryGetSuccess([NotNullWhen(true)] out TSuccess? result)
    {
        if (_isSuccess)
        {
            result = _success!;
            return true;
        }

        result = default;
        return false;
    }

    public bool TryGetError([NotNullWhen(true)] out TError? result)
    {
        if (!_isSuccess)
        {
            result = _error!;
            return true;
        }

        result = default;
        return false;
    }


}
