using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XmtSchool_TeachersApp.Models;
using XmtSchoolTypes.Tests;

namespace XmtSchool_TeachersApp.Views
{
    /// <summary>
    /// Interaction logic for QuestionModifyWindow.xaml
    /// </summary>
    /// 

    public partial class QuestionModifyWindow : Window
    {
        private readonly QuestionAction ActionType;

        public Question QuestionData { get; private set; }

        public QuestionModifyWindow(QuestionAction actionType, Question questionData)
        {
            InitializeComponent();

            ActionType = actionType;
            QuestionData = questionData;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Question_TextBox.Focus();

            if (QuestionData == null)
                return;

            if (ActionType == QuestionAction.Create)
            {
                Title = "XMT School - Management App - Create a new question";

                Confirm_Button.Content = "Create";

                return;
            }

            Title = $"XMT School - Management App - Update question {QuestionData.Id}";

            Confirm_Button.Content = "Update";

            Question_TextBox.Text = QuestionData.QuestionText;

            InitAnswersList();
        }


        /// <summary>
        /// Initializes loading of answers in a list.
        /// </summary>
        /// <param name="fetchNewData"></param>
        /// <returns></returns>
        private void InitAnswersList()
        {
            // Retrieve the updated list of answers from the cache.
            List<Answer> answers = QuestionData?.Answers ?? new List<Answer>();

            // Assign the answers list as the data source for the AnswersList UI element.
            AnswersList.ItemsSource = answers;

            if (AnswersList.View is GridView gv)
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

        #region Context Menu - Answers

        private void ContextAnswerAdd_Click(object sender, RoutedEventArgs e)
        {
            AddAnswer();
        }

        private void ContextAnswerEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ContextAnswerRemove_Click(object sender, RoutedEventArgs e)
        {
            RemoveAnswer();
        }

        #endregion


        #region Buttons - Properties

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            // Assuming AnswersList is your UI element that holds the answers.
            // This might need adjustments based on how you store/display answers in your UI.
            if (AnswersList.ItemsSource is not List<Answer> answers || !answers.Any())
            {
                MessageBoxBuilderModel messageBuilder = new MessageBoxBuilderModel()
                {
                    Message = "No answers specified.",
                    Icon = MessageBoxImage.Error,
                    Title = "Question Modification",
                    Button = MessageBoxButton.OK
                };

                messageBuilder.ShowMessage();

                return;
            }

            switch (ActionType)
            {
                case QuestionAction.Update:
                case QuestionAction.Create:
                    if (string.IsNullOrWhiteSpace(Question_TextBox.Text))
                    {
                        MessageBoxBuilderModel messageBuilder = new MessageBoxBuilderModel()
                        {
                            Message = "Question must have text.",
                            Icon = MessageBoxImage.Error,
                            Title = "Question Modification",
                            Button = MessageBoxButton.OK
                        };

                        messageBuilder.ShowMessage();

                        return;
                    }

                    // Assuming QuestionData is a field or property of your class
                    QuestionData = new Question(Question_TextBox.Text, answers);

                    Close();

                    break;

                default:
                    // Handle other cases or throw an unexpected action type exception
                    break;
            }
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Buttons - Answers

        private void AnswerAddBtn_Click(object sender, RoutedEventArgs e)
        {
            AddAnswer();
        }

        private void AnswerRemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            RemoveAnswer();
        }

        private void AnswerUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            EditAnswer();
        }

        #endregion

        #region Fixes for better experience

        private void AnswersList_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            AnswersList.UnselectAll();
        }

        private void AnswersList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AnswersList.UnselectAll();
        }

        private void AnswersList_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            bool state = AnswersList.SelectedItems.Count > 0;

            ContextAnswerEdit.IsEnabled = state;
            ContextAnswerRemove.IsEnabled = state;
        }

        #endregion

        #region Add, Update, Remove tasks

        private void AddAnswer()
        {
            AnswerModifyWindow answerModifyWindow = new AnswerModifyWindow(AnswerAction.Create, new Answer());

            answerModifyWindow.ShowDialog();

            if (answerModifyWindow.AnswerData is Answer answer && QuestionData is Question question)
            {
                if (answerModifyWindow.AnswerData.AnswerText == "N/A")
                    return;

                question.Answers.Add(answer);

                QuestionData.Answers = new List<Answer>(question.Answers);
            }

            InitAnswersList();
        }

        private void EditAnswer()
        {
            // Retrieve the currently selected answer from the UserList.
            Answer selectedAnswer = (Answer)AnswersList.SelectedItem;

            // If no answer is selected, display an error message and return a completed Task.
            if (selectedAnswer == null)
            {
                // Use MessageBoxBuilderModel to construct and display a message box.
                MessageBoxBuilderModel messageBuilder = new MessageBoxBuilderModel()
                {
                    Message = "Must select a answer to modify.",
                    Icon = MessageBoxImage.Error,
                    Title = "Answer Modification",
                    Button = MessageBoxButton.OK
                };

                // Display the constructed error message box.
                messageBuilder.ShowMessage();

                // Return a completed Task to satisfy the Task-returning signature of the method.
                return;
            }

            // Open the answerModifyWindow for editing the selected answer's details.
            try
            {
                AnswerModifyWindow answerModifyWindow = new AnswerModifyWindow(AnswerAction.Update, selectedAnswer);

                answerModifyWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                // An exception occurred during the operation. Show an error message.
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Return a completed Task to end the method execution.
            return;
        }

        private void RemoveAnswer()
        {
            // Attempt to cast the selected item in QuestionList to a Question object.
            Answer selectedAnswer = (Answer)AnswersList.SelectedItem;
            MessageBoxBuilderModel messageBuilder;

            // If no Question is selected, display an error message and return early.
            if (selectedAnswer == null)
            {
                messageBuilder = new MessageBoxBuilderModel()
                {
                    Message = "Must select an Answer to delete.",
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
                Message = $"Are you sure you want to delete answer \"{selectedAnswer.AnswerText}\"?",
                Icon = MessageBoxImage.Warning,
                Title = "Answer deletion confirmation",
                Button = MessageBoxButton.YesNo
            };

            // If the Question does not confirm deletion, return early.
            if (messageBuilder.ShowMessage() != MessageBoxResult.Yes)
                return;

            IsEnabled = false;

            try
            {
                if (AnswersList.ItemsSource is not List<Answer> answers)
                    return;

                answers.Remove(selectedAnswer);

                QuestionData.Answers = new List<Answer>(answers);
            }
            finally
            {
                IsEnabled = true;
            }

            InitAnswersList();
        }
        #endregion
    }
}
