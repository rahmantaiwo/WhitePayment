namespace WhitePayment.Application.Common
{
    public class BaseResponseModel<T>
    {
        public bool IsSuccess { get; private set; }
        public int StatusCode { get; private set; }
        public string Message { get; private set; }
        public T? Data { get; private set; }

        private BaseResponseModel(bool isSuccess, int statusCode, string message, T? data)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

        public static BaseResponseModel<T> Success(T data, string message, int statusCode = 200)
        {
            return new BaseResponseModel<T>(
                true,
                statusCode,
                message,
                data);
        }

        public static BaseResponseModel<T> Failure(string message, int statusCode = 400)
        {
            return new BaseResponseModel<T>(
                false,
                statusCode,
                message,
                default);
        }
    }

}
