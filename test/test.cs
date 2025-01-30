using System;
using TechTalk.SpecFlow;
using FluentAssertions;
using Microsoft.Playwright;

namespace TestProject
{
    [Binding]
    public class TestSteps
    {
        private IPage _page;
        private IBrowser _browser;
        private IBrowserContext _context;

        [Given(@"I have navigated to the example page")]
        public async Task GivenIHaveNavigatedToTheExamplePage()
        {
            var playwright = await Playwright.CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [When(@"I type the name 'Peter' into the name input")]
        public async Task WhenITypeTheNamePeterIntoTheNameInput()
        {
            await _page.FillAsync("#developer-name", "Peter");
        }

        [Then(@"the name input should contain 'Peter'")]
        public async Task ThenTheNameInputShouldContainPeter()
        {
            var value = await _page.InputValueAsync("#developer-name");
            value.Should().Be("Peter");
        }

        // Additional steps for other tests can be added here following the same pattern.
    }
}
