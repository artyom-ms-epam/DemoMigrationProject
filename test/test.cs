using NUnit.Framework;
using FluentAssertions;
using Microsoft.Playwright;
using TechTalk.SpecFlow;

namespace PlaywrightTests
{
    [Binding]
    public class ExampleTests
    {
        private readonly ScenarioContext _scenarioContext;
        private IPage _page;

        public ExampleTests(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public async Task SetUp()
        {
            var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            _page = await browser.NewPageAsync();
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [AfterScenario]
        public async Task TearDown()
        {
            await _page.CloseAsync();
        }

        [Given("I type text into the input field")]
        public async Task GivenITypeTextIntoTheInputField()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync("Peter");
            await nameInput.FillAsync("Paker");
            await nameInput.PressAsync("ArrowLeft");
            await nameInput.TypeAsync("r");
            var value = await nameInput.InputValueAsync();
            value.Should().Be("Parker");
        }

        [Given("I click an array of labels and check their states")]
        public async Task GivenIClickAnArrayOfLabelsAndCheckTheirStates()
        {
            var featureList = _page.Locator(".column.col-2 label");
            for (int i = 0; i < await featureList.CountAsync(); i++)
            {
                var feature = featureList.Nth(i);
                await feature.ClickAsync();
                var checkbox = feature.Locator("input[type=checkbox]");
                (await checkbox.IsCheckedAsync()).Should().BeTrue();
            }
        }

        [Given("I deal with text using keyboard")]
        public async Task GivenIDealWithTextUsingKeyboard()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync("Peter Parker");
            await nameInput.PressAsync("ArrowLeft");
            await nameInput.PressAsync("Backspace");
            var value = await nameInput.InputValueAsync();
            value.Should().Be("Pete Parker");
            await nameInput.PressAsync("Home");
            await nameInput.PressAsync("ArrowRight");
            await nameInput.TypeAsync(".");
            await nameInput.PressAsync("Delete");
            await nameInput.PressAsync("Delete");
            await nameInput.PressAsync("Delete");
            value = await nameInput.InputValueAsync();
            value.Should().Be("P. Parker");
        }

        [Given("I move the slider")]
        public async Task GivenIMoveTheSlider()
        {
            var slider = _page.Locator("#slider");
            var handle = slider.Locator(".ui-slider-handle");
            var initialOffset = await handle.EvaluateAsync<int>("el => el.offsetLeft");
            await _page.CheckAsync("#tried-test-cafe");
            await handle.DragToAsync(slider.Locator(".ui-slider-tick:nth-child(10)"));
            var newOffset = await handle.EvaluateAsync<int>("el => el.offsetLeft");
            newOffset.Should().BeGreaterThan(initialOffset);
        }

        [Given("I deal with text using selection")]
        public async Task GivenIDealWithTextUsingSelection()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync("Test Cafe");
            await nameInput.SelectTextAsync(1, 7);
            await nameInput.PressAsync("Delete");
            var value = await nameInput.InputValueAsync();
            value.Should().Be("Tfe");
        }

        [Given("I handle native confirmation dialog")]
        public async Task GivenIHandleNativeConfirmationDialog()
        {
            _page.Dialog += async (sender, dialog) =>
            {
                await dialog.AcceptAsync();
            };
            await _page.ClickAsync("#populate");
            var dialogHistory = await _page.EvaluateAsync<Dialog[]>("window.__nativeDialogs");
            dialogHistory[0].Message.Should().Be("Reset information before proceeding?");
            await _page.ClickAsync("#submit-button");
            var results = await _page.InnerTextAsync("#article-header");
            results.Should().Contain("Peter Parker");
        }

        [Given("I pick option from select")]
        public async Task GivenIPickOptionFromSelect()
        {
            await _page.SelectOptionAsync("#preferred-interface", "Both");
            var value = await _page.InputValueAsync("#preferred-interface");
            value.Should().Be("Both");
        }

        [Given("I fill a form")]
        public async Task GivenIFillAForm()
        {
            await _page.FillAsync("#developer-name", "Bruce Wayne");
            await _page.CheckAsync("#macos");
            await _page.CheckAsync("#tried-test-cafe");
            await _page.FillAsync("#comments", "It's...");
            await Task.Delay(500);
            await _page.FillAsync("#comments", "\ngood");
            await Task.Delay(500);
            await _page.SelectTextAreaContentAsync("#comments", 1, 0);
            await _page.PressAsync("#comments", "Delete");
            await _page.FillAsync("#comments", "awesome!!!");
            await Task.Delay(500);
            await _page.ClickAsync("#submit-button");
            var results = await _page.InnerTextAsync("#article-header");
            results.Should().Contain("Bruce Wayne");
        }
    }
}
