namespace MyOnboardingApp.Contracts.Errors
{
    public class Error
    {
        public string Message { get; }
        public ErrorCode Code { get; }
        public string Location { get; }


        public Error(ErrorCode code, string message, string location)
        {
            Code = code;
            Message = message;
            Location = location;
        }
    }
}