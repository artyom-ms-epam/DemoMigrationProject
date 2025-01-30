using FluentAssertions;
using TechTalk.SpecFlow;
using System.Threading.Tasks;

[Binding]
public class TestSteps
{
    private readonly Page _page;

    public TestSteps(Page page)
    {
        _page = page;
    }

    [Given(@"I navigate to the TestCafe example page")]
    public async Task GivenINavigateToTheTestCafeExamplePage()
    {
        await _page.GoToAsync("https://devexpress.github.io/testcafe/example/");
    }

    [When(@"I type the name 'Peter' into the name input")]
    public async Task WhenITypeTheNamePeterIntoTheNameInput()
    {
        await _page.TypeTextAsync(_page.NameInput, "Peter");
    }

    [When(@"I replace the name with 'Parker'")]
    public async Task WhenIReplaceTheNameWithParker()
    {
        await _page.TypeTextAsync(_page.NameInput, "Parker", new TypeOptions { Replace = true });
    }

    [When(@"I correct the name to 'Parker'")]
    public async Task WhenICorrectTheNameToParker()
    {
        await _page.TypeTextAsync(_page.NameInput, "r", new TypeOptions { CaretPos = 2 });
    }

    [Then(@"the name input should contain 'Parker'")]
    public async Task ThenTheNameInputShouldContainParker()
    {
        var value = await _page.GetInputValueAsync(_page.NameInput);
        value.Should().Be("Parker");
    }

    [When(@"I click each feature label and check their states")]
    public async Task WhenIClickEachFeatureLabelAndCheckTheirStates()
    {
        foreach (var feature in _page.FeatureList)
        {
            await _page.ClickAsync(feature.Label);
            var isChecked = await _page.IsCheckedAsync(feature.Checkbox);
            isChecked.Should().BeTrue();
        }
    }

    [When(@"I type 'Peter Parker' into the name input and use the keyboard to edit the text")]
    public async Task WhenITypePeterParkerIntoTheNameInputAndUseTheKeyboardToEditTheText()
    {
        await _page.TypeTextAsync(_page.NameInput, "Peter Parker");
        await _page.ClickAsync(_page.NameInput, new ClickOptions { CaretPos = 5 });
        await _page.PressKeyAsync("backspace");
        var value = await _page.GetInputValueAsync(_page.NameInput);
        value.Should().Be("Pete Parker");
        await _page.PressKeyAsync("home right . delete delete delete");
        value = await _page.GetInputValueAsync(_page.NameInput);
        value.Should().Be("P. Parker");
    }

    [When(@"I move the slider to 9")]
    public async Task WhenIMoveTheSliderTo9()
    {
        var initialOffset = await _page.GetOffsetLeftAsync(_page.Slider.Handle);
        await _page.ClickAsync(_page.TriedTestCafeCheckbox);
        await _page.DragToElementAsync(_page.Slider.Handle, _page.Slider.Tick.WithText("9"));
        var newOffset = await _page.GetOffsetLeftAsync(_page.Slider.Handle);
        newOffset.Should().BeGreaterThan(initialOffset);
    }

    [When(@"I type 'Test Cafe' and use selection to delete text")]
    public async Task WhenITypeTestCafeAndUseSelectionToDeleteText()
    {
        await _page.TypeTextAsync(_page.NameInput, "Test Cafe");
        await _page.SelectTextAsync(_page.NameInput, 7, 1);
        await _page.PressKeyAsync("delete");
        var value = await _page.GetInputValueAsync(_page.NameInput);
        value.Should().Be("Tfe");
    }

    [When(@"I handle the native confirmation dialog")]
    public async Task WhenIHandleTheNativeConfirmationDialog()
    {
        await _page.SetNativeDialogHandlerAsync(() => true);
        await _page.ClickAsync(_page.PopulateButton);
        var dialogHistory = await _page.GetNativeDialogHistoryAsync();
        dialogHistory[0].Text.Should().Be("Reset information before proceeding?");
        await _page.ClickAsync(_page.SubmitButton);
        var resultsText = await _page.GetInnerTextAsync(_page.Results);
        resultsText.Should().Contain("Peter Parker");
    }

    [When(@"I pick 'Both' from the interface select")]
    public async Task WhenIPickBothFromTheInterfaceSelect()
    {
        await _page.ClickAsync(_page.InterfaceSelect);
        await _page.ClickAsync(_page.InterfaceSelectOption.WithText("Both"));
        var value = await _page.GetSelectValueAsync(_page.InterfaceSelect);
        value.Should().Be("Both");
    }

    [When(@"I fill the form with 'Bruce Wayne' and submit it")]
    public async Task WhenIFillTheFormWithBruceWayneAndSubmitIt()
    {
        await _page.TypeTextAsync(_page.NameInput, "Bruce Wayne");
        await _page.ClickAsync(_page.MacOSRadioButton);
        await _page.ClickAsync(_page.TriedTestCafeCheckbox);
        await _page.TypeTextAsync(_page.CommentsTextArea, "It's...");
        await Task.Delay(500);
        await _page.TypeTextAsync(_page.CommentsTextArea, "\ngood");
        await Task.Delay(500);
        await _page.SelectTextAreaContentAsync(_page.CommentsTextArea, 1, 0);
        await _page.PressKeyAsync("delete");
        await _page.TypeTextAsync(_page.CommentsTextArea, "awesome!!!");
        await Task.Delay(500);
        await _page.ClickAsync(_page.SubmitButton);
        var resultsText = await _page.GetInnerTextAsync(_page.Results);
        resultsText.Should().Contain("Bruce Wayne");
    }
}
