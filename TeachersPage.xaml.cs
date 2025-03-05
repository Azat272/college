using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CollegeApp
{
    public partial class TeachersPage : Page
    {
        private Database db = new Database();
        public ObservableCollection<TeacherViewModel> Teachers { get; set; }

        public TeachersPage()
        {
            InitializeComponent();
            LoadTeachersData();
        }

        private void LoadTeachersData()
        {
            var teachersList = db.GetTeachers();

            Teachers = new ObservableCollection<TeacherViewModel>(
                teachersList.Select(t => new TeacherViewModel
                {
                    Id = t.Id,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    DisciplineName = db.GetDisciplineByTeacherId(t.Id)?.Name ?? "Не назначена"
                })
            );

            teachersListBox.ItemsSource = Teachers;
        }

        private void AddTeacher_Click(object sender, RoutedEventArgs e)
        {
            AddTeacherWindow addTeacherWindow = new AddTeacherWindow();
            if (addTeacherWindow.ShowDialog() == true)
            {
                LoadTeachersData();
            }
        }

        private void DeleteTeacher_Click(object sender, RoutedEventArgs e)
        {
            TeacherViewModel selectedTeacher = (TeacherViewModel)teachersListBox.SelectedItem;
            if (selectedTeacher == null)
            {
                MessageBox.Show("Выберите преподавателя для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить преподавателя {selectedTeacher.FirstName} {selectedTeacher.LastName}?",
                "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                db.DeleteTeacher(selectedTeacher.Id);
                LoadTeachersData();
            }
        }
    }

    public class TeacherViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisciplineName { get; set; }
    }
}
