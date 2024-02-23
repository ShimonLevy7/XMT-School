using System.Windows;
using XmtSchool_TeachersApp.Models;
using XmtSchoolTypes.Tests;

namespace XmtSchool_TeachersApp.Views
{
    /// <summary>
    /// Interaction logic for AnswerModifyWindow.xaml
    /// </summary>
    public partial class AnswerModifyWindow : Window
    {
        private readonly AnswerAction ActionType;

        public Answer AnswerData { get; private set; }

        public AnswerModifyWindow(AnswerAction actionType, Answer answerData)
        {
            InitializeComponent();
            
            ActionType = actionType;
            AnswerData = answerData;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Answer_TextBox.Focus();

            if (AnswerData == null)
                return;

            if (ActionType == AnswerAction.Create)
            {
                Title = "XMT School - Management App - Create a new answer";

                Confirm_Button.Content = "Create";

                return;
            }

            Title = $"XMT School - Management App - Update answer {AnswerData.Id}";

            Confirm_Button.Content = "Update";
        }

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            switch(ActionType)
            {
                case AnswerAction.Create:
                    if (string.IsNullOrWhiteSpace(Answer_TextBox.Text))
                    {
                        MessageBoxBuilderModel messageBuilder = new MessageBoxBuilderModel()
                        {
                            Message = "Answer field must be filled",
                            Icon = MessageBoxImage.Error,
                            Title = "Answer Modification",
                            Button = MessageBoxButton.OK
                        };

                        messageBuilder.ShowMessage();

                        return;
                    }    

                    AnswerData = new Answer(Answer_TextBox.Text, AnswerState.IsChecked ?? false);

                    Close();

                    return;
            }
        }
    }
}
