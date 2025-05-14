namespace WebApplicationResultPattern
{
    public class Result<T, TError>
    {
        public bool IsSuccess { get; }
        private T? value { get; }
        private TError? error { get; }


        public T Value
        {
            get
            {
                return IsSuccess
                              ? value!
                              : throw new Exception("Result is not a Success.");
            }
        }

        public TError Error
        {
            get
            {
                return !IsSuccess
                          ? error!
                          : throw new Exception("Result is a Success.");
            }
        }


        private Result(bool _isSuccess, T? _value, TError? _error) =>
            (IsSuccess, value, error) = (_isSuccess, _value, _error);

        public static Result<T, TError> Success(T value)
        {
            return new Result<T, TError>(true, value, default);
        }
        public static Result<T, TError> Failure(TError error)
        {
            return new Result<T, TError>(false, default, error);
        }
    }
}
