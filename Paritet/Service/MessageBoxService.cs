using System.Windows;

namespace Paritet.Service
{
    public class MessageBoxService : IMessageBox
    {
        public void Show(string message)
        {
            MessageBox.Show(message);
        }
    }
}
