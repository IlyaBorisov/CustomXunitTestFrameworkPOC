using CustomTestFramework.Core;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TestsProject.TestFixtures
{
    public class TestFixture1 : TestFixture<TestSetup1>
    {
        public TestFixture1(ITestOutputHelper output, TestSetup1 setup) : base(output, setup) { }

        [Fact]
        [Trait("Category", "Lala")]
        [Trait("Severity", "Blocker")]
        public async Task Test1()
        {
            await Task.Delay(2_000);
        }

        [Fact()]
        [Trait("Category", "Lala")]
        [Trait("Severity", "Critical")]
        public async Task Test2()
        {
            Output.WriteLine("TestOutput Test2");
            await Task.Delay(2_000);
        }

        [Fact]
        [Trait("Category", "Lala")]
        [Trait("Severity", "Critical")]
        public async Task Test3()
        {
            await Task.Delay(2_000);
        }
    }

    public class TestSetup1 : Setup, IDisposable
    {
        public TestSetup1(IMessageSink output) : base(output)
        {
            Output.OnMessage(new DiagnosticMessage("TestSetup1.ctor"));
        }
        public void Dispose()
        {
            Output.OnMessage(new DiagnosticMessage("TestSetup1.Dispose"));
        }
    }
}