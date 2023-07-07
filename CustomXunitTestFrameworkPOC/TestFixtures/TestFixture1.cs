using Xunit.Abstractions;
using Xunit.Sdk;

namespace TestsProject.TestFixtures
{
    public class TestFixture1: IClassFixture<TestSetup1>
    {
        protected readonly TestSetup1 Setup;
        protected readonly ITestOutputHelper TestOutput;
        public TestFixture1(ITestOutputHelper output, TestSetup1 setup)
        {
            TestOutput = output;
            Setup = setup;
        }

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
            TestOutput.WriteLine("TestOutput Test2");
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

    public class TestSetup1 : IDisposable
    {
        protected readonly IMessageSink TestOutput;
        public TestSetup1(IMessageSink output)
        {
            TestOutput = output;
            TestOutput.OnMessage(new DiagnosticMessage("TestSetup1.ctor"));
        }
        public void Dispose()
        {
            TestOutput.OnMessage(new DiagnosticMessage("TestSetup1.Dispose"));
        }
    }
}