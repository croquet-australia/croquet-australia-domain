Feature: AddEventsAsync

Scenario Outline: Valid parameters
    Given <aggregateEventsCount> aggregateEvents with <eventCount> event
    When I call AzureEventStore.AddEventsAsync(aggregateEventsCollection)
    Then <rowCount> row is added to the aggregate's events table

    Examples:
    | aggregateEventsCount | eventCount | rowCount |
    | 1                    | 1          | 1        |
    | 1                    | 3          | 3        |
    | 4                    | 1          | 4        |
    | 2                    | 4          | 8        |

Scenario: aggregateEventsCollection is empty
    Given aggregateEventsCollection is empty
    When I call AzureEventStore.AddEventsAsync(aggregateEventsCollection)
    Then ArgumentOutOfRangeException should be thrown for aggregateEventsCollection
    And exception message should be 
        """
        Value cannot be empty.
        Parameter name: aggregateEventsCollection
        """