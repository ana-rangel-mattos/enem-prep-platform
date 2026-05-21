namespace EnemPrep.Domain.Result;

public class Result
{
    protected Result(bool isSuccess, Error error)
    { 
        if (isSuccess && error != Error.None || 
            !isSuccess && error == Error.None)
        { 
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }
  
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }
    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value)
    {
        return new Result<TValue>(value, true, Error.None);
    }

    public static Result<TValue> Failure<TValue>(Error error)
    {
        return new Result<TValue>(default, false, error);
    }
}

public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;

    internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value
    {
        get
        {
            if (IsFailure)
            {
                throw new InvalidOperationException("Cannot access the value of an error result.");
            }

            return _value!;
        }
    }
}
