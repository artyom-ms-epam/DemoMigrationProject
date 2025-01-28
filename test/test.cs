using NUnit.Framework;
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

        [Given(@"I navigate to the TestCafe example page")]
        public async Task GivenINavigateToTheTestCafeExamplePage()
        {
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [When(@"I type the name 'Peter' in the name input")]
        public async Task WhenITypeTheNamePeterInTheNameInput()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync("Peter");
        }

        [When(@"I replace the name with 'Parker'")]
        public async Task WhenIReplaceTheNameWithParker()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync("Parker");
        }

        [When(@"I correct the name to 'Parker'")]
        public async Task WhenICorrectTheNameToParker()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.PressAsync("ArrowLeft");
            await nameInput.PressAsync("ArrowLeft");
            await nameInput.PressAsync("ArrowLeft");
            await nameInput.PressAsync("ArrowLeft");
            await nameInput.TypeAsync("r");
        }

        [Then(@"the name input should have the value 'Parker'")]
        public async Task ThenTheNameInputShouldHaveTheValueParker()
        {
            var nameInput = _page.Locator("#developer-name");
            var value = await nameInput.InputValueAsync();
            value.Should().Be("Parker");
        }

        // Additional steps for other tests can be added here...
    }
}
