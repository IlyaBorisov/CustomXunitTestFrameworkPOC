using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;
using Xunit;

namespace CustomTestFramework.Core
{
    public class CustomTestClassRunner : XunitTestClassRunner
    {
        public CustomTestClassRunner(ITestClass testClass, IReflectionTypeInfo @class, IEnumerable<IXunitTestCase> testCases, IMessageSink diagnosticMessageSink, IMessageBus messageBus, ITestCaseOrderer testCaseOrderer, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource, IDictionary<Type, object> collectionFixtureMappings)
            : base(testClass, @class, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator, cancellationTokenSource, collectionFixtureMappings)
        {
        }

        protected override Task AfterTestClassStartingAsync()
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomTestClassRunner.AfterTestCollectionStartingAsync"));
            return base.AfterTestClassStartingAsync();
        }

        protected override Task BeforeTestClassFinishedAsync()
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomTestClassRunner.BeforeTestClassFinishedAsync"));
            return base.BeforeTestClassFinishedAsync();
        }

        protected override void CreateClassFixture(Type fixtureType)
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomTestClassRunner.CreateClassFixture"));
            base.CreateClassFixture(fixtureType);
        }

        protected override object[] CreateTestClassConstructorArguments()
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomTestClassRunner.CreateTestClassConstructorArguments"));
            return base.CreateTestClassConstructorArguments();
        }

        protected override string FormatConstructorArgsMissingMessage(ConstructorInfo constructor, IReadOnlyList<Tuple<int, ParameterInfo>> unusedArguments)
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomTestClassRunner.FormatConstructorArgsMissingMessage"));
            return base.FormatConstructorArgsMissingMessage(constructor, unusedArguments);
        }

        protected override ConstructorInfo SelectTestClassConstructor()
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomTestClassRunner.SelectTestClassConstructor"));
            return base.SelectTestClassConstructor();
        }

        protected override bool TryGetConstructorArgument(ConstructorInfo constructor, int index, ParameterInfo parameter, out object argumentValue)
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomTestClassRunner.TryGetConstructorArgument"));
            return base.TryGetConstructorArgument(constructor, index, parameter, out argumentValue);
        }

        protected override async Task<RunSummary> RunTestMethodsAsync()
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomTestClassRunner.RunTestMethodsAsync"));
            var disableParallelization = TestClass.Class.GetCustomAttributes(typeof(CollectionAttribute)).Any();

            if (disableParallelization)
                return await base.RunTestMethodsAsync().ConfigureAwait(false);

            var summary = new RunSummary();
            IEnumerable<IXunitTestCase> orderedTestCases;
            try
            {
                orderedTestCases = TestCaseOrderer.OrderTestCases(TestCases);
            }
            catch (Exception ex)
            {
                var innerEx = Unwrap(ex);
                DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"Test case orderer '{TestCaseOrderer.GetType().FullName}' threw '{innerEx.GetType().FullName}' during ordering: {innerEx.Message}{Environment.NewLine}{innerEx.StackTrace}"));
                orderedTestCases = TestCases.ToList();
            }

            var constructorArguments = CreateTestClassConstructorArguments();
            var methodGroups = orderedTestCases.GroupBy(tc => tc.TestMethod, TestMethodComparer.Instance);
            var methodTasks = methodGroups.Select(m => RunTestMethodAsync(m.Key, (IReflectionMethodInfo)m.Key.Method, m, constructorArguments));
            var methodSummaries = await Task.WhenAll(methodTasks).ConfigureAwait(false);

            foreach (var methodSummary in methodSummaries)
            {
                summary.Aggregate(methodSummary);
            }

            return summary;
        }

        protected override Task<RunSummary> RunTestMethodAsync(ITestMethod testMethod, IReflectionMethodInfo method, IEnumerable<IXunitTestCase> testCases, object[] constructorArguments)
        {
            DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"CustomTestClassRunner.RunTestMethodAsync"));
            return new CustomTestMethodRunner(testMethod, Class, method, testCases, DiagnosticMessageSink, MessageBus, new ExceptionAggregator(Aggregator), CancellationTokenSource, constructorArguments).RunAsync();
        }

        private static Exception Unwrap(Exception ex)
        {
            while (true)
            {
                if (ex is not TargetInvocationException tiex || tiex.InnerException == null)
                    return ex;

                ex = tiex.InnerException;
            }
        }
    }
}
