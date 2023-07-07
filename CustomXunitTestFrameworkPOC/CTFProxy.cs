using CustomTestFramework.Core;
using Xunit.Abstractions;

namespace TestsProject
{
    public class CTFProxy : TestFramework
    {
        public CTFProxy(IMessageSink messageSink) : base(messageSink) {}
    }
}
