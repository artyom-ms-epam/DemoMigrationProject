using FluentAssertions;
using TechTalk.SpecFlow;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SpecFlowProjectConverted.StepDefinitions
{
    [Binding]
    public class TestSteps
    {
        private readonly IPage _page;
        private readonly Page _pageModel;

        public TestSteps(IPage page)
        {
            _page = page;
            _pageModel = new Page(_page);
        }

        [Given(@"I navigate to the example page")]
        public async Task GivenINavigateToTheExamplePage()
        {
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [When(@"I type 'Peter' into the name input")]
        public async Task WhenITypePeterIntoTheNameInput()
        {
            await _pageModel.NameInput.FillAsync("Peter");
        }

        [When(@"I replace the name with 'Parker'")]
        public async Task WhenIReplaceTheNameWithParker()
        {
            await _pageModel.NameInput.FillAsync("Parker");
        }

        [When(@"I correct the name to 'Parker'")]
        public async Task WhenICorrectTheNameToParker()
        {
            await _pageModel.NameInput.FillAsync("Parker");
        }

        [Then(@"the name input should have 'Parker'")]
        public async Task ThenTheNameInputShouldHaveParker()
        {
            var value = await _pageModel.NameInput.InputValueAsync();
            value.Should().Be("Parker");
        }

        // Other tests can be converted similarly...
    }
}
