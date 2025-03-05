using System.Collections.Generic;
using System.Windows;

namespace CollegeApp
{
    public partial class AddTeacherWindow : Window
    {
        private Database db = new Database();
        public Teacher NewTeacher { get; private set; }
        private List<Discipline> disciplinesList;

        public AddTeacherWindow()
        {
            InitializeComponent();
            LoadDisciplines();
        }

        private void LoadDisciplines()
        {
            disciplinesList = db.GetDisciplines(); // Загружаем список дисциплин
            disciplineComboBox.ItemsSource = disciplinesList;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            string firstName = firstNameTb.Text.Trim();
            string lastName = lastNameTb.Text.Trim();
            int? disciplineId = (int?)disciplineComboBox.SelectedValue;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || disciplineId == null)
            {
                MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            db.AddTeacher(firstName, lastName);
            int teacherId = db.GetLastInsertedId(); // Получаем ID добавленного преподавателя

            db.AssignDisciplineToTeacher(teacherId, disciplineId.Value);

            NewTeacher = new Teacher
            {
                FirstName = firstName,
                LastName = lastName
            };
            MessageBox.Show("Преподаватель добавлен!");
            DialogResult = true;
            Close();
        }
    }
}
