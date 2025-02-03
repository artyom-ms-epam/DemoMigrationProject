using NUnit.Framework;
using FluentAssertions;
using TechTalk.SpecFlow;
using Microsoft.Playwright;

namespace SpecFlowProjectConverted.StepDefinitions
{
    [Binding]
    public class TestSteps
    {
        private readonly IPage _page;
        private readonly IBrowser _browser;
        private readonly IBrowserContext _context;

        public TestSteps(IPage page, IBrowser browser, IBrowserContext context)
        {
            _page = page;
            _browser = browser;
            _context = context;
        }

        [Given(@"I navigate to the example page")]
        public async Task GivenINavigateToTheExamplePage()
        {
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [When(@"I type the name '([^']*)' into the input field")]
        public async Task WhenITypeTheNameIntoTheInputField(string name)
        {
            await _page.FillAsync("#developer-name", name);
        }

        [Then(@"the input field should contain '([^']*)'")]
        public async Task ThenTheInputFieldShouldContain(string expectedName)
        {
            var value = await _page.InputValueAsync("#developer-name");
            value.Should().Be(expectedName);
        }

        [When(@"I click the '([^']*)' checkbox")]
        public async Task WhenIClickTheCheckbox(string label)
        {
            var checkbox = await _page.QuerySelectorAsync($"label:has-text('{label}')");
            await checkbox.ClickAsync();
        }

        [Then(@"the '([^']*)' checkbox should be checked")]
        public async Task ThenTheCheckboxShouldBeChecked(string label)
        {
            var checkbox = await _page.QuerySelectorAsync($"label:has-text('{label}') input");
            var isChecked = await checkbox.IsCheckedAsync();
            isChecked.Should().BeTrue();
        }

        [When(@"I move the slider to '([^']*)'")]
        public async Task WhenIMoveTheSliderTo(string value)
        {
            var slider = await _page.QuerySelectorAsync("#slider");
            await slider.DragToAsync(value);
        }

        [Then(@"the slider should be at '([^']*)'")]
        public async Task ThenTheSliderShouldBeAt(string expectedValue)
        {
            var value = await _page.InputValueAsync("#slider");
            value.Should().Be(expectedValue);
        }

        [When(@"I select '([^']*)' from the dropdown")]
        public async Task WhenISelectFromTheDropdown(string option)
        {
            await _page.SelectOptionAsync("#preferred-interface", option);
        }

        [Then(@"the dropdown should have '([^']*)' selected")]
        public async Task ThenTheDropdownShouldHaveSelected(string expectedOption)
        {
            var value = await _page.InputValueAsync("#preferred-interface");
            value.Should().Be(expectedOption);
        }

        [When(@"I fill the comments with '([^']*)'")]
        public async Task WhenIFillTheCommentsWith(string comments)
        {
            await _page.FillAsync("#comments", comments);
        }

        [Then(@"the comments should be '([^']*)'")]
        public async Task ThenTheCommentsShouldBe(string expectedComments)
        {
            var value = await _page.InputValueAsync("#comments");
            value.Should().Be(expectedComments);
        }

        [When(@"I submit the form")]
        public async Task WhenISubmitTheForm()
        {
            await _page.ClickAsync("#submit-button");
        }

        [Then(@"the result should contain '([^']*)'")]
        public async Task ThenTheResultShouldContain(string expectedResult)
        {
            var resultText = await _page.InnerTextAsync("#result-content");
            resultText.Should().Contain(expectedResult);
        }
    }
}
