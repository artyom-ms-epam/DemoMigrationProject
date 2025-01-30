using System;
using TechTalk.SpecFlow;
using FluentAssertions;
using Microsoft.Playwright;

[Binding]
public class TestConverted
{
    private readonly IPage _page;
    private readonly IPlaywright _playwright;

    public TestConverted(IPage page, IPlaywright playwright)
    {
        _page = page;
        _playwright = playwright;
    }

    [Given("I navigate to the TestCafe example page")]
    public async Task GivenINavigateToTheTestCafeExamplePage()
    {
        await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
    }

    [When("I type the name Peter Parker")]
    public async Task WhenITypeTheNamePeterParker()
    {
        await _page.FillAsync("#developer-name", "Peter Parker");
    }

    [Then("the name input should have the value Parker")]
    public async Task ThenTheNameInputShouldHaveTheValueParker()
    {
        var value = await _page.InputValueAsync("#developer-name");
        value.Should().Be("Parker");
    }

    [When("I click an array of labels")]
    public async Task WhenIClickAnArrayOfLabels()
    {
        var labels = await _page.QuerySelectorAllAsync(".feature label");
        foreach (var label in labels)
        {
            await label.ClickAsync();
            var checkbox = await label.QuerySelectorAsync("input[type='checkbox']");
            var isChecked = await checkbox.IsCheckedAsync();
            isChecked.Should().BeTrue();
        }
    }

    [When("I type and delete text using the keyboard")]
    public async Task WhenITypeAndDeleteTextUsingTheKeyboard()
    {
        await _page.FillAsync("#developer-name", "Peter Parker");
        await _page.ClickAsync("#developer-name", new ClickOptions { Position = new Position { X = 5, Y = 5 } });
        await _page.PressAsync("#developer-name", "Backspace");
        var value = await _page.InputValueAsync("#developer-name");
        value.Should().Be("Pete Parker");
        await _page.PressAsync("#developer-name", "Home Right . Delete Delete Delete");
        value = await _page.InputValueAsync("#developer-name");
        value.Should().Be("P. Parker");
    }

    [When("I move the slider")]
    public async Task WhenIMoveTheSlider()
    {
        var initialOffset = await _page.EvaluateAsync<int>("document.querySelector('#slider').offsetLeft");
        await _page.ClickAsync("#tried-test-cafe");
        await _page.DragToAsync("#slider .ui-slider-handle", "#slider .ui-slider-tick[data-value='9']");
        var newOffset = await _page.EvaluateAsync<int>("document.querySelector('#slider').offsetLeft");
        newOffset.Should().BeGreaterThan(initialOffset);
    }

    [When("I select text and delete")]
    public async Task WhenISelectTextAndDelete()
    {
        await _page.FillAsync("#developer-name", "Test Cafe");
        await _page.SelectTextAsync("#developer-name", 7, 1);
        await _page.PressAsync("#developer-name", "Delete");
        var value = await _page.InputValueAsync("#developer-name");
        value.Should().Be("Tfe");
    }

    [When("I handle a native confirmation dialog")]
    public async Task WhenIHandleANativeConfirmationDialog()
    {
        await _page.DialogAsync += async (_, dialog) => { await dialog.AcceptAsync(); };
        await _page.ClickAsync("#populate");
        var dialogHistory = await _page.EvaluateAsync<string[]>("window.dialogHistory");
        dialogHistory[0].Should().Be("Reset information before proceeding?");
        await _page.ClickAsync("#submit-button");
        var resultText = await _page.InnerTextAsync("#results");
        resultText.Should().Contain("Peter Parker");
    }

    [When("I pick an option from the select")]
    public async Task WhenIPickAnOptionFromTheSelect()
    {
        await _page.ClickAsync("#preferred-interface");
        await _page.ClickAsync("#preferred-interface option[value='Both']");
        var value = await _page.InputValueAsync("#preferred-interface");
        value.Should().Be("Both");
    }

    [When("I fill out the form")]
    public async Task WhenIFillOutTheForm()
    {
        await _page.FillAsync("#developer-name", "Bruce Wayne");
        await _page.ClickAsync("#macos");
        await _page.ClickAsync("#tried-test-cafe");
        await _page.FillAsync("#comments", "It's...");
        await _page.WaitForTimeoutAsync(500);
        await _page.FillAsync("#comments", "\ngood");
        await _page.WaitForTimeoutAsync(500);
        await _page.SelectTextAreaContentAsync("#comments", 1, 0);
        await _page.PressAsync("#comments", "Delete");
        await _page.FillAsync("#comments", "awesome!!!");
        await _page.WaitForTimeoutAsync(500);
        await _page.ClickAsync("#submit-button");
        var resultText = await _page.InnerTextAsync("#results");
        resultText.Should().Contain("Bruce Wayne");
    }
}
