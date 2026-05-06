namespace PropertyLeasingSystem.Services
{
    public sealed class WorkflowValidationResult
    {
        private WorkflowValidationResult(bool isValid, string? errorMessage = null)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }

        public bool IsValid { get; }
        public string? ErrorMessage { get; }

        public static WorkflowValidationResult Success()
        {
            return new WorkflowValidationResult(true);
        }

        public static WorkflowValidationResult Failure(string errorMessage)
        {
            return new WorkflowValidationResult(false, errorMessage);
        }
    }
}
