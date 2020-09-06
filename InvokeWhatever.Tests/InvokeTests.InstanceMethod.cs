using FluentAssertions;
using NUnit.Framework;

namespace InvokeWhatever.Tests
{
    public class InvokeTests_InstanceMethod
    {
        private class TestObject
        {
            public string TestA() => "A";
            public string TestA(int a, string b = "") => $"A,{a},{b}";
            public string TestA(object a, string b) => $"A,{a},{b}";

            public string TestB(int a, int b) => $"B,{a},{b}";

            public string TestC(int a = 0) => $"C,{a}";
        }

        [Test]
        public void NoArguments()
        {
            var obj = new TestObject();
            var result = Invoke.InstanceMethod(obj, "TestA");
            result.Value.Should().Be("A");
        }

        [Test]
        public void NoArguments_CaseInsensitive()
        {
            var obj = new TestObject();
            var result = Invoke.InstanceMethod(obj, "testa");
            result.Value.Should().Be("A");
        }

        [Test]
        public void ExactArguments()
        {
            var obj = new TestObject();
            var result = Invoke.InstanceMethod(obj, "TestA", 5, "ok");
            result.Value.Should().Be("A,5,ok");
        }

        [Test]
        public void DefaultParameter()
        {
            var obj = new TestObject();
            var result = Invoke.InstanceMethod(obj, "TestA", 5);
            result.Value.Should().Be("A,5,");
        }

        [Test]
        public void BoxingDouble()
        {
            var obj = new TestObject();
            var result = Invoke.InstanceMethod(obj, "TestA", 3.14, "ok");
            result.Value.Should().Be("A,3.14,ok");
        }

        [Test]
        public void NoMatch()
        {
            // TestA has a parameterless overload, so that will be invoked
            var obj = new TestObject();
            var result = Invoke.InstanceMethod(obj, "TestA", "ok");
            result.Value.Should().Be("A");

            // TestB has no parameterless overload, so there is no match 
            result = Invoke.InstanceMethod(obj, "TestB", "ok");
            result.Success.Should().BeFalse();
        }

        [Test]
        public void OrderedArguments()
        {
            // Arguments of the same type should have their relative ordering preserved when the
            // method has multiple arguments of that type
            var obj = new TestObject();
            var result = Invoke.InstanceMethod(obj, "TestB", 1, 2);
            result.Value.Should().Be("B,1,2");
        }

        [Test]
        public void NoArgsDefaultParameterValue()
        {
            // Arguments of the same type should have their relative ordering preserved when the
            // method has multiple arguments of that type
            var obj = new TestObject();
            var result = Invoke.InstanceMethod(obj, "TestC");
            result.Value.Should().Be("C,0");
        }
    }
}