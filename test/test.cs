using NUnit.Framework;
using Microsoft.Playwright.NUnit;
using FluentAssertions;

namespace TestProject
{
    [TestFixture]
    public class Tests : PageTest
    {
        private Page _page;

        [SetUp]
        public async Task SetUp()
        {
            _page = await Browser.NewPageAsync();
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [Test]
        public async Task TextTypingBasics()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync("Peter");
            await nameInput.FillAsync("Paker");
            await nameInput.FillAsync("Parker");
            (await nameInput.InputValueAsync()).Should().Be("Parker");
        }

        [Test]
        public async Task ClickArrayOfLabelsAndCheckStates()
        {
            var featureList = _page.Locator(".feature");
            for (var i = 0; i < await featureList.CountAsync(); i++)
            {
                var feature = featureList.Nth(i);
                await feature.ClickAsync();
                (await feature.IsCheckedAsync()).Should().BeTrue();
            }
        }

        [Test]
        public async Task DealingWithTextUsingKeyboard()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync("Peter Parker");
            await nameInput.PressAsync("ArrowLeft");
            await nameInput.PressAsync("Backspace");
            (await nameInput.InputValueAsync()).Should().Be("Pete Parker");
            await nameInput.PressAsync("Home");
            await nameInput.PressAsync("ArrowRight");
            await nameInput.PressAsync(".");
            await nameInput.PressAsync("Delete");
            (await nameInput.InputValueAsync()).Should().Be("P. Parker");
        }

        [Test]
        public async Task MovingTheSlider()
        {
            var slider = _page.Locator("#slider");
            var initialOffset = await slider.EvaluateAsync<int>("el => el.offsetLeft");
            await slider.DragToAsync(_page.Locator(".slider-value").WithText("9"));
            var newOffset = await slider.EvaluateAsync<int>("el => el.offsetLeft");
            newOffset.Should().BeGreaterThan(initialOffset);
        }

        [Test]
        public async Task DealingWithTextUsingSelection()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync("Test Cafe");
            await nameInput.SelectTextAsync(1, 7);
            await nameInput.PressAsync("Delete");
            (await nameInput.InputValueAsync()).Should().Be("Tfe");
        }

        [Test]
        public async Task HandleNativeConfirmationDialog()
        {
            _page.Dialog += (_, dialog) => dialog.AcceptAsync();
            await _page.Locator("#populate").ClickAsync();
            var dialogHistory = await _page.Locator("#populate").EvaluateAsync<string[]>("window.dialogHistory");
            dialogHistory[0].Should().Be("Reset information before proceeding?");
            await _page.Locator("#submit-button").ClickAsync();
            (await _page.Locator("#result").InnerTextAsync()).Should().Contain("Peter Parker");
        }

        [Test]
        public async Task PickOptionFromSelect()
        {
            var interfaceSelect = _page.Locator("#preferred-interface");
            await interfaceSelect.SelectOptionAsync(new[] { "Both" });
            (await interfaceSelect.InputValueAsync()).Should().Be("Both");
        }

        [Test]
        public async Task FillingAForm()
        {
            var nameInput = _page.Locator("#developer-name");
            var macOSRadioButton = _page.Locator("#macos");
            var triedTestCafeCheckbox = _page.Locator("#tried-test-cafe");
            var commentsTextArea = _page.Locator("#comments");
            var submitButton = _page.Locator("#submit-button");
            var results = _page.Locator("#result");

            await nameInput.FillAsync("Bruce Wayne");
            await macOSRadioButton.ClickAsync();
            await triedTestCafeCheckbox.ClickAsync();
            await commentsTextArea.FillAsync("It's...");
            await Task.Delay(500);
            await commentsTextArea.FillAsync("\ngood");
            await Task.Delay(500);
            await commentsTextArea.SelectTextAsync(0, 1);
            await commentsTextArea.PressAsync("Delete");
            await commentsTextArea.FillAsync("awesome!!!");
            await Task.Delay(500);
            await submitButton.ClickAsync();
            (await results.InnerTextAsync()).Should().Contain("Bruce Wayne");
        }
    }
}