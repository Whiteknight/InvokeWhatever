namespace InvokeWhatever
{
    public interface IInvokeResult
    {
        bool Success { get; }
        bool HasValue { get; }
        object Value { get; }
        string ResultDescription { get; }
    }

    public static class InvokeResultExtensions
    {
        public static void ThrowOnError(this IInvokeResult result)
        {
            if (result == null || result.Success)
                return;

            throw new InvokeException($"Invoke failed: {result.ResultDescription}");
        }
    }
}
