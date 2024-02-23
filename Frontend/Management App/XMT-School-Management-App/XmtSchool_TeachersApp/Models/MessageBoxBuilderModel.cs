using System.Windows;

namespace XmtSchool_TeachersApp.Models
{
    public struct MessageBoxBuilderModel
    {
        public MessageBoxImage Icon { get; set; }
        public MessageBoxButton Button { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

		public readonly MessageBoxResult ShowMessage()
		{
			return MessageBox.Show(Message, Title, Button, Icon);
		}
	}
}
