using FluentAssertions;
using NUnit.Framework;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PlaywrightTests
{
    [TestFixture]
    public class ExampleTests
    {
        private IPage _page;
        private IBrowser _browser;
        private IBrowserContext _context;

        [SetUp]
        public async Task SetUp()
        {
            var playwright = await Playwright.CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [TearDown]
        public async Task TearDown()
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
            await nameInput.FillAsync("Parker");
            var value = await nameInput.InputValueAsync();
            value.Should().Be("Parker");
        }

        [Test]
        public async Task ClickArrayOfLabelsAndCheckTheirStates()
        {
            var featureList = _page.Locator(".column.col-2 label");
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
            await nameInput.PressAsync(".");
            await nameInput.PressAsync("Delete");
            await nameInput.PressAsync("Delete");
            await nameInput.PressAsync("Delete");
            value = await nameInput.InputValueAsync();
            value.Should().Be("P. Parker");
        }

        [Test]
        public async Task MovingTheSlider()
        {
            var sliderHandle = _page.Locator("#slider");
            var initialOffset = await sliderHandle.EvaluateAsync<int>("el => el.offsetLeft");
            await _page.ClickAsync("#tried-test-cafe");
            await sliderHandle.DragToAsync(_page.Locator("#slider .ui-slider-handle").Nth(9));
            var newOffset = await sliderHandle.EvaluateAsync<int>("el => el.offsetLeft");
            newOffset.Should().BeGreaterThan(initialOffset);
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
            _page.Dialog += async (sender, dialog) => await dialog.AcceptAsync();
            await _page.ClickAsync("#populate");
            var dialogHistory = await _page.DialogsAsync();
            dialogHistory[0].Message.Should().Be("Reset information before proceeding?");
            await _page.ClickAsync("#submit-button");
            var resultText = await _page.InnerTextAsync("#article-header");
            resultText.Should().Contain("Peter Parker");
        }

        [Test]
        public async Task PickOptionFromSelect()
        {
            var interfaceSelect = _page.Locator("#preferred-interface");
            await interfaceSelect.SelectOptionAsync("Both");
            var value = await interfaceSelect.InputValueAsync();
            value.Should().Be("Both");
        }

        [Test]
        public async Task FillingAForm()
        {
            var nameInput = _page.Locator("#developer-name");
            var macOSRadioButton = _page.Locator("#macos");
            var triedTestCafeCheckbox = _page.Locator("#tried-test-cafe");
            var commentsTextArea = _page.Locator("#comments");
            var submitButton = _page.Locator("#submit-button");
            var results = _page.Locator("#article-header");

            await nameInput.FillAsync("Bruce Wayne");
            await macOSRadioButton.ClickAsync();
            await triedTestCafeCheckbox.ClickAsync();
            await commentsTextArea.FillAsync("It's...");
            await Task.Delay(500);
            await commentsTextArea.FillAsync("It's...\ngood");
            await Task.Delay(500);
            await commentsTextArea.SelectTextAsync(0, 1);
            await commentsTextArea.PressAsync("Delete");
            await commentsTextArea.FillAsync("awesome!!!");
            await Task.Delay(500);
            await submitButton.ClickAsync();
            var resultText = await results.InnerTextAsync();
            resultText.Should().Contain("Bruce Wayne");
        }
    }
}