using TechTalk.SpecFlow;

namespace CroquetAustralia.Domain.Specifications.Steps
{
    [Binding]
    public class CommonSteps
    {
        [Given(@"the setup procedure has not been run")]
        public void NothingToDo()
        {
            // There is not action to perform. These step simply improve scenario readability.
        }
    }
}