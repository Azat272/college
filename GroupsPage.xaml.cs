using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CollegeApp
{
    public partial class GroupsPage : Page
    {
        private Database db = new Database();
        public ObservableCollection<StudyGroup> Groups { get; set; }

        public GroupsPage()
        {
            InitializeComponent();
            Groups = new ObservableCollection<StudyGroup>(db.GetGroups());
            groupsListBox.ItemsSource = Groups;
        }

        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            AddGroupWindow addGroupWindow = new AddGroupWindow();
            if (addGroupWindow.ShowDialog() == true)
            {
                Groups.Add(addGroupWindow.NewGroup);
            }
        }
        private void DeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            StudyGroup selectedGroup = (StudyGroup)groupsListBox.SelectedItem;
            if (selectedGroup == null)
            {
                MessageBox.Show("Выберите группу для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить группу {selectedGroup.Name}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                db.DeleteGroup(selectedGroup.Id);
                Groups.Remove(selectedGroup);
            }
        }

    }
}
