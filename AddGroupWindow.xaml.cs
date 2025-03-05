using System.Windows;

namespace CollegeApp
{
    public partial class AddGroupWindow : Window
    {
        private Database db = new Database();
        public StudyGroup NewGroup { get; private set; }

        public AddGroupWindow()
        {
            InitializeComponent();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            string groupName = groupNameTb.Text.Trim();
            if (string.IsNullOrWhiteSpace(groupName))
            {
                MessageBox.Show("Введите название группы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            db.AddGroup(groupName);
            NewGroup = new StudyGroup { Name = groupName };
            MessageBox.Show("Группа добавлена!");
            DialogResult = true;
            Close();
        }
    }
}
