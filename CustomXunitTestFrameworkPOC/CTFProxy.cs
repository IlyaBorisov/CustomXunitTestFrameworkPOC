using CustomTestFrameworkSpace;
using Xunit.Abstractions;

namespace TestsProject
{
    public class CTFProxy : CustomTestFramework
    {
        public CTFProxy(IMessageSink messageSink) : base(messageSink) {}
    }
}
