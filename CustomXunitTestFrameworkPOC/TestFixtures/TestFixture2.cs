namespace TestsProject.TestFixtures
{
    public class TestFixture2
    {
        [Fact]
        public async Task Test1()
        {
            await Task.Delay(2_000);
        }

        [Fact]
        public async Task Test2()
        {
            await Task.Delay(2_000);
        }

        [Fact]
        public async Task Test3()
        {
            await Task.Delay(2_000);
        }
    }
}