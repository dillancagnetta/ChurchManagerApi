using Codeboss.Results;

namespace ChurchManager.SharedKernel.Wrappers
{
    public class ApiResponse
    {
        public ApiResponse()
        {
        }

        public ApiResponse(dynamic data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        public ApiResponse(string message)
        {
            Succeeded = false;
            Message = message;
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public dynamic Data { get; set; }
        
        public static ApiResponse Success() => new() {Succeeded = true };
        public static ApiResponse FromOperation(OperationResult op) => new()
        {
            Succeeded = op.IsSuccess,
            Errors = op.Errors?.Select(e => e.Message).ToList() ?? []
        };
        public static ApiResponse FromOperation<T>(OperationResult<T> op) => new()
        {
            Succeeded = op.IsSuccess, 
            Data = op.Result, 
            Errors = op.Errors?.Select(e => e.Message).ToList() ?? []
        };
    }
}