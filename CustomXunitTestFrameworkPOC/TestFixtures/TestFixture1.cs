using CustomTestFramework.Core;

namespace TestsProject.TestFixtures
{
    public class TestFixture1: TestFixture<TestSetup1>
    {
        public TestFixture1(TestSetup1 setup) : base(setup) {}

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
        public TestSetup1()
        {
            
        }
        public void Dispose()
        {
            
        }
    }
}