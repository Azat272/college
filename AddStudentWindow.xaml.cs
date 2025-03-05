using System.Collections.Generic;
using System.Windows;

namespace CollegeApp
{
    public partial class AddStudentWindow : Window
    {
        private Database db = new Database();
        public Student NewStudent { get; private set; }
        private List<StudyGroup> groupsList;

        public AddStudentWindow()
        {
            InitializeComponent();
            LoadGroups();
        }

        private void LoadGroups()
        {
            groupsList = db.GetGroups(); // Загружаем список групп
            groupComboBox.ItemsSource = groupsList;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            string firstName = firstNameTb.Text.Trim();
            string lastName = lastNameTb.Text.Trim();
            int? groupId = (int?)groupComboBox.SelectedValue;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || groupId == null)
            {
                MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            db.AddStudent(firstName, lastName, groupId.Value);
            NewStudent = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                GroupId = groupId.Value,
                GroupName = groupsList.Find(g => g.Id == groupId.Value)?.Name
            };
            MessageBox.Show("Студент добавлен!");
            DialogResult = true;
            Close();
        }
    }
}
