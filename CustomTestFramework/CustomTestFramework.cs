using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;
namespace CustomTestFrameworkSpace
{
    public class CustomTestFramework : XunitTestFramework
    {
        public CustomTestFramework(IMessageSink messageSink)
            : base(messageSink)
        {
            messageSink.OnMessage(new DiagnosticMessage("Using CustomTestFramework"));
        }
        protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
        {
            var customExecutor = new CustomExecutor(assemblyName, SourceInformationProvider, DiagnosticMessageSink);
            return customExecutor;
        }

        private class CustomExecutor : XunitTestFrameworkExecutor
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
                using var assemblyRunner1 = new CustomAssemblyRunner(TestAssembly, testCases.Take(2), DiagnosticMessageSink, executionMessageSink, executionOptions);
                using var assemblyRunner2 = new CustomAssemblyRunner(TestAssembly, testCases.Skip(2), DiagnosticMessageSink, executionMessageSink, executionOptions);
                DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"Running two first tests..."));
                await assemblyRunner1.RunAsync();
                DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"Two first tests passed"));
                DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"Running rest tests..."));
                await assemblyRunner2.RunAsync();
                DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"Rest tests passed"));
            }
        }

        private class CustomAssemblyRunner : XunitTestAssemblyRunner
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

        private class CustomTestCollectionRunner : XunitTestCollectionRunner
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

        private class CustomTestClassRunner : XunitTestClassRunner
        {
            public CustomTestClassRunner(ITestClass testClass, IReflectionTypeInfo @class, IEnumerable<IXunitTestCase> testCases, IMessageSink diagnosticMessageSink, IMessageBus messageBus, ITestCaseOrderer testCaseOrderer, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource, IDictionary<Type, object> collectionFixtureMappings)
                : base(testClass, @class, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator, cancellationTokenSource, collectionFixtureMappings)
            {
            }

            protected override Task AfterTestClassStartingAsync()
            {
                return base.AfterTestClassStartingAsync();
            }

            protected override Task BeforeTestClassFinishedAsync()
            {
                return base.BeforeTestClassFinishedAsync();
            }

            protected override void CreateClassFixture(Type fixtureType)
            {
                base.CreateClassFixture(fixtureType);
            }

            protected override object[] CreateTestClassConstructorArguments()
            {
                return base.CreateTestClassConstructorArguments();
            }

            protected override string FormatConstructorArgsMissingMessage(ConstructorInfo constructor, IReadOnlyList<Tuple<int, ParameterInfo>> unusedArguments)
            {
                return base.FormatConstructorArgsMissingMessage(constructor, unusedArguments);
            }

            protected override ConstructorInfo SelectTestClassConstructor()
            {
                return base.SelectTestClassConstructor();
            }

            protected override bool TryGetConstructorArgument(ConstructorInfo constructor, int index, ParameterInfo parameter, out object argumentValue)
            {
                return base.TryGetConstructorArgument(constructor, index, parameter, out argumentValue);
            }

            protected override Task<RunSummary> RunTestMethodsAsync()
            {
                return base.RunTestMethodsAsync();
            }

            protected override Task<RunSummary> RunTestMethodAsync(ITestMethod testMethod, IReflectionMethodInfo method, IEnumerable<IXunitTestCase> testCases, object[] constructorArguments)
            {
                var customTestMethodRunner = new CustomTestMethodRunner(testMethod, Class, method, testCases, DiagnosticMessageSink, MessageBus, new ExceptionAggregator(Aggregator), CancellationTokenSource, constructorArguments);
                return customTestMethodRunner.RunAsync();
            }
        }

        private class CustomTestMethodRunner : XunitTestMethodRunner
        {
            private readonly IMessageSink _diagnosticMessageSink;

            public CustomTestMethodRunner(ITestMethod testMethod, IReflectionTypeInfo @class, IReflectionMethodInfo method, IEnumerable<IXunitTestCase> testCases, IMessageSink diagnosticMessageSink, IMessageBus messageBus, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource, object[] constructorArguments)
                : base(testMethod, @class, method, testCases, diagnosticMessageSink, messageBus, aggregator, cancellationTokenSource, constructorArguments)
            {
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

            protected override Task<RunSummary> RunTestCasesAsync()
            {
                return base.RunTestCasesAsync();
            }

            protected override async Task<RunSummary> RunTestCaseAsync(IXunitTestCase testCase)
            {
                var parameters = string.Empty;

                if (testCase.TestMethodArguments != null)
                {
                    parameters = string.Join(", ", testCase.TestMethodArguments.Select(a => a?.ToString() ?? "null"));
                }

                var test = $"{TestMethod.TestClass.Class.Name}.{TestMethod.Method.Name}({parameters})";

                _diagnosticMessageSink.OnMessage(new DiagnosticMessage($"STARTED: {test}"));

                try
                {
                    var result = await base.RunTestCaseAsync(testCase);

                    var status = result.Failed > 0
                        ? "FAILURE"
                        : (result.Skipped > 0 ? "SKIPPED" : "SUCCESS");

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
}