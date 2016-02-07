using System;
using System.Linq;
using System.Reflection;
using CroquetAustralia.CQRS.AzurePersistence.Infrastructure;
using CroquetAustralia.CQRS.AzurePersistence.Specifications.TestHelpers;
using TechTalk.SpecFlow;

namespace CroquetAustralia.CQRS.AzurePersistence.Specifications.Steps.Infrastructure
{
    [Binding]
    public class TableNameResolverSteps
    {
        private readonly ActualData _actual;
        private Type _aggregateType;
        private ITableNameResolver _tableNameResolver;

        public TableNameResolverSteps(TestContext testContext)
            : this(testContext.Actual)
        {
        }

        private TableNameResolverSteps(ActualData actual)
        {
            _actual = actual;
            _tableNameResolver = new TableNameResolver();
        }

        [Given(@"tableNameFormat is (.*)")]
        public void GivenTableNameFormatIsAbcXyz(string tableNameFormat)
        {
            _tableNameResolver = new TableNameResolver(tableNameFormat);
        }

        [Given(@"aggregateType is (.*)")]
        public void GivenAggregateTypeIs(string aggregateType)
        {
            _aggregateType = Assembly.GetExecutingAssembly().GetTypes().Single(t => t.Name == aggregateType);
        }

        [When(@"I call TableNameResolver\.GetTableName\(aggregateType\)")]
        public void WhenICallTableNameResolver_GetTableNameAggregateType()
        {
            _actual.Result = _tableNameResolver.GetTableName(_aggregateType);
        }
    }
}