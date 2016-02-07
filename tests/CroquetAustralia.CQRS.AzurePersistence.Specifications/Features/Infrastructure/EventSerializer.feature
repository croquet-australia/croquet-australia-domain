Feature: EventSerializer

Scenario: Valid arguments
	Given an aggregateId
	And an event
	When I call EventSerializer.Serialize(aggregateId, event)
	Then the result should be a DynamicTableEntity
    And PartitionKey should be aggregateId
    And RowKey should be sortable and unique
    And there should be DynamicTableEntity property __eventType
    And DynamicTableEntity[__eventType] should be event type
    And there should be DynamicTableEntity property for each Event property

Scenario: aggregateId is empty
	Given aggregateId is empty
	And an event
	When I call EventSerializer.Serialize(aggregateId, event)
    Then ArgumentOutOfRangeException should be thrown for aggregateId