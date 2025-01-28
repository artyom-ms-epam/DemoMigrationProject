using NUnit.Framework;
using FluentAssertions;
using Microsoft.Playwright;
using TechTalk.SpecFlow;

namespace TestProject
{
    [Binding]
    public class TestSteps
    {
        private readonly IPage _page;
        private readonly PageModel _pageModel;

        public TestSteps(IPage page)
        {
            _page = page;
            _pageModel = new PageModel(_page);
        }

        [Given(@"I am on the example page")]
        public async Task GivenIAmOnTheExamplePage()
        {
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [When(@"I type the name '([^']*)' into the name input")]
        public async Task WhenITypeTheNameIntoTheNameInput(string name)
        {
            await _pageModel.NameInput.FillAsync(name);
        }

        [Then(@"the name input should have the value '([^']*)'")]
        public async Task ThenTheNameInputShouldHaveTheValue(string expectedValue)
        {
            var value = await _pageModel.NameInput.InputValueAsync();
            value.Should().Be(expectedValue);
        }

        // Add more steps as needed for other tests
    }

    public class PageModel
    {
        private readonly IPage _page;

        public PageModel(IPage page)
        {
            _page = page;
        }

        public ILocator NameInput => _page.Locator("#developer-name");
        // Define other elements as needed
    }
}
