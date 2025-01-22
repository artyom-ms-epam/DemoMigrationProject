using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using TechTalk.SpecFlow;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace TestProject
{
    [Binding]
    public class TestSteps
    {
        private IPage _page;

        public TestSteps(IPage page)
        {
            _page = page;
        }

        [Given(@"I navigate to the test page")]
        public async Task GivenINavigateToTheTestPage()
        {
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [When(@"I type the name 'Peter' into the name input")]
        public async Task WhenITypeTheNamePeterIntoTheNameInput()
        {
            await _page.FillAsync("#developer-name", "Peter");
        }

        [When(@"I replace the name with 'Parker'")]
        public async Task WhenIReplaceTheNameWithParker()
        {
            await _page.FillAsync("#developer-name", "Parker");
        }

        [When(@"I correct the name to 'Parker'")]
        public async Task WhenICorrectTheNameToParker()
        {
            await _page.FillAsync("#developer-name", "Parker");
        }

        [Then(@"the name input should contain 'Parker'")]
        public async Task ThenTheNameInputShouldContainParker()
        {
            var value = await _page.InputValueAsync("#developer-name");
            value.Should().Be("Parker");
        }

        // Additional steps for other tests can be added here following the same pattern.

        [When(@"I type '([^']*)' into the name input")]
        public async Task WhenITypeIntoTheNameInput(string text)
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync(text);
        }

        [Then(@"the name input should have value '([^']*)'")]
        public async Task ThenTheNameInputShouldHaveValue(string expectedValue)
        {
            var nameInput = _page.Locator("#developer-name");
            var value = await nameInput.InputValueAsync();
            value.Should().Be(expectedValue);
        }
    }
}
