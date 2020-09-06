using FluentAssertions;
using NUnit.Framework;

namespace InvokeWhatever.Tests
{
    public class InvokeTests_StaticMethod
    {
        private class TestObject
        {
            public static string TestA() => "A";
            public static string TestA(int a, string b = "") => $"A,{a},{b}";
            public static string TestA(object a, string b) => $"A,{a},{b}";

            public static string TestB(int a, int b) => $"B,{a},{b}";

            public static string TestC(int a = 0) => $"C,{a}";
        }

        [Test]
        public void NoArguments()
        {
            var result = Invoke.StaticMethod(typeof(TestObject), "TestA");
            result.Value.Should().Be("A");
        }

        [Test]
        public void NoArguments_CaseInsensitive()
        {
            var result = Invoke.StaticMethod(typeof(TestObject), "testa");
            result.Value.Should().Be("A");
        }

        [Test]
        public void ExactArguments()
        {
            var result = Invoke.StaticMethod(typeof(TestObject), "TestA", 5, "ok");
            result.Value.Should().Be("A,5,ok");
        }

        [Test]
        public void DefaultParameter()
        {
            var result = Invoke.StaticMethod(typeof(TestObject), "TestA", 5);
            result.Value.Should().Be("A,5,");
        }

        [Test]
        public void BoxingDouble()
        {
            var result = Invoke.StaticMethod(typeof(TestObject), "TestA", 3.14, "ok");
            result.Value.Should().Be("A,3.14,ok");
        }

        [Test]
        public void NoMatch()
        {
            // TestA has a parameterless overload, so that will be invoked
            var result = Invoke.StaticMethod(typeof(TestObject), "TestA", "ok");
            result.Value.Should().Be("A");

            // TestB has no parameterless overload, so there is no match 
            result = Invoke.StaticMethod(typeof(TestObject), "TestB", "ok");
            result.Success.Should().BeFalse();
        }

        [Test]
        public void OrderedArguments()
        {
            // Arguments of the same type should have their relative ordering preserved when the
            // method has multiple arguments of that type
            var result = Invoke.StaticMethod(typeof(TestObject), "TestB", 1, 2);
            result.Value.Should().Be("B,1,2");
        }

        [Test]
        public void NoArgsDefaultParameterValue()
        {
            // Arguments of the same type should have their relative ordering preserved when the
            // method has multiple arguments of that type
            var result = Invoke.StaticMethod(typeof(TestObject), "TestC");
            result.Value.Should().Be("C,0");
        }
    }
}