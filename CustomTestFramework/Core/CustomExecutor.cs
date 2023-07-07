using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CustomTestFramework.Core
{
    public class CustomExecutor : XunitTestFrameworkExecutor
    {
        public CustomExecutor(AssemblyName assemblyName, ISourceInformationProvider sourceInformationProvider, IMessageSink diagnosticMessageSink)
            : base(assemblyName, sourceInformationProvider, diagnosticMessageSink)
        {
        }

        protected override ITestFrameworkDiscoverer CreateDiscoverer()
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomExecutor.CreateDiscoverer"));
            return base.CreateDiscoverer();
        }

        public override ITestCase Deserialize(string value)
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomExecutor.Deserialize"));
            return base.Deserialize(value);
        }

        public override void RunTests(IEnumerable<ITestCase> testCases, IMessageSink executionMessageSink, ITestFrameworkExecutionOptions executionOptions)
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomExecutor.RunTests"));
            base.RunTests(testCases, executionMessageSink, executionOptions);
        }

        public override void RunAll(IMessageSink executionMessageSink, ITestFrameworkDiscoveryOptions discoveryOptions, ITestFrameworkExecutionOptions executionOptions)
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomExecutor.RunAll"));
            base.RunAll(executionMessageSink, discoveryOptions, executionOptions);
        }

        protected override async void RunTestCases(IEnumerable<IXunitTestCase> testCases, IMessageSink executionMessageSink, ITestFrameworkExecutionOptions executionOptions)
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomExecutor.RunTestCases"));
            using var assemblyRunner = new CustomAssemblyRunner(TestAssembly, testCases, DiagnosticMessageSink, executionMessageSink, executionOptions);
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomExecutor.Running tests.Before"));
            await assemblyRunner.RunAsync();
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomExecutor.Running tests.After"));
        }
    }
}
