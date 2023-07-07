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
            return base.CreateDiscoverer();
        }

        public override ITestCase Deserialize(string value)
        {
            return base.Deserialize(value);
        }

        public override void RunTests(IEnumerable<ITestCase> testCases, IMessageSink executionMessageSink, ITestFrameworkExecutionOptions executionOptions)
        {
            base.RunTests(testCases, executionMessageSink, executionOptions);
        }

        public override void RunAll(IMessageSink executionMessageSink, ITestFrameworkDiscoveryOptions discoveryOptions, ITestFrameworkExecutionOptions executionOptions)
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage("Using RunAll"));
            base.RunAll(executionMessageSink, discoveryOptions, executionOptions);
        }

        protected override async void RunTestCases(IEnumerable<IXunitTestCase> testCases, IMessageSink executionMessageSink, ITestFrameworkExecutionOptions executionOptions)
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomExecutor.RunTestCases: Setuping tests: {string.Join(", ", testCases.Select(tc => tc.DisplayName))}"));
            using var assemblyRunner = new CustomAssemblyRunner(TestAssembly, testCases, DiagnosticMessageSink, executionMessageSink, executionOptions);
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"Running tests..."));
            await assemblyRunner.RunAsync();
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"Tests passed"));
        }
    }
}
