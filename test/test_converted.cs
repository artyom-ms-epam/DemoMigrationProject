using FluentAssertions;
using TechTalk.SpecFlow;
using Microsoft.Playwright;

namespace TestProject
{
    [Binding]
    public class TestSteps
    {
        private readonly IPage _page;

        public TestSteps(IPage page)
        {
            _page = page;
        }

        [Given(@"I navigate to the test page")]
        public async Task GivenINavigateToTheTestPage()
        {
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [When(@"I type the name 'Peter' in the name input")]
        public async Task WhenITypeTheNamePeterInTheNameInput()
        {
            var nameInput = await _page.QuerySelectorAsync("#developer-name");
            await nameInput.TypeAsync("Peter");
        }

        [When(@"I replace the name with 'Parker'")]
        public async Task WhenIReplaceTheNameWithParker()
        {
            var nameInput = await _page.QuerySelectorAsync("#developer-name");
            await nameInput.FillAsync("Parker");
        }

        [When(@"I correct the name to 'Parker' by typing 'r' at position 2")]
        public async Task WhenICorrectTheNameToParkerByTypingRAtPosition2()
        {
            var nameInput = await _page.QuerySelectorAsync("#developer-name");
            await nameInput.PressAsync("ArrowLeft");
            await nameInput.TypeAsync("r");
        }

        [Then(@"the name input should have the value 'Parker'")]
        public async Task ThenTheNameInputShouldHaveTheValueParker()
        {
            var nameInput = await _page.QuerySelectorAsync("#developer-name");
            var value = await nameInput.GetAttributeAsync("value");
            value.Should().Be("Parker");
        }

        // Additional steps for other tests can be added here following the same pattern
    }
}
