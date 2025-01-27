using NUnit.Framework;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PlaywrightTests
{
    public class Tests
    {
        IPage page;

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
            Assert.AreEqual("Parker", value);
        }

        [Test]
        public async Task ClickArrayOfLabels()
        {
            var features = await page.QuerySelectorAllAsync(".column.col-2 label");
            foreach (var feature in features)
            {
                await feature.ClickAsync();
                var checkbox = await feature.QuerySelectorAsync("input[type=checkbox]");
                var isChecked = await checkbox.IsCheckedAsync();
                Assert.IsTrue(isChecked);
            }
        }

        [Test]
        public async Task DealingWithTextUsingKeyboard()
        {
            await page.FillAsync("#developer-name", "Peter Parker");
            await page.ClickAsync("#developer-name", new ClickOptions { Position = new Position { X = 50, Y = 5 } });
            await page.PressAsync("#developer-name", "Backspace");
            var value = await page.InputValueAsync("#developer-name");
            Assert.AreEqual("Pete Parker", value);
            await page.PressAsync("#developer-name", "Home");
            await page.PressAsync("#developer-name", "Right");
            await page.PressAsync("#developer-name", ".");
            await page.PressAsync("#developer-name", "Delete");
            await page.PressAsync("#developer-name", "Delete");
            await page.PressAsync("#developer-name", "Delete");
            value = await page.InputValueAsync("#developer-name");
            Assert.AreEqual("P. Parker", value);
        }

        [Test]
        public async Task MovingTheSlider()
        {
            var initialOffset = await page.EvalOnSelectorAsync<int>("#slider", "el => el.offsetLeft");
            await page.ClickAsync("#tried-test-cafe");
            await page.DragAndDropAsync("#slider", "#slider span:nth-child(10)");
            var newOffset = await page.EvalOnSelectorAsync<int>("#slider", "el => el.offsetLeft");
            Assert.Greater(newOffset, initialOffset);
        }

        [Test]
        public async Task DealingWithTextUsingSelection()
        {
            await page.FillAsync("#developer-name", "Test Cafe");
            await page.FocusAsync("#developer-name");
            await page.EvalOnSelectorAsync("#developer-name", "el => el.setSelectionRange(1, 7)");
            await page.PressAsync("#developer-name", "Delete");
            var value = await page.InputValueAsync("#developer-name");
            Assert.AreEqual("Tfe", value);
        }

        [Test]
        public async Task HandleNativeConfirmationDialog()
        {
            page.Dialog += async (_, dialog) => await dialog.AcceptAsync();
            await page.ClickAsync("#populate");
            var dialogHistory = await page.EvalOnSelectorAllAsync<string[]>("#developer-name", "el => window.dialogHistory");
            Assert.AreEqual("Reset information before proceeding?", dialogHistory[0]);
            await page.ClickAsync("#submit-button");
            var resultText = await page.InnerTextAsync("#article-header");
            Assert.IsTrue(resultText.Contains("Peter Parker"));
        }

        [Test]
        public async Task PickOptionFromSelect()
        {
            await page.ClickAsync("#preferred-interface");
            await page.ClickAsync("#preferred-interface option[value='Both']");
            var value = await page.InputValueAsync("#preferred-interface");
            Assert.AreEqual("Both", value);
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
            await page.EvalOnSelectorAsync("#comments", "el => el.select()");
            await page.PressAsync("#comments", "Delete");
            await page.FillAsync("#comments", "awesome!!!");
            await Task.Delay(500);
            await page.ClickAsync("#submit-button");
            var resultText = await page.InnerTextAsync("#article-header");
            Assert.IsTrue(resultText.Contains("Bruce Wayne"));
        }
    }
}