Feature: TableNameResolver

Scenario: Use empty constructor
	Given aggregateType is DummyAggregate
	When I call TableNameResolver.GetTableName(aggregateType)
	Then the result should be DummyAggregateEvents

Scenario: Use constructor with tableNameFormat parameter
    Given tableNameFormat is abc{0}xyz
	And aggregateType is DummyAggregate
	When I call TableNameResolver.GetTableName(aggregateType)
	Then the result should be abcDummyAggregatexyz