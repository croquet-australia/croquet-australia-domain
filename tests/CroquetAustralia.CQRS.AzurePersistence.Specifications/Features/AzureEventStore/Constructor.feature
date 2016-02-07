Feature: Constructor

Scenario: Valid parameters
	When I call new AzureEventStore(connectionString, tableNameResolver, eventSerializer)
	Then the object should be created in less than 150ms