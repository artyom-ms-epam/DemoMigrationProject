using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using TechTalk.SpecFlow;

namespace TestProject
{
    [Binding]
    public class TestSteps
    {
        private IPage _page;
        private readonly IBrowser _browser;
        private readonly IBrowserContext _context;

        public TestSteps(IBrowser browser, IBrowserContext context)
        {
            _browser = browser;
            _context = context;
        }

        [Given(@"I type name '([^']*)' in the input field")]
        public async Task GivenITypeNameInTheInputField(string name)
        {
            await _page.FillAsync("#developer-name", name);
        }

        [Then(@"the input field should contain '([^']*)'")]
        public async Task ThenTheInputFieldShouldContain(string expectedName)
        {
            var value = await _page.InputValueAsync("#developer-name");
            value.Should().Be(expectedName);
        }

        [When(@"I click on '([^']*)' checkbox")]
        public async Task WhenIClickOnCheckbox(string checkboxId)
        {
            await _page.CheckAsync($"#{checkboxId}");
        }

        [Then(@"the '([^']*)' checkbox should be checked")]
        public async Task ThenTheCheckboxShouldBeChecked(string checkboxId)
        {
            var isChecked = await _page.IsCheckedAsync($"#{checkboxId}");
            isChecked.Should().BeTrue();
        }

        [When(@"I move the slider to '([^']*)'")]
        public async Task WhenIMoveTheSliderTo(string value)
        {
            await _page.FillAsync("#slider", value);
        }

        [Then(@"the slider should have value '([^']*)'")]
        public async Task ThenTheSliderShouldHaveValue(string expectedValue)
        {
            var value = await _page.InputValueAsync("#slider");
            value.Should().Be(expectedValue);
        }

        [When(@"I select '([^']*)' from the dropdown")]
        public async Task WhenISelectFromTheDropdown(string option)
        {
            await _page.SelectOptionAsync("#preferred-interface", new[] { option });
        }

        [Then(@"the dropdown should have '([^']*)' selected")]
        public async Task ThenTheDropdownShouldHaveSelected(string expectedOption)
        {
            var selectedOption = await _page.InputValueAsync("#preferred-interface");
            selectedOption.Should().Be(expectedOption);
        }

        [When(@"I fill the comments with '([^']*)'")]
        public async Task WhenIFillTheCommentsWith(string comments)
        {
            await _page.FillAsync("#comments", comments);
        }

        [Then(@"the comments should contain '([^']*)'")]
        public async Task ThenTheCommentsShouldContain(string expectedComments)
        {
            var value = await _page.InputValueAsync("#comments");
            value.Should().Contain(expectedComments);
        }

        [When(@"I submit the form")]
        public async Task WhenISubmitTheForm()
        {
            await _page.ClickAsync("#submit-button");
        }

        [Then(@"the results should contain '([^']*)'")]
        public async Task ThenTheResultsShouldContain(string expectedResults)
        {
            var results = await _page.InnerTextAsync("#article-header");
            results.Should().Contain(expectedResults);
        }

        [BeforeScenario]
        public async Task BeforeScenario()
        {
            _page = await _context.NewPageAsync();
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            await _page.CloseAsync();
        }

        [Given(@"I type 'Peter' into the name input")]
        public async Task GivenITypePeterIntoTheNameInput()
        {
            await _page.FillAsync("#developer-name", "Peter");
        }

        [When(@"I replace it with 'Parker'")]
        public async Task WhenIReplaceItWithParker()
        {
            await _page.FillAsync("#developer-name", "Parker");
        }

        [Then(@"The name input should contain 'Parker'")]
        public async Task ThenTheNameInputShouldContainParker()
        {
            var value = await _page.InputValueAsync("#developer-name");
            value.Should().Be("Parker");
        }
    }
}