using Xunit;

namespace CustomTestFramework.Core
{
    public class TestFixture<TSetup> : TestFixture, IClassFixture<TSetup> where TSetup: class
    {
        protected readonly TSetup Setup;
        public TestFixture(TSetup setup)
        {
            Setup = setup;
        }
    }
    public class TestFixture
    {
        // static methods
    }
}
