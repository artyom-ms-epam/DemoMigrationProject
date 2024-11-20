using TechTalk.SpecFlow;
using Playwright;

[Binding]
public class TestSteps
{
    [Given("I am on the TestCafe example page")] 
    public async Task GivenIAmOnTheTestCafeExamplePage()
    {
        // Implementation
    }

    [When("I type my name")] 
    public async Task WhenITypeMyName()
    {
        // Implementation
    }

    // ... other steps ...
}