Feature: GetAllAsync

Scenario: Event store is empty
	Given the event store is empty
    When I call AzureEventStore\.GetAllAsync\()
    Then an empty dictionary is returned

Scenario: Event store is populated
    Given the event store has
        | id     | eventCount |
        | random | 2          |
        | random | 5          |
        | random | 12         |
        | random | 100        |
        | random | 1022       |
        | random | 7          |
    When I call AzureEventStore\.GetAllAsync\()
    Then 6 IAggregateEvents should be returned
    And the number events in each IAggregateEvents should equal given eventCount
