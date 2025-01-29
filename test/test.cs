using NUnit.Framework;
using Microsoft.Playwright;
using System.Threading.Tasks;
using FluentAssertions;

namespace PlaywrightTests
{
    public class Tests
    {
        private IPage page;

        [SetUp]
        public async Task Setup()
        {
            var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            var context = await browser.NewContextAsync();
            page = await context.NewPageAsync();
            await page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [Test]
        public async Task TextTypingBasics()
        {
            await page.FillAsync("#developer-name", "Peter");
            await page.FillAsync("#developer-name", "Paker");
            await page.FillAsync("#developer-name", "Parker");
            var value = await page.InputValueAsync("#developer-name");
            value.Should().Be("Parker");
        }

        [Test]
        public async Task ClickAnArrayOfLabelsAndThenCheckTheirStates()
        {
            var features = await page.QuerySelectorAllAsync(".feature");
            foreach (var feature in features)
            {
                await feature.ClickAsync();
                var checkbox = await feature.QuerySelectorAsync("input[type='checkbox']");
                var isChecked = await checkbox.IsCheckedAsync();
                isChecked.Should().BeTrue();
            }
        }

        [Test]
        public async Task DealingWithTextUsingKeyboard()
        {
            await page.FillAsync("#developer-name", "Peter Parker");
            await page.ClickAsync("#developer-name");
            await page.PressAsync("#developer-name", "Backspace");
            var value = await page.InputValueAsync("#developer-name");
            value.Should().Be("Pete Parker");
            await page.PressAsync("#developer-name", "Home");
            await page.PressAsync("#developer-name", "Right");
            await page.PressAsync("#developer-name", ".");
            await page.PressAsync("#developer-name", "Delete");
            await page.PressAsync("#developer-name", "Delete");
            await page.PressAsync("#developer-name", "Delete");
            value = await page.InputValueAsync("#developer-name");
            value.Should().Be("P. Parker");
        }

        [Test]
        public async Task MovingTheSlider()
        {
            var initialOffset = await page.EvalOnSelectorAsync<int>("#slider", "el => el.offsetLeft");
            await page.ClickAsync("#tried-test-cafe");
            await page.DragAndDropAsync("#slider", "#slider .tick:nth-child(9)");
            var newOffset = await page.EvalOnSelectorAsync<int>("#slider", "el => el.offsetLeft");
            newOffset.Should().BeGreaterThan(initialOffset);
        }

        [Test]
        public async Task DealingWithTextUsingSelection()
        {
            await page.FillAsync("#developer-name", "Test Cafe");
            await page.ClickAsync("#developer-name");
            await page.SelectTextAsync("#developer-name", 1, 7);
            await page.PressAsync("#developer-name", "Delete");
            var value = await page.InputValueAsync("#developer-name");
            value.Should().Be("Tfe");
        }

        [Test]
        public async Task HandleNativeConfirmationDialog()
        {
            page.Dialog += (_, dialog) => dialog.AcceptAsync();
            await page.ClickAsync("#populate");
            var dialogHistory = await page.RunAndWaitForDialogAsync(async () => await page.ClickAsync("#populate"));
            dialogHistory.Message.Should().Be("Reset information before proceeding?");
            await page.ClickAsync("#submit-button");
            var result = await page.InnerTextAsync("#result");
            result.Should().Contain("Peter Parker");
        }

        [Test]
        public async Task PickOptionFromSelect()
        {
            await page.SelectOptionAsync("#preferred-interface", "Both");
            var value = await page.InputValueAsync("#preferred-interface");
            value.Should().Be("Both");
        }

        [Test]
        public async Task FillingAForm()
        {
            await page.FillAsync("#developer-name", "Bruce Wayne");
            await page.ClickAsync("#macos");
            await page.ClickAsync("#tried-test-cafe");
            await page.FillAsync("#comments", "It's...");
            await Task.Delay(500);
            await page.FillAsync("#comments", "\ngood");
            await Task.Delay(500);
            await page.SelectTextAsync("#comments", 1, 0);
            await page.PressAsync("#comments", "Delete");
            await page.FillAsync("#comments", "awesome!!!");
            await Task.Delay(500);
            await page.ClickAsync("#submit-button");
            var result = await page.InnerTextAsync("#result");
            result.Should().Contain("Bruce Wayne");
        }
    }
}
