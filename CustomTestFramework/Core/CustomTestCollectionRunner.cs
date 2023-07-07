using Xunit.Abstractions;
using Xunit.Sdk;

namespace CustomTestFramework.Core
{
    public class CustomTestCollectionRunner : XunitTestCollectionRunner
    {
        public CustomTestCollectionRunner(ITestCollection testCollection, IEnumerable<IXunitTestCase> testCases, IMessageSink diagnosticMessageSink, IMessageBus messageBus, ITestCaseOrderer testCaseOrderer, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
            : base(testCollection, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator, cancellationTokenSource)
        {
        }

        protected override Task AfterTestCollectionStartingAsync()
        {
            return base.AfterTestCollectionStartingAsync();
        }

        protected override Task BeforeTestCollectionFinishedAsync()
        {
            return base.BeforeTestCollectionFinishedAsync();
        }

        protected override void CreateCollectionFixture(Type fixtureType)
        {
            base.CreateCollectionFixture(fixtureType);
        }

        protected override ITestCaseOrderer GetTestCaseOrderer()
        {
            return base.GetTestCaseOrderer();
        }

        protected override Task<RunSummary> RunTestClassesAsync()
        {
            return base.RunTestClassesAsync();
        }

        protected override Task<RunSummary> RunTestClassAsync(ITestClass testClass, IReflectionTypeInfo @class, IEnumerable<IXunitTestCase> testCases)
        {
            var customTestClassRunner = new CustomTestClassRunner(testClass, @class, testCases, DiagnosticMessageSink, MessageBus, TestCaseOrderer, new ExceptionAggregator(Aggregator), CancellationTokenSource, CollectionFixtureMappings);
            return customTestClassRunner.RunAsync();
        }
    }
}
