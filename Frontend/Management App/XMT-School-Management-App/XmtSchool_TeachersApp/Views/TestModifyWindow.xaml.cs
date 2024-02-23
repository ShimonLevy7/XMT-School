using System;
using System.Collections.Generic;
using System.Windows;
using XmtSchool_TeachersApp.Models;
using XmtSchoolWebApi.Types;
using XmtSchoolTypes.Tests;
using XmtSchool_TeachersApp.Cache;
using static XmtSchoolWebApi.Types.ResultTypes.TestsResultTypes;
using System.Windows.Controls;

namespace XmtSchool_TeachersApp.Views
{
    /// <summary>
    /// Interaction logic for TestModifyWindow.xaml
    /// </summary>
    public partial class TestModifyWindow : Window
    {
        private readonly TestAction ActionType;

        public TestWithAuthorName TestData { get; private set; }

        public TestModifyWindow(TestAction actionType, TestWithAuthorName testData)
        {
            InitializeComponent();

            ActionType = actionType;
            TestData = testData;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title_TextBox.Focus();

            try
            {
                if (ActionType == TestAction.Create)
                {
                    Title = "XMT School - Management App - Create a new test";

                    Confirm_Button.Content = "Create";

                    return;
                }

                if (TestData == null)
                    throw new Exception("Action type is Update but Test was not specified");

                Title = $"XMT School - Management App - Update test '{TestData.Test.Name}'";
                Title_TextBox.Text = TestData.Test.Name;
                StartDate.SelectedDate = TestData.Test.StartDate;
                EndDate.SelectedDate = TestData.Test.EndDate;

                Confirm_Button.Content = "Update";

                RandomizeAnswers.IsChecked = TestData.Test.RandomiseAnswersOrder;
                RandomizeQuestions.IsChecked = TestData.Test.RandomiseQuestionsOrder;
            }
            finally
            {
                InitQuestionsList();
            }
        }

        /// <summary>
        /// Initializes loading of questions in a list.
        /// </summary>
        /// <param name="fetchNewData"></param>
        /// <returns></returns>
        private void InitQuestionsList()
        {
            // Retrieve the updated list of tests from the cache.
            List<Question>? questions = TestData.Test.Questions;

            // Assign the tests list as the data source for the TestList UI element.
            QuestionsList.ItemsSource = questions;

            if (QuestionsList.View is GridView gv)
            {
                foreach (GridViewColumn? c in gv.Columns)
                {
                    if (double.IsNaN(c.Width))
                    {
                        c.Width = ActualWidth;
                    }

                    c.Width = double.NaN;
                }
            }
        }

        #region Context Menu - Questions

        /// <summary>
        /// Handles the 'Add Question' context button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextQuestionAdd_Click(object sender, RoutedEventArgs e)
        {
            AddQuestion();
        }

        /// <summary>
        /// Handles the 'Remove Question' context button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextQuestionRemove_Click(object sender, RoutedEventArgs e)
        {
            RemoveQuestion();
        }

        /// <summary>
        /// Handles the 'Edit Question' context button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextQuestionEdit_Click(object sender, RoutedEventArgs e)
        {
            UpdateQuestion();
        }

        #endregion

        #region Buttons - Properties

        /// <summary>
        /// Handles the confirm button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            switch (ActionType)
            {
                case TestAction.Create:
                    // Check if all nullable controls have a value.
                    if (!StartDate.SelectedDate.HasValue || !EndDate.SelectedDate.HasValue ||
                        !RandomizeQuestions.IsChecked.HasValue || !RandomizeAnswers.IsChecked.HasValue)
                    {
                        MessageBox.Show("Please fill in all fields.");

                        return;
                    }

                    TestData.Test.AuthorUserId = CurrentUser.Id;
                    TestData.Test.Name = Title_TextBox.Text;
                    TestData.Test.StartDate = StartDate.SelectedDate.Value;
                    TestData.Test.EndDate = EndDate.SelectedDate.Value;
                    TestData.Test.RandomiseQuestionsOrder = RandomizeQuestions.IsChecked.Value;
                    TestData.Test.RandomiseAnswersOrder = RandomizeAnswers.IsChecked.Value;

                    IsEnabled = false;

                    try
                    {
                        AddTestResult? addTestResult = await Api.Api.AddTestAsync(TestData.Test);

                        if (addTestResult == null)
                            return; // Handled by UpdateTest method.

                        if (addTestResult.Message.MessageType != BaseApi.ServerMessageTypes.Success)
                        {
                            MessageBox.Show(addTestResult.Message.Message, "API Error", MessageBoxButton.OK, MessageBoxImage.Error);

                            return;
                        }

                        MessageBox.Show("Test created.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        Close();
                    }
                    finally
                    {
                        IsEnabled = true;
                    }

                    return;
                case TestAction.Update:
                    if (TestData == null)
                        return;

                    Test testToUpdateCopy = new Test(
                        TestData.Test.Id, // ID: Retain the same as API.
                        Title_TextBox.Text, // TITLE: Retain the same as API.
                        TestData.Test.CreationDate, // CREATION DATE: Retain the same as API.
                        TestData.Test.AuthorUserId, // AUTHOR ID: Retain the same as API.
                        StartDate.SelectedDate ?? DateTime.Now, // If StartDate is null, then use DateTime.Now (adjust accordingly to fit your needs)
                        EndDate.SelectedDate ?? DateTime.Now, // If EndDate is null, then use DateTime.Now (adjust accordingly to fit your needs)
                        RandomizeQuestions.IsChecked ?? false, // If RandomizeQuestions is null, then use false as default
                        RandomizeAnswers.IsChecked ?? false, // If RandomizeAnswers is null, then use false as default
                        TestData.Test.Questions // QUESTIONS: Retain the same as API / fetch from data.
                    );

                    IsEnabled = false;
                    
                    try
                    {
                        UpdateTestResult? updateTestResult = await Api.Api.UpdateTestAsync(testToUpdateCopy);

                        if (updateTestResult == null)
                            return; // Handled by UpdateTest method.

                        if (updateTestResult.Message.MessageType != BaseApi.ServerMessageTypes.Success)
                        {
                            MessageBox.Show(updateTestResult.Message.Message, "API Error", MessageBoxButton.OK, MessageBoxImage.Error);

                            return;
                        }

                        MessageBox.Show("Test updated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        Close();
                    }
                    finally
                    {
                        IsEnabled = true;
                    }

                    return;
            }
        }

        /// <summary>
        /// Handles the cancel button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
			MessageBoxResult result = MessageBox.Show("Discard the test?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
				e.Handled = true;
		}

        #endregion

        #region Buttons - Questions

        /// <summary>
        /// Handles the question add button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuestionAddBtn_Click(object sender, RoutedEventArgs e)
        {
            AddQuestion();
        }

        /// <summary>
        /// Handles the question remove button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuestionRemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            RemoveQuestion();
        }

