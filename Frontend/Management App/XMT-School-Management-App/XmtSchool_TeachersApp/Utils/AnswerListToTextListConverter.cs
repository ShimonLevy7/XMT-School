using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using XmtSchoolTypes.Tests;

namespace XmtSchool_TeachersApp.Utils
{
    public class AnswerListToTextListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not List<Answer> answers)
                return Array.Empty<string>();

            IEnumerable<string> getAnswersText()
            {
                foreach (Answer answer in answers)
                {
                    yield return answer.AnswerText;
                }
            }

            return getAnswersText().ToArray();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not List<string> answersText)
                return Array.Empty<Answer>();

            IEnumerable<Answer> getAnswers()
            {
                foreach (string answerText in answersText)
                {
                    yield return new Answer(answerText, false);
                }
            }

            return getAnswers().ToArray();
        }
    }
}
