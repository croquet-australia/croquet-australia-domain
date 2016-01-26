using System;
using System.Linq;
using CroquetAustralia.CQRS;
using CroquetAustralia.Domain.App;
using CroquetAustralia.Domain.Specifications.Helpers;
using CroquetAustralia.Domain.Users;
using FluentAssertions;
using FluentAssertions.Execution;
using OpenMagic;
using TechTalk.SpecFlow;

namespace CroquetAustralia.Domain.Specifications.Steps.Application
{
    [Binding]
    public class SetupSteps
    {
        private readonly ActualData _actual;
        private readonly ICommandBus _commandBus;
        private readonly IEventStore _eventStore;
        private readonly GivenData _given;

        public SetupSteps(TestContext testContext)
            : this(testContext.Given, testContext.Actual, testContext.CommandBus, testContext.EventStore)
        {
        }

        private SetupSteps(GivenData given, ActualData actual, ICommandBus commandBus, IEventStore eventStore)
        {
            _given = given;
            _actual = actual;
            _commandBus = commandBus;
            _eventStore = eventStore;
        }

        [Given(@"the administrator email address is (.*)")]
        public void GivenTheAdministratorEmailAddressIs(string emailAddress)
        {
            _given.EmailAddress = emailAddress;
        }

        [Given(@"the setup procedure has been run")]
        public void GivenTheSetupProcedureHasBeenRun()
        {
            SendSetupCommand(Guid.NewGuid(), RandomString.NextEmailAddress());
        }

        [When(@"I send RunSetup command")]
        public void SendSetupCommand()
        {
            SendSetupCommand(_given.AggregateId, _given.EmailAddress);
        }

        private void SendSetupCommand(Guid aggregateId, string emailAddress)
        {
            _given.Command = new RunSetup(aggregateId, emailAddress);
            _actual.TryCatch(() => _commandBus.SendCommandAsync(_given.Command).Wait());
        }

        [Then(@"the SetupCannotBeRepeatedException should be thrown")]
        public void ThenTheSetupCannotBeRepeatedExceptionShouldBeThrown()
        {
            _actual.Exception.Should().NotBeNull($"because it should be {typeof(SetupCannotBeRepeatedException)}");

            Execute.Assertion
                .ForCondition(_actual.Exception is SetupCannotBeRepeatedException)
                .FailWith($"Expected type to be {typeof(SetupCannotBeRepeatedException)} but found {_actual.Exception.GetType()} with message '{_actual.Exception.Message}'.");
        }

        [Given(@"a random AggregateId")]
        public void GivenARandomAggregateId()
        {
            _given.AggregateId = Guid.NewGuid();
        }

        [Given(@"a random email address")]
        public void GivenARandomEmailAddress()
        {
            _given.EmailAddress = RandomString.NextEmailAddress();
        }

        [Then(@"User\.EmailAddress should be given email address")]
        public void ThenUser_EmailAddressShouldBeGivenEmailAddress()
        {
            _actual.User.EmailAddress.Should().Be(_given.EmailAddress);
        }

        [Then(@"Application aggregate should be added to the event store")]
        public void ThenApplicationAggregateShouldBeAddedToTheEventStore()
        {
            _actual.Application = _eventStore.GetAggregateAsync<App.Application>(_given.AggregateId).Result;
            _actual.Application.Should().NotBeNull();
        }

        [Then(@"Application\.Events should be RanSetup")]
        public void ThenApplication_EventsShouldBeRanSetup()
        {
            var actualEvents = _actual.Application.Events.Cast<RanSetup>();
            var expectedEvents = new[]
            {
                new RanSetup(new RunSetup(_given.AggregateId, _given.EmailAddress))
            };

            actualEvents.ShouldAllBeEquivalentTo(expectedEvents);
        }

        [Then(@"Application\.Id should be given AggregateId")]
        public void ThenApplication_IdShouldBeGivenAggregateId()
        {
            _actual.Application.Id.Should().Be(_given.AggregateId);
        }

        [Then(@"a User aggregate should be added to the event store")]
        public void ThenAUserAggregateShouldBeAddedToTheEventStore()
        {
            _actual.User = _eventStore.GetAggregatesAsync<User>().Result.Single();
            _actual.User.Should().NotBeNull();
        }

        [Then(@"User\.Events should be RegisteredUser")]
        public void ThenUser_EventsShouldBeRegisteredUser()
        {
            _actual.User.Events.Cast<RegisteredUser>().ShouldAllBeEquivalentTo(new[]
            {
                new RegisteredUser(new RegisterUser(_actual.User.Id, _given.EmailAddress))
            });
        }

        [Then(@"User\.Id should not be null or empty")]
        public void ThenUser_IdShouldNotBeNullOrEmpty()
        {
            _actual.User.Id.Should().NotBeEmpty();
        }
    }
}