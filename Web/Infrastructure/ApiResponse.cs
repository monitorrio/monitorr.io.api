namespace Web.Infrastructure
{
    public class ApiResponse
    {
        public string Message { get; set; }
        public object Data { get; set; }
        public string ErrorStatus { get; set; }

        public static ApiResponse MapSuccess(string message, object data, string status)
        {
            var entity = new ApiResponse();
            entity.Message = message;
            entity.Data = data;
            entity.ErrorStatus = status;
            return entity;
        }
        public static ApiResponse MapSuccess(string message,string status)
        {
            var entity = new ApiResponse();
            entity.Message = message;
            entity.Data = null;
            entity.ErrorStatus = status;
            return entity;
        }
        public static ApiResponse MapFailed(string message, string status)
        {
            var entity = new ApiResponse();
            entity.Message = message;
            entity.ErrorStatus = status;
            entity.Data = null;
            return entity;
        }
    }
}
