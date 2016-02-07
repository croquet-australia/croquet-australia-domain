Feature: GetEventsAsync

Scenario Outline: Aggregate exists
    Given an aggregateId with <eventCount> events is in the event store
    When I call AzureEventStore.GetEventsAsync(aggregateId)
    Then <eventCount> events should be returned

    Examples:
        | eventCount |
        | 1          |
        | 2          |
        | 3          |
        | 4          |

Scenario: Aggregate does not exists
    Given an aggregatedId that is not in the event store
    When I call AzureEventStore.GetEventsAsync(aggregateId)
    Then AggregateNotFoundException should be thrown