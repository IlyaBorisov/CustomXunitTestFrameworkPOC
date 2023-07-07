using Xunit.Abstractions;
using Xunit.Sdk;

namespace CustomTestFramework.Core
{
    public class CustomAssemblyRunner : XunitTestAssemblyRunner
    {
        public CustomAssemblyRunner(ITestAssembly testAssembly, IEnumerable<IXunitTestCase> testCases, IMessageSink diagnosticMessageSink, IMessageSink executionMessageSink, ITestFrameworkExecutionOptions executionOptions)
            : base(testAssembly, testCases, diagnosticMessageSink, executionMessageSink, executionOptions)
        {
        }

        protected override Task AfterTestAssemblyStartingAsync()
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomAssemblyRunner.AfterTestAssemblyStartingAsync"));
            return base.AfterTestAssemblyStartingAsync();
        }

        protected override Task BeforeTestAssemblyFinishedAsync()
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomAssemblyRunner.BeforeTestAssemblyFinishedAsync"));
            return base.BeforeTestAssemblyFinishedAsync();
        }

        protected override IMessageBus CreateMessageBus()
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomAssemblyRunner.CreateMessageBus"));
            return base.CreateMessageBus();
        }

        protected override string GetTestFrameworkDisplayName()
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomAssemblyRunner.GetTestFrameworkDisplayName"));
            return base.GetTestFrameworkDisplayName();
        }

        protected override string GetTestFrameworkEnvironment()
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomAssemblyRunner.GetTestFrameworkEnvironment"));
            return base.GetTestFrameworkEnvironment();
        }

        protected override Task<RunSummary> RunTestCollectionsAsync(IMessageBus messageBus, CancellationTokenSource cancellationTokenSource)
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomAssemblyRunner.RunTestCollectionsAsync"));
            return base.RunTestCollectionsAsync(messageBus, cancellationTokenSource);
        }

        protected override void SetupSyncContext(int maxParallelThreads)
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomAssemblyRunner.SetupSyncContext"));
            base.SetupSyncContext(maxParallelThreads);
        }

        protected override Task<RunSummary> RunTestCollectionAsync(IMessageBus messageBus, ITestCollection testCollection, IEnumerable<IXunitTestCase> testCases, CancellationTokenSource cancellationTokenSource)
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomAssemblyRunner.RunTestCollectionAsync"));
            var customTestCollectionRunner = new CustomTestCollectionRunner(testCollection, testCases, DiagnosticMessageSink, messageBus, TestCaseOrderer, new ExceptionAggregator(Aggregator), cancellationTokenSource);
            return customTestCollectionRunner.RunAsync();
        }
    }
}
