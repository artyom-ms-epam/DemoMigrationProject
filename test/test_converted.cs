using NUnit.Framework;
using FluentAssertions;
using Microsoft.Playwright;
using TechTalk.SpecFlow;

namespace TestProject
{
    [Binding]
    public class TestSteps
    {
        private readonly Page _page;

        public TestSteps(Page page)
        {
            _page = page;
        }

        [Given(@"I navigate to the test page")]
        public async Task GivenINavigateToTheTestPage()
        {
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [When(@"I type the name 'Peter' into the name input")]
        public async Task WhenITypeTheNameIntoTheNameInput()
        {
            await _page.FillAsync("#developer-name", "Peter");
        }

        [Then(@"the name input should contain 'Peter'")]
        public async Task ThenTheNameInputShouldContain()
        {
            var value = await _page.InputValueAsync("#developer-name");
            value.Should().Be("Peter");
        }

        // Additional steps for other tests...
    }
}
