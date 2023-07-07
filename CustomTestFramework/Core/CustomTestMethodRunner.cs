using Xunit.Abstractions;
using Xunit.Sdk;
using Xunit;

namespace CustomTestFramework.Core
{
    public class CustomTestMethodRunner : XunitTestMethodRunner
    {
        private readonly object[] _constructorArguments;
        private readonly IMessageSink _diagnosticMessageSink;

        public CustomTestMethodRunner(ITestMethod testMethod, IReflectionTypeInfo @class, IReflectionMethodInfo method, IEnumerable<IXunitTestCase> testCases, IMessageSink diagnosticMessageSink, IMessageBus messageBus, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource, object[] constructorArguments)
            : base(testMethod, @class, method, testCases, diagnosticMessageSink, messageBus, aggregator, cancellationTokenSource, constructorArguments)
        {
            _constructorArguments = constructorArguments;
            _diagnosticMessageSink = diagnosticMessageSink;
        }

        protected override void AfterTestMethodStarting()
        {
            base.AfterTestMethodStarting();
        }

        protected override void BeforeTestMethodFinished()
        {
            base.BeforeTestMethodFinished();
        }

        protected override async Task<RunSummary> RunTestCasesAsync()
        {
            var disableParallelization = TestMethod.TestClass.Class.GetCustomAttributes(typeof(CollectionAttribute)).Any() ||
                                         TestMethod.Method.GetCustomAttributes(typeof(MemberDataAttribute)).Any(a => a.GetNamedArgument<bool>(nameof(MemberDataAttribute.DisableDiscoveryEnumeration)));

            if (disableParallelization)
                return await base.RunTestCasesAsync().ConfigureAwait(false);

            var summary = new RunSummary();

            var caseTasks = TestCases.Select(RunTestCaseAsync);
            var caseSummaries = await Task.WhenAll(caseTasks).ConfigureAwait(false);

            foreach (var caseSummary in caseSummaries)
            {
                summary.Aggregate(caseSummary);
            }

            return summary;
        }

        protected override async Task<RunSummary> RunTestCaseAsync(IXunitTestCase testCase)
        {
            var args = _constructorArguments.Select(a => a is TestOutputHelper ? new TestOutputHelper() : a).ToArray();

            var action = () => testCase.RunAsync(_diagnosticMessageSink, MessageBus, args, new ExceptionAggregator(Aggregator), CancellationTokenSource);

            var parameters = string.Empty;

            if (testCase.TestMethodArguments != null)
            {
                parameters = string.Join(", ", testCase.TestMethodArguments.Select(a => a?.ToString() ?? "null"));
            }

            var test = $"{TestMethod.TestClass.Class.Name}.{TestMethod.Method.Name}({parameters})";

            _diagnosticMessageSink.OnMessage(new DiagnosticMessage($"STARTED: {test}"));

            RunSummary? result = null;

            try
            {
                if (SynchronizationContext.Current != null)
                {
                    var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
                    result = await Task.Factory.StartNew(action, CancellationTokenSource.Token, TaskCreationOptions.DenyChildAttach | TaskCreationOptions.HideScheduler, scheduler).Unwrap().ConfigureAwait(false);
                }
                else
                {
                    result = await Task.Run(action, CancellationTokenSource.Token).ConfigureAwait(false);
                }

                var status = result.Failed > 0
                    ? "FAILURE"
                    : result.Skipped > 0 ? "SKIPPED" : "SUCCESS";

                _diagnosticMessageSink.OnMessage(new DiagnosticMessage($"{status}: {test} ({result.Time}s)"));

                return result;
            }
            catch (Exception ex)
            {
                _diagnosticMessageSink.OnMessage(new DiagnosticMessage($"ERROR: {test} ({ex.Message})"));
                throw;
            }
        }
    }
}
