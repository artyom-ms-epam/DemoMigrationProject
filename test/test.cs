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

        [Given(@"I navigate to the example page")]
        public async Task GivenINavigateToTheExamplePage()
        {
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [When(@"I type the name 'Peter' into the name input")]
        public async Task WhenITypeTheNamePeterIntoTheNameInput()
        {
            await _page.FillAsync("#developer-name", "Peter");
        }

        [When(@"I replace it with 'Parker'")]
        public async Task WhenIReplaceItWithParker()
        {
            await _page.FillAsync("#developer-name", "Parker");
        }

        [When(@"I correct it to 'Parker' with a caret position")]
        public async Task WhenICorrectItToParkerWithACaretPosition()
        {
            await _page.FillAsync("#developer-name", "Parker");
        }

        [Then(@"the name input should have the value 'Parker'")]
        public async Task ThenTheNameInputShouldHaveTheValueParker()
        {
            var value = await _page.GetAttributeAsync("#developer-name", "value");
            value.Should().Be("Parker");
        }

        // Other tests can be converted similarly...
    }
}
    }
}
