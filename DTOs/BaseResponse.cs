namespace RealtyWebApp.DTOs
{
    public class BaseResponseModel<T>
    {
        public string Message { get; set; }
        public bool Status { get; set; }
        public T Data { get; set; }
    }

    public class BaseResponse
    {
        public string Message { get; set; }
        public bool Status { get; set; }
    }
}