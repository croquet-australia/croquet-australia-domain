Feature: EventDeserializer

Scenario: Valid arguments
    Given an event
	And the event serialized
	When I call EventDeserializer.Deserializer
	Then the returned event should be equalivent to given event
