using System;
using TechTalk.SpecFlow;
using FluentAssertions;
using Microsoft.Playwright;

[Binding]
public class TestSteps
{
    private readonly ScenarioContext _scenarioContext;
    private IPage _page;

    public TestSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"I navigate to the TestCafe example page")]
    public async Task GivenINavigateToTheTestCafeExamplePage()
    {
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        _page = await browser.NewPageAsync();
        await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
    }

    [When(@"I type the name 'Peter' in the input field")]
    public async Task WhenITypeTheNamePeterInTheInputField()
    {
        await _page.FillAsync("#developer-name", "Peter");
    }

    [Then(@"the input field should contain 'Peter'")]
    public async Task ThenTheInputFieldShouldContainPeter()
    {
        var value = await _page.InputValueAsync("#developer-name");
        value.Should().Be("Peter");
    }

    // Additional steps can be added here following the same pattern
}
