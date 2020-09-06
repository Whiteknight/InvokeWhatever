using System;
using System.Linq;
using System.Reflection;

namespace InvokeWhatever
{
    public static class Invoke
    {
        public static IInvokeResult InstanceMethod(object obj, string methodName, params object[] availableArgs)
            => InstanceMethod(MethodSortOptions.Default, obj, methodName, availableArgs);

        public static IInvokeResult InstanceMethod(MethodSortOptions options, object obj, string methodName, params object[] availableArgs)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Must provide an object reference");
            if (string.IsNullOrEmpty(methodName))
                throw new ArgumentException("Must provide a method name to invoke", nameof(methodName));
            availableArgs = availableArgs ?? new object[0];
            var type = obj.GetType();

            var methods = type.GetMethods()
                .Where(method => method.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase))
                .Where(method => method.IsPublic && !method.IsStatic)
                .ToList();
            if (methods.Count == 0)
                return new MethodNotFoundResult();

            var sorter = new MethodSorter(options);
            var bestMethod = sorter.GetBestMethod(methods, availableArgs);
            if (bestMethod?.Member == null)
                return new NoSuitableMethodsResult();
            var methodInfo = bestMethod.Member as MethodInfo;
            if (methodInfo == null)
                return new NoSuitableMethodsResult();

            if (methodInfo.ReturnType == typeof(void))
            {
                methodInfo.Invoke(obj, bestMethod.Arguments);
                return new ActionSuccessResult();
            }

            var result = methodInfo.Invoke(obj, bestMethod.Arguments);
            return new FuncSuccessResult(result);
        }

        public static IInvokeResult StaticMethod(Type type, string methodName, params object[] availableArgs)
            => StaticMethod(MethodSortOptions.Default, type, methodName, availableArgs);

        public static IInvokeResult StaticMethod(MethodSortOptions options, Type type, string methodName, params object[] availableArgs)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type), "Must provide a valid Type");
            if (string.IsNullOrEmpty(methodName))
                throw new ArgumentException("Must provide a method name to invoke", nameof(methodName));
            availableArgs = availableArgs ?? new object[0];

            var methods = type.GetMethods()
                .Where(method => method.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase))
                .Where(method => method.IsPublic && method.IsStatic)
                .ToList();
            if (methods.Count == 0)
                return new MethodNotFoundResult();

            var sorter = new MethodSorter(options);
            var bestMethod = sorter.GetBestMethod(methods, availableArgs);
            if (bestMethod?.Member == null)
                return new NoSuitableMethodsResult();
            var methodInfo = bestMethod.Member as MethodInfo;
            if (methodInfo == null)
                return new NoSuitableMethodsResult();

            if (methodInfo.ReturnType == typeof(void))
            {
                methodInfo.Invoke(null, bestMethod.Arguments);
                return new ActionSuccessResult();
            }

            var result = methodInfo.Invoke(null, bestMethod.Arguments);
            return new FuncSuccessResult(result);
        }

        public static IInvokeResult Constructor(Type type, params object[] availableArgs)
            => Constructor(MethodSortOptions.Default, type, availableArgs);

        public static IInvokeResult Constructor(MethodSortOptions options, Type type, params object[] availableArgs)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type), "Must provide a valid type");
            availableArgs = availableArgs ?? new object[0];
            if (type.IsAbstract || type.IsInterface)
                return new ConstructorNotFoundResult();

            var ctors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (ctors.Length == 0)
                return new ConstructorNotFoundResult();

            var sorter = new MethodSorter(options);
            var bestCtor = sorter.GetBestMethod(ctors, availableArgs);
            if (bestCtor?.Member == null)
                return new NoSuitableConstructorResult();
            var ctorInfo = bestCtor.Member as ConstructorInfo;
            if (ctorInfo == null)
                return new NoSuitableConstructorResult();

            var obj = ctorInfo.Invoke(bestCtor.Arguments);
            return new FuncSuccessResult(obj);
        }
    }
}
