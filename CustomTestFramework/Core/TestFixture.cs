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
        protected readonly IMessageSink TestOutput;
        public ISink(IMessageSink output)
        {
            TestOutput = output;
        }
    }
    public class TestFixture<TSetup> : TestFixture, IClassFixture<TSetup> where TSetup: Setup
    {
        protected readonly TSetup Setup;
        public TestFixture(IMessageSink output, TSetup setup) : base(output)
        {
            Setup = setup;
        }
    }
    public class TestFixture
    {
        // static methods
        protected readonly IMessageSink TestOutput;
        public TestFixture(IMessageSink output)
        {
            TestOutput = output;
        }
    }
}
