using Paritet.Service;
using Paritet.ViewModel;
using System;
using System.Windows.Controls;

namespace Paritet.View
{
    /// <summary>
    /// Логика взаимодействия для PersonView.xaml
    /// </summary>
    public partial class PersonView : Page
    {
        public PersonView()
        {
            InitializeComponent();
            DataContext = new PersonViewModel(new MessageBoxService());

            DateOfBirth.SelectedDate = DateTime.Now;
        }

        private void WeightTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            WeightTB.Text = WeightTB.Text.Replace('.', ',');
        }
    }
}