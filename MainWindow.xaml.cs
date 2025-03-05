using System.Windows;
using System.Windows.Navigation;

namespace CollegeApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StudentsBtn_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new StudentsPage());
        }

        private void TeachersBtn_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new TeachersPage());
        }

        private void DisciplinesBtn_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new DisciplinesPage());
        }

        private void GroupsBtn_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new GroupsPage());
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new SearchPage());
        }
    }
}
