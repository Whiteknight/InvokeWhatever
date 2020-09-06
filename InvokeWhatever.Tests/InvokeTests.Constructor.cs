using FluentAssertions;
using NUnit.Framework;

namespace InvokeWhatever.Tests
{
    public class InvokeTests_Constructor
    {
        private class TestObject
        {
            public string Value { get; private set; }

            public TestObject()
            {
                Value = "A";
            }

            public TestObject(int a, string b = "")
            {
                Value = $"A,{a},{b}";
            }

            public TestObject(object a, string b)
            {
                Value = $"A,{a},{b}";
            }

            public TestObject(int a, int b)
            {
                Value = $"B,{a},{b}";
            }
        }

        [Test]
        public void NoArguments()
        {
            var result = Invoke.Constructor(typeof(TestObject));
            (result.Value as TestObject).Value.Should().Be("A");
        }

        [Test]
        public void ExactArguments()
        {
            var result = Invoke.Constructor(typeof(TestObject), 5, "ok");
            (result.Value as TestObject).Value.Should().Be("A,5,ok");
        }

        [Test]
        public void DefaultParameter()
        {
            var result = Invoke.Constructor(typeof(TestObject), 5);
            (result.Value as TestObject).Value.Should().Be("A,5,");
        }

        [Test]
        public void BoxingDouble()
        {
            var result = Invoke.Constructor(typeof(TestObject), 3.14, "ok");
            (result.Value as TestObject).Value.Should().Be("A,3.14,ok");
        }

        [Test]
        public void NoMatch()
        {
            var result = Invoke.Constructor(typeof(TestObject), "ok");
            (result.Value as TestObject).Value.Should().Be("A");
        }

        [Test]
        public void OrderedArguments()
        {
            // Arguments of the same type should have their relative ordering preserved when the
            // method has multiple arguments of that type
            var result = Invoke.Constructor(typeof(TestObject), 1, 2);
            (result.Value as TestObject).Value.Should().Be("B,1,2");
        }

        private class TestObject2
        {
            public string Value { get; private set; }

            public TestObject2(int a = 0)
            {
                Value = $"A,{a}";
            }
        }

        [Test]
        public void NoArgsDefaultParameterValue()
        {
            // Arguments of the same type should have their relative ordering preserved when the
            // method has multiple arguments of that type
            var result = Invoke.Constructor(typeof(TestObject2));
            (result.Value as TestObject2).Value.Should().Be("A,0");
        }
    }
}