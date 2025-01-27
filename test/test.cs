using NUnit.Framework;
using Microsoft.Playwright;
using System.Threading.Tasks;

[TestFixture]
public class TestSuite : PageTest
{
    [Test]
    public async Task TextTypingBasics()
    {
        var page = await Browser.NewPageAsync();
        await page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        var nameInput = page.Locator("#developer-name");
        await nameInput.FillAsync("Peter");
        await nameInput.FillAsync("Parker");
        await nameInput.FillAsync("Parker");
        Assert.AreEqual("Parker", await nameInput.InputValueAsync());
    }

    [Test]
    public async Task ClickArrayOfLabels()
    {
        var page = await Browser.NewPageAsync();
        await page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        var featureList = page.Locator(".feature");
        for (int i = 0; i < await featureList.CountAsync(); i++)
        {
            var feature = featureList.Nth(i);
            await feature.ClickAsync();
            Assert.IsTrue(await feature.Locator("input").IsCheckedAsync());
        }
    }

    [Test]
    public async Task DealingWithTextUsingKeyboard()
    {
        var page = await Browser.NewPageAsync();
        await page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        var nameInput = page.Locator("#developer-name");
        await nameInput.FillAsync("Peter Parker");
        await nameInput.PressAsync("ArrowLeft", new LocatorPressOptions { Position = 5 });
        await nameInput.PressAsync("Backspace");
        Assert.AreEqual("Pete Parker", await nameInput.InputValueAsync());
        await nameInput.PressAsync("Home");
        await nameInput.PressAsync("ArrowRight");
        await nameInput.PressAsync(".");
        await nameInput.PressAsync("Delete");
        await nameInput.PressAsync("Delete");
        await nameInput.PressAsync("Delete");
        Assert.AreEqual("P. Parker", await nameInput.InputValueAsync());
    }

    [Test]
    public async Task MovingTheSlider()
    {
        var page = await Browser.NewPageAsync();
        await page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        var sliderHandle = page.Locator(".ui-slider-handle");
        var initialOffset = await sliderHandle.EvaluateAsync<int>("el => el.offsetLeft");
        await page.Locator("#tried-test-cafe").ClickAsync();
        await sliderHandle.DragToAsync(page.Locator(".ui-slider-tick").WithText("9"));
        var newOffset = await sliderHandle.EvaluateAsync<int>("el => el.offsetLeft");
        Assert.Greater(newOffset, initialOffset);
    }

    [Test]
    public async Task DealingWithTextUsingSelection()
    {
        var page = await Browser.NewPageAsync();
        await page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        var nameInput = page.Locator("#developer-name");
        await nameInput.FillAsync("Test Cafe");
        await nameInput.SelectTextAsync(1, 7);
        await nameInput.PressAsync("Delete");
        Assert.AreEqual("Tfe", await nameInput.InputValueAsync());
    }

    [Test]
    public async Task HandleNativeConfirmationDialog()
    {
        var page = await Browser.NewPageAsync();
        await page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        page.Dialog += async (sender, dialog) => await dialog.AcceptAsync();
        await page.Locator("#populate").ClickAsync();
        var dialogHistory = await page.Locator("#populate").EvaluateAllAsync<string[]>("dialogs => dialogs.map(dialog => dialog.message)");
        Assert.AreEqual("Reset information before proceeding?", dialogHistory[0]);
        await page.Locator("#submit-button").ClickAsync();
        Assert.IsTrue((await page.Locator("#article-header").InnerTextAsync()).Contains("Peter Parker"));
    }

    [Test]
    public async Task PickOptionFromSelect()
    {
        var page = await Browser.NewPageAsync();
        await page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        var interfaceSelect = page.Locator("#preferred-interface");
        await interfaceSelect.SelectOptionAsync("Both");
        Assert.AreEqual("Both", await interfaceSelect.InputValueAsync());
    }

    [Test]
    public async Task FillingForm()
    {
        var page = await Browser.NewPageAsync();
        await page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        await page.Locator("#developer-name").FillAsync("Bruce Wayne");
        await page.Locator("#macos").ClickAsync();
        await page.Locator("#tried-test-cafe").ClickAsync();
        var commentsTextArea = page.Locator("#comments");
        await commentsTextArea.FillAsync("It's...");
        await Task.Delay(500);
        await commentsTextArea.FillAsync("It's...\ngood");
        await Task.Delay(500);
        await commentsTextArea.SelectTextAsync(0, 1);
        await commentsTextArea.PressAsync("Delete");
        await commentsTextArea.FillAsync("awesome!!!");
        await Task.Delay(500);
        await page.Locator("#submit-button").ClickAsync();
        Assert.IsTrue((await page.Locator("#article-header").InnerTextAsync()).Contains("Bruce Wayne"));
    }
}
