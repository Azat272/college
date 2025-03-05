using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CollegeApp
{
    public partial class StudentsPage : Page
    {
        private Database db = new Database();
        public ObservableCollection<Student> Students { get; set; }

        public StudentsPage()
        {
            InitializeComponent();
            Students = new ObservableCollection<Student>(db.GetStudents());
            studentsListBox.ItemsSource = Students;
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            AddStudentWindow addStudentWindow = new AddStudentWindow();
            if (addStudentWindow.ShowDialog() == true)
            {
                Students.Add(addStudentWindow.NewStudent);
            }
        }

        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            Student selectedStudent = (Student)studentsListBox.SelectedItem;
            if (selectedStudent == null)
            {
                MessageBox.Show("Выберите студента для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить студента {selectedStudent.FirstName} {selectedStudent.LastName}?",
                "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                db.DeleteStudent(selectedStudent.Id);
                Students.Remove(selectedStudent);
            }
        }

        private void studentsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
