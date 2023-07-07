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
            return base.AfterTestAssemblyStartingAsync();
        }

        protected override Task BeforeTestAssemblyFinishedAsync()
        {
            return base.BeforeTestAssemblyFinishedAsync();
        }

        protected override IMessageBus CreateMessageBus()
        {
            return base.CreateMessageBus();
        }

        protected override string GetTestFrameworkDisplayName()
        {
            return base.GetTestFrameworkDisplayName();
        }

        protected override string GetTestFrameworkEnvironment()
        {
            return base.GetTestFrameworkEnvironment();
        }

        protected override Task<RunSummary> RunTestCollectionsAsync(IMessageBus messageBus, CancellationTokenSource cancellationTokenSource)
        {
            return base.RunTestCollectionsAsync(messageBus, cancellationTokenSource);
        }

        protected override void SetupSyncContext(int maxParallelThreads)
        {
            base.SetupSyncContext(maxParallelThreads);
        }

        protected override Task<RunSummary> RunTestCollectionAsync(IMessageBus messageBus, ITestCollection testCollection, IEnumerable<IXunitTestCase> testCases, CancellationTokenSource cancellationTokenSource)
        {
            var customTestCollectionRunner = new CustomTestCollectionRunner(testCollection, testCases, DiagnosticMessageSink, messageBus, TestCaseOrderer, new ExceptionAggregator(Aggregator), cancellationTokenSource);
            return customTestCollectionRunner.RunAsync();
        }
    }
}
