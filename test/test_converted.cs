using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Playwright;
using TechTalk.SpecFlow;

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

        [When(@"I type the name 'Peter' in the input field")]
        public async Task WhenITypeTheNamePeterInTheInputField()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync("Peter");
        }

        [Then(@"the input field should contain 'Peter'")]
        public async Task ThenTheInputFieldShouldContainPeter()
        {
            var nameInput = _page.Locator("#developer-name");
            (await nameInput.InputValueAsync()).Should().Be("Peter");
        }

        // Additional steps for other tests
    }
}
