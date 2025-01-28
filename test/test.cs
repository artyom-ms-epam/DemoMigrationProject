using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Playwright;
using TechTalk.SpecFlow;

namespace TestNamespace
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

        [When(@"I type the name 'Peter'")]
        public async Task WhenITypeTheNamePeter()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync("Peter");
        }

        [When(@"I replace it with 'Parker'")]
        public async Task WhenIReplaceItWithParker()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync("Parker");
        }

        [When(@"I correct it to 'Parker'")]
        public async Task WhenICorrectItToParker()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync("Parker");
        }

        [Then(@"the name input should have the value 'Parker'")]
        public async Task ThenTheNameInputShouldHaveTheValueParker()
        {
            var nameInput = _page.Locator("#developer-name");
            var value = await nameInput.InputValueAsync();
            value.Should().Be("Parker");
        }

        // Additional steps for other tests can be added here following the same pattern.
    }
}