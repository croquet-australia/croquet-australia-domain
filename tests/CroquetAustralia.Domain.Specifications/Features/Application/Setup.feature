Feature: Setup
	As an administrator
	I want to run the setup procedure
	So the site can be used

Scenario: Calling setup procedure for the first time
	Given the setup procedure has not been run
    And a random AggregateId
    And a random email address
    When I send RunSetup command
    Then Application aggregate should be added to the event store
    And Application.Events should be RanSetup
    And Application.Id should be given AggregateId
    And a User aggregate should be added to the event store
    And User.Events should be RegisteredUser
    And User.Id should not be null or empty
    And User.EmailAddress should be given email address

Scenario: Calling setup procedure when setup has previously been called
	Given the setup procedure has been run
    And a random AggregateId
    And a random email address
    When I send RunSetup command
    Then the SetupCannotBeRepeatedException should be thrown