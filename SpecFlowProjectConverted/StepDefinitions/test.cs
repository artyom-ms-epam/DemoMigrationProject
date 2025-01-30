using FluentAssertions;
using TechTalk.SpecFlow;
using Microsoft.Playwright;

namespace SpecFlowProjectConverted.StepDefinitions
{
    [Binding]
    public class TestSteps
    {
        private readonly IPage _page;

        public TestSteps(IPage page)
        {
            _page = page;
        }

        [Given(@"I navigate to the example page")]
        public async Task GivenINavigateToTheExamplePage()
        {
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [When(@"I type the name 'Peter' into the name input")]
        public async Task WhenITypeTheNamePeterIntoTheNameInput()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync("Peter");
        }

        [Then(@"the name input should have the value 'Peter'")]
        public async Task ThenTheNameInputShouldHaveTheValuePeter()
        {
            var nameInput = _page.Locator("#developer-name");
            var value = await nameInput.InputValueAsync();
            value.Should().Be("Peter");
        }

        // Add other steps here following the same pattern
    }
}
