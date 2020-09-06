using System;

namespace InvokeWhatever
{
    public class NoSuitableMethodsResult : IInvokeResult
    {
        public bool Success => false;

        public bool HasValue => false;

        public object Value => throw new InvalidOperationException(ResultDescription);

        public string ResultDescription => "No suitable methods were found for the given name and arguments";
    }

    public class NoSuitableConstructorResult : IInvokeResult
    {
        public bool Success => false;

        public bool HasValue => false;

        public object Value => throw new InvalidOperationException(ResultDescription);

        public string ResultDescription => "No suitable constructors were found for the given type and arguments";
    }
}