        /// <summary>
        /// Handles the question modify/update button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuestionUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateQuestion();
        }

        #endregion

        #region Fixes for better experience

        /// <summary>
        /// Allows unselecting questions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuestionsList_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            QuestionsList.UnselectAll();
        }

        /// <summary>
        /// Allows unselecting questions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuestionsList_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            QuestionsList.UnselectAll();
        }

        /// <summary>
        /// Allows unselecting questions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuestionsList_PreviewMouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            bool state = QuestionsList.SelectedItems.Count > 0;

            ContextQuestionEdit.IsEnabled = state;
            ContextQuestionRemove.IsEnabled = state;
        }

        #endregion

        #region Add, Update, Remove tasks



        /// <summary>
        /// Logic for adding a question.
        /// </summary>
        /// <returns></returns>
        public void AddQuestion()
        {
            // Create a new instance of the QuestionModifyWindow intended for question creation.
            QuestionModifyWindow questionModifyWindow = new QuestionModifyWindow(QuestionAction.Create, new Question());

            // Display the window as a modal dialog, blocking interaction with the parent window.
            questionModifyWindow.ShowDialog();

            if (questionModifyWindow.QuestionData is Question question && TestData is not null)
            {
                if (QuestionsList.ItemsSource is not List<Question> questions)
                    return;

                if (question.QuestionText == "N/A")
                    return;

                questions.Add(question);

                TestData.Test.Questions = new List<Question>(questions);
            }

            // Re-initialize the questions list to include the newly added user.
            // 'true' indicates that the method should fetch new data.
            InitQuestionsList();
        }

        /// <summary>
        /// Logic for updating a question.
        /// </summary>
        /// <returns></returns>
        private void UpdateQuestion()
        {
            // Retrieve the currently selected question from the UserList.
            Question selectedQuestion = (Question)QuestionsList.SelectedItem;

            // If no question is selected, display an error message and return a completed Task.
            if (selectedQuestion == null)
            {
                // Use MessageBoxBuilderModel to construct and display a message box.
                MessageBoxBuilderModel messageBuilder = new MessageBoxBuilderModel()
                {
                    Message = "Must select a question to modify.",
                    Icon = MessageBoxImage.Error,
                    Title = "Question Modification",
                    Button = MessageBoxButton.OK
                };

                // Display the constructed error message box.
                messageBuilder.ShowMessage();

                // Return a completed Task to satisfy the Task-returning signature of the method.
                return;
            }

            // Open the questionModifyWindow for editing the selected question's details.
            try
            {
                QuestionModifyWindow questionModifyWindow = new QuestionModifyWindow(QuestionAction.Update, selectedQuestion);

                questionModifyWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                // An exception occurred during the operation. Show an error message.
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Return a completed Task to end the method execution.
            return;
        } // Updates an existing questions's properties, info.

        /// <summary>
        /// Logic for removing a question.
        /// </summary>
        /// <returns></returns>
        private void RemoveQuestion()
        {
            // Attempt to cast the selected item in QuestionList to a Question object.
            Question selectedQuestion = (Question)QuestionsList.SelectedItem;
            MessageBoxBuilderModel messageBuilder;

            // If no Question is selected, display an error message and return early.
            if (selectedQuestion == null)
            {
                messageBuilder = new MessageBoxBuilderModel()
                {
                    Message = "Must select a Question to delete.",
                    Icon = MessageBoxImage.Error,
                    Title = "Question Deletion",
                    Button = MessageBoxButton.OK
                };

                messageBuilder.ShowMessage();

                return;
            }

            // Ask for confirmation before deletion.
            messageBuilder = new MessageBoxBuilderModel()
            {
                Message = $"Are you sure you want to delete the question \"{selectedQuestion.QuestionText}\"?",
                Icon = MessageBoxImage.Warning,
                Title = "Question deletion confirmation",
                Button = MessageBoxButton.YesNo
            };

            // If the Question does not confirm deletion, return early.
            if (messageBuilder.ShowMessage() != MessageBoxResult.Yes)
                return;

            IsEnabled = false;

            try
            {
                if (QuestionsList.ItemsSource is not List<Question> questions)
                    return;

                questions.Remove(selectedQuestion);

                TestData.Test.Questions = new List<Question>(questions);
            }
            finally
            {
                IsEnabled = true;
            }

            InitQuestionsList();
        }

        #endregion

        private void ComboBoxAnswers_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (sender is not ComboBox cb)
                return;

            // Prevent potential stack overflow.
            if (cb.SelectedIndex == -1)
                return;

            cb.SelectedIndex = -1;
        }
    }
}
