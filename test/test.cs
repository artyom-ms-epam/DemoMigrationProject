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