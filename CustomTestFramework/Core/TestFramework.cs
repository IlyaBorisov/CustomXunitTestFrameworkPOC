using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;
namespace CustomTestFramework.Core
{
    public class TestFramework : XunitTestFramework
    {
        public TestFramework(IMessageSink messageSink)
            : base(messageSink)
        {
            messageSink.OnMessage(new DiagnosticMessage("Using CustomTestFramework"));
        }
        protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"TestFramework.CreateExecutor"));
            var customExecutor = new CustomExecutor(assemblyName, SourceInformationProvider, DiagnosticMessageSink);
            return customExecutor;
        }
    }
}