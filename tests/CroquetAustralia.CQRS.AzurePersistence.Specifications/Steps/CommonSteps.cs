using System;
using CroquetAustralia.CQRS.AzurePersistence.Specifications.TestHelpers;
using CroquetAustralia.CQRS.AzurePersistence.Specifications.TestHelpers.Dummies;
using FluentAssertions;
using OpenMagic.Exceptions;
using TechTalk.SpecFlow;

namespace CroquetAustralia.CQRS.AzurePersistence.Specifications.Steps
{
    [Binding]
    public class CommonSteps
    {
        private readonly ActualData _actual;
        private readonly GivenData _given;
        private readonly TypeResolver _typeResolver;

        public CommonSteps(TestContext testContext)
            : this(testContext.Given, testContext.Actual, testContext.TypeResolver)
        {
        }

        private CommonSteps(GivenData given, ActualData actual, TypeResolver typeResolver)
        {
            _given = given;
            _actual = actual;
            _typeResolver = typeResolver;
        }

        [Given(@"todo")]
        public void GivenTodo()
        {
            throw new ToDoException();
        }

        [Given(@"an aggregatedId that is not in the event store")]
        [Given(@"an aggregateId")]
        public void GivenAnAggregateId()
        {
            _given.AggregateId = Guid.NewGuid();
        }

        [Given(@"an event")]
        public void GivenAnEvent()
        {
            _given.Event = DummyEventWithManyProperties.RandomValue();
        }

        [Given(@"aggregateId is empty")]
        public void GivenAggregateIdIsEmpty()
        {
            _given.AggregateId = Guid.Empty;
        }

        [Then(@"the result should be (.*)")]
        public void ThenTheResultShouldBe(object expectedResult)
        {
            _actual.Result.Should().Be(expectedResult);
        }

        [Then(@"(.*) should be thrown")]
        public void ThenExceptionShouldBeThrown(string exceptionType)
        {
            _actual.Exception.Should().BeOfType(_typeResolver.GetType(exceptionType));
        }

        [Then(@"(.*) should be thrown for (.*)")]
        public void ThenExceptionShouldBeThrownFor(string exceptionType, string paramName)
        {
            var argumentException = (ArgumentException) _actual.Exception;

            argumentException.Should().BeOfType(_typeResolver.GetType(exceptionType));
            argumentException.ParamName.Should().Be(paramName);
        }

        [Then(@"exception message should be")]
        public void ThenExceptionMessageShouldBe(string expectedExceptionMessage)
        {
            _actual.Exception.Message.Should().Be(expectedExceptionMessage);
        }
    }
}