using System;

namespace InvokeWhatever
{
    public class MethodNotFoundResult : IInvokeResult
    {
        public bool Success => false;

        public bool HasValue => false;

        public object Value => throw new InvalidOperationException(ResultDescription);

        public string ResultDescription => "No methods were found with the given name";
    }

    public class ConstructorNotFoundResult : IInvokeResult
    {
        public bool Success => false;

        public bool HasValue => false;

        public object Value => throw new InvalidOperationException(ResultDescription);

        public string ResultDescription => "No public constructors were found for the given type";
    }
}
