using FluentAssertions;
using TechTalk.SpecFlow;
using System.Threading.Tasks;

namespace SpecFlowProjectConverted.StepDefinitions
{
    [Binding]
    public class TestSteps
    {
        private readonly Page _page;

        public TestSteps(Page page)
        {
            _page = page;
        }

        [Given("I navigate to the example page")]
        public async Task GivenINavigateToTheExamplePage()
        {
            await _page.GoToAsync("https://devexpress.github.io/testcafe/example/");
        }

        [When("I type the name 'Peter' in the name input")]
        public async Task WhenITypeTheNamePeterInTheNameInput()
        {
            await _page.TypeTextAsync(_page.NameInput, "Peter");
        }

        [When("I replace the name with 'Parker'")]
        public async Task WhenIReplaceTheNameWithParker()
        {
            await _page.TypeTextAsync(_page.NameInput, "Parker", replace: true);
        }

        [When("I correct the name to 'Parker'")]
        public async Task WhenICorrectTheNameToParker()
        {
            await _page.TypeTextAsync(_page.NameInput, "r", caretPos: 2);
        }

        [Then("the name input should contain 'Parker'")]
        public void ThenTheNameInputShouldContainParker()
        {
            _page.NameInput.Value.Should().Be("Parker");
        }

        [When("I click on each feature label")]
        public async Task WhenIClickOnEachFeatureLabel()
        {
            foreach (var feature in _page.FeatureList)
            {
                await _page.ClickAsync(feature.Label);
                feature.Checkbox.Checked.Should().BeTrue();
            }
        }

        [When("I type 'Peter Parker' and edit it using keyboard")]
        public async Task WhenITypePeterParkerAndEditItUsingKeyboard()
        {
            await _page.TypeTextAsync(_page.NameInput, "Peter Parker");
            await _page.ClickAsync(_page.NameInput, caretPos: 5);
            await _page.PressKeyAsync("backspace");
            _page.NameInput.Value.Should().Be("Pete Parker");
            await _page.PressKeyAsync("home right . delete delete delete");
            _page.NameInput.Value.Should().Be("P. Parker");
        }

        [When("I move the slider")]
        public async Task WhenIMoveTheSlider()
        {
            var initialOffset = await _page.Slider.Handle.OffsetLeftAsync();
            await _page.ClickAsync(_page.TriedTestCafeCheckbox);
            await _page.DragToElementAsync(_page.Slider.Handle, _page.Slider.Tick.WithText("9"));
            (await _page.Slider.Handle.OffsetLeftAsync()).Should().BeGreaterThan(initialOffset);
        }

        [When("I select text and delete it")]
        public async Task WhenISelectTextAndDeleteIt()
        {
            await _page.TypeTextAsync(_page.NameInput, "Test Cafe");
            await _page.SelectTextAsync(_page.NameInput, 7, 1);
            await _page.PressKeyAsync("delete");
            _page.NameInput.Value.Should().Be("Tfe");
        }

        [When("I handle the native confirmation dialog")]
        public async Task WhenIHandleTheNativeConfirmationDialog()
        {
            await _page.SetNativeDialogHandlerAsync(() => true);
            await _page.ClickAsync(_page.PopulateButton);
            var dialogHistory = await _page.GetNativeDialogHistoryAsync();
            dialogHistory[0].Text.Should().Be("Reset information before proceeding?");
            await _page.ClickAsync(_page.SubmitButton);
            _page.Results.InnerText.Should().Contain("Peter Parker");
        }

        [When("I pick an option from the select")]
        public async Task WhenIPickAnOptionFromTheSelect()
        {
            await _page.ClickAsync(_page.InterfaceSelect);
            await _page.ClickAsync(_page.InterfaceSelectOption.WithText("Both"));
            _page.InterfaceSelect.Value.Should().Be("Both");
        }

        [When("I fill the form")]
        public async Task WhenIFillTheForm()
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
            _page.Results.InnerText.Should().Contain("Bruce Wayne");
        }
    }
}
}