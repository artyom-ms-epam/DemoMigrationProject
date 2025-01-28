using NUnit.Framework;
using Microsoft.Playwright;
using FluentAssertions;
using System.Threading.Tasks;

namespace PlaywrightTests
{
    public class Tests
    {
        private IPage _page;
        private IBrowser _browser;
        private IBrowserContext _context;

        [SetUp]
        public async Task Setup()
        {
            var playwright = await Playwright.CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [TearDown]
        public async Task Teardown()
        {
            await _context.CloseAsync();
            await _browser.CloseAsync();
        }

        [Test]
        public async Task TextTypingBasics()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync("Peter");
            await nameInput.FillAsync("Paker");
            await nameInput.PressAsync("ArrowLeft");
            await nameInput.TypeAsync("r");
            var value = await nameInput.InputValueAsync();
            value.Should().Be("Parker");
        }

        [Test]
        public async Task ClickArrayOfLabelsAndCheckStates()
        {
            var featureList = _page.Locator(".feature");
            var count = await featureList.CountAsync();
            for (int i = 0; i < count; i++)
            {
                var feature = featureList.Nth(i);
                await feature.ClickAsync();
                var checkbox = feature.Locator("input[type=checkbox]");
                var isChecked = await checkbox.IsCheckedAsync();
                isChecked.Should().BeTrue();
            }
        }

        [Test]
        public async Task DealingWithTextUsingKeyboard()
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

        [Test]
        public async Task MovingTheSlider()
        {
            var slider = _page.Locator("#slider");
            var handle = slider.Locator(".ui-slider-handle");
            var initialOffset = await handle.BoundingBoxAsync();
            await _page.CheckAsync("#tried-test-cafe");
            await handle.DragToAsync(slider.Locator(".ui-slider-tick:nth-child(10)"));
            var newOffset = await handle.BoundingBoxAsync();
            newOffset.X.Should().BeGreaterThan(initialOffset.X);
        }

        [Test]
        public async Task DealingWithTextUsingSelection()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync("Test Cafe");
            await nameInput.SelectTextAsync(1, 7);
            await nameInput.PressAsync("Delete");
            var value = await nameInput.InputValueAsync();
            value.Should().Be("Tfe");
        }

        [Test]
        public async Task HandleNativeConfirmationDialog()
        {
            _page.Dialog += async (_, dialog) => await dialog.AcceptAsync();
            await _page.ClickAsync("#populate");
            var dialogHistory = await _page.EvaluateAsync<string[]>("window.__nativeDialogHistory");
            dialogHistory[0].Should().Be("Reset information before proceeding?");
            await _page.ClickAsync("#submit-button");
            var results = await _page.InnerTextAsync("#results");
            results.Should().Contain("Peter Parker");
        }

        [Test]
        public async Task PickOptionFromSelect()
        {
            var interfaceSelect = _page.Locator("#preferred-interface");
            await interfaceSelect.SelectOptionAsync(new SelectOptionValue { Label = "Both" });
            var value = await interfaceSelect.InputValueAsync();
            value.Should().Be("Both");
        }

        [Test]
        public async Task FillingForm()
        {
            await _page.FillAsync("#developer-name", "Bruce Wayne");
            await _page.CheckAsync("#macos");
            await _page.CheckAsync("#tried-test-cafe");
            var commentsTextArea = _page.Locator("#comments");
            await commentsTextArea.FillAsync("It's...");
            await Task.Delay(500);
            await commentsTextArea.TypeAsync("\ngood");
            await Task.Delay(500);
            await commentsTextArea.SelectTextAsync(1, 0);
            await commentsTextArea.PressAsync("Delete");
            await commentsTextArea.TypeAsync("awesome!!!");
            await Task.Delay(500);
            await _page.ClickAsync("#submit-button");
            var results = await _page.InnerTextAsync("#results");
            results.Should().Contain("Bruce Wayne");
        }
    }
}