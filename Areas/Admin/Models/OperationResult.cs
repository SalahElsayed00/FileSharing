namespace FileSharing.Areas.Admin.Models
{
    public class OperationResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public static OperationResult NotFound(string msg = "Item not found")
        {
            return new OperationResult
            {
                Success = false,
                Message = msg
            };
        }
        public static OperationResult succeded(string msg = "operation completed successfully")
        {
            return new OperationResult
            {
                Success = false,
                Message = msg
            };
        }
    }
}
