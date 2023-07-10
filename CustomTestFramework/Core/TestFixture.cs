using Xunit;
using Xunit.Abstractions;

namespace CustomTestFramework.Core
{
    public abstract class Setup : ISink
    {
        protected Setup(IMessageSink output) : base(output) { }
    }
    public abstract class ISink
    {
        protected readonly IMessageSink Output;
        public ISink(IMessageSink output)
        {
            Output = output;
        }
    }
    public class TestFixture<TSetup> : TestFixture, IClassFixture<TSetup> where TSetup: Setup
    {
        protected readonly TSetup Setup;
        public TestFixture(ITestOutputHelper output, TSetup setup) : base(output)
        {
            Setup = setup;
        }
    }
    public class TestFixture
    {
        // static methods
        protected readonly ITestOutputHelper Output;
        public TestFixture(ITestOutputHelper output)
        {
            Output = output;
        }
    }
}
