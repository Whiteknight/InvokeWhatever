using FluentAssertions;
using NUnit.Framework;
using System;

namespace InvokeWhatever.Tests
{
    public class InvokeResultTests
    {
        public class TestObject
        {
            public void TestA() { }
        }

        [Test]
        public void ThrowOnError()
        {
            var obj = new TestObject();
            Action act = () => Invoke.InstanceMethod(obj, "TestA").ThrowOnError();
            act.Should().NotThrow();

            act = () => Invoke.InstanceMethod(obj, "TestB").ThrowOnError();
            act.Should().Throw<InvokeException>();
        }
    }
}
