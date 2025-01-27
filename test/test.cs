using NUnit.Framework;
using FluentAssertions;
using System.Threading.Tasks;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests
{
    [TestFixture]
    public class Tests : PageTest
    {
        [Test]
        public async Task TextTypingBasics()
        {
            var page = await Browser.NewPageAsync();
            await page.GotoAsync("https://devexpress.github.io/testcafe/example/");
            var nameInput = await page.QuerySelectorAsync("#developer-name");
            await nameInput.TypeAsync("Peter");
            await nameInput.FillAsync("Paker");
            await nameInput.PressAsync("ArrowLeft");
            await nameInput.TypeAsync("r");
            var value = await nameInput.GetAttributeAsync("value");
            value.Should().Be("Parker");
        }

        [Test]
        public async Task ClickArrayOfLabelsAndCheckStates()
        {
            var page = await Browser.NewPageAsync();
            await page.GotoAsync("https://devexpress.github.io/testcafe/example/");
            var features = await page.QuerySelectorAllAsync(".feature");
            foreach (var feature in features)
            {
                var label = await feature.QuerySelectorAsync("label");
                var checkbox = await feature.QuerySelectorAsync("input[type=checkbox]");
                await label.ClickAsync();
                var isChecked = await checkbox.IsCheckedAsync();
                isChecked.Should().BeTrue();
            }
        }

        [Test]
        public async Task DealingWithTextUsingKeyboard()
        {
            var page = await Browser.NewPageAsync();
            await page.GotoAsync("https://devexpress.github.io/testcafe/example/");
            var nameInput = await page.QuerySelectorAsync("#developer-name");
            await nameInput.TypeAsync("Peter Parker");
            await nameInput.PressAsync("ArrowLeft", new PressOptions { Delay = 100 });
            await nameInput.PressAsync("Backspace");
            var value = await nameInput.GetAttributeAsync("value");
            value.Should().Be("Pete Parker");
            await nameInput.PressAsync("Home");
            await nameInput.PressAsync("ArrowRight");
            await nameInput.TypeAsync(".");
            await nameInput.PressAsync("Delete");
            await nameInput.PressAsync("Delete");
            await nameInput.PressAsync("Delete");
            value = await nameInput.GetAttributeAsync("value");
            value.Should().Be("P. Parker");
        }

        [Test]
        public async Task MovingTheSlider()
        {
            var page = await Browser.NewPageAsync();
            await page.GotoAsync("https://devexpress.github.io/testcafe/example/");
            var sliderHandle = await page.QuerySelectorAsync(".ui-slider-handle");
            var initialOffset = await sliderHandle.EvaluateAsync<int>("el => el.offsetLeft");
            await page.ClickAsync("#tried-test-cafe");
            await sliderHandle.DragToAsync(await page.QuerySelectorAsync(".ui-slider-tick:nth-child(10)"));
            var newOffset = await sliderHandle.EvaluateAsync<int>("el => el.offsetLeft");
            newOffset.Should().BeGreaterThan(initialOffset);
        }

        [Test]
        public async Task DealingWithTextUsingSelection()
        {
            var page = await Browser.NewPageAsync();
            await page.GotoAsync("https://devexpress.github.io/testcafe/example/");
            var nameInput = await page.QuerySelectorAsync("#developer-name");
            await nameInput.TypeAsync("Test Cafe");
            await nameInput.SelectTextAsync(1, 7);
            await page.Keyboard.PressAsync("Delete");
            var value = await nameInput.GetAttributeAsync("value");
            value.Should().Be("Tfe");
        }

        [Test]
        public async Task HandleNativeConfirmationDialog()
        {
            var page = await Browser.NewPageAsync();
            await page.GotoAsync("https://devexpress.github.io/testcafe/example/");
            page.Dialog += async (_, dialog) => await dialog.AcceptAsync();
            await page.ClickAsync("#populate");
            var dialogHistory = await page.DialogHistoryAsync();
            dialogHistory[0].Message.Should().Be("Reset information before proceeding?");
            await page.ClickAsync("#submit-button");
            var resultText = await page.InnerTextAsync("#article-header");
            resultText.Should().Contain("Peter Parker");
        }

        [Test]
        public async Task PickOptionFromSelect()
        {
            var page = await Browser.NewPageAsync();
            await page.GotoAsync("https://devexpress.github.io/testcafe/example/");
            var interfaceSelect = await page.QuerySelectorAsync("#preferred-interface");
            await interfaceSelect.ClickAsync();
            await interfaceSelect.SelectOptionAsync(new[] { "Both" });
            var value = await interfaceSelect.GetAttributeAsync("value");
            value.Should().Be("Both");
        }

        [Test]
        public async Task FillingAForm()
        {
            var page = await Browser.NewPageAsync();
            await page.GotoAsync("https://devexpress.github.io/testcafe/example/");
            await page.TypeAsync("#developer-name", "Bruce Wayne");
            await page.ClickAsync("#macos");
            await page.ClickAsync("#tried-test-cafe");
            var commentsTextArea = await page.QuerySelectorAsync("#comments");
            await commentsTextArea.TypeAsync("It's...");
            await Task.Delay(500);
            await commentsTextArea.TypeAsync("\ngood");
            await Task.Delay(500);
            await commentsTextArea.SelectTextAsync(1, 0);
            await page.Keyboard.PressAsync("Delete");
            await commentsTextArea.TypeAsync("awesome!!!");
            await Task.Delay(500);
            await page.ClickAsync("#submit-button");
            var resultText = await page.InnerTextAsync("#article-header");
            resultText.Should().Contain("Bruce Wayne");
        }
    }
}

[Binding]
public class TestSteps
{
    private readonly IPage _page;
    private readonly PageModel _pageModel;

    public TestSteps(IPage page)
    {
        _page = page;
        _pageModel = new PageModel(_page);
    }

    [Given(@"I navigate to the TestCafe example page")]
    public async Task GivenINavigateToTheTestCafeExamplePage()
    {
        await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
    }

    [When(@"I type the name 'Peter' in the name input")]
    public async Task WhenITypeTheNamePeterInTheNameInput()
    {
        await _pageModel.NameInput.FillAsync("Peter");
    }

    [When(@"I replace the name with 'Parker'")]
    public async Task WhenIReplaceTheNameWithParker()
    {
        await _pageModel.NameInput.FillAsync("Parker");
    }

    [When(@"I correct the name to 'Parker'")]
    public async Task WhenICorrectTheNameToParker()
    {
        await _pageModel.NameInput.FillAsync("Parker");
    }

    [Then(@"the name input should contain 'Parker'")]
    public async Task ThenTheNameInputShouldContainParker()
    {
        var value = await _pageModel.NameInput.InputValueAsync();
        value.Should().Be("Parker");
    }

    // Additional steps for other tests...
}

public class PageModel
{
    private readonly IPage _page;

    public PageModel(IPage page)
    {
        _page = page;
    }

    public ILocator NameInput => _page.Locator("#developer-name");
    public ILocator FeatureList => _page.Locator(".feature");
    public ILocator Slider => _page.Locator("#slider");
    public ILocator TriedTestCafeCheckbox => _page.Locator("#tried-test-cafe");
    public ILocator PopulateButton => _page.Locator("#populate");
    public ILocator SubmitButton => _page.Locator("#submit-button");
    public ILocator Results => _page.Locator("#results");
    public ILocator InterfaceSelect => _page.Locator("#preferred-interface");
    public ILocator InterfaceSelectOption => _page.Locator("#preferred-interface option");
    public ILocator CommentsTextArea => _page.Locator("#comments");
    public ILocator MacOSRadioButton => _page.Locator("#macos");
}
