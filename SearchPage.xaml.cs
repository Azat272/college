using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CollegeApp
{
    public partial class SearchPage : Page
    {
        private Database db = new Database();
        public ObservableCollection<object> Results { get; set; }

        public SearchPage()
        {
            InitializeComponent();
            Results = new ObservableCollection<object>();
            resultsListBox.ItemsSource = Results;
            searchTextBox.TextChanged += SearchTextBox_TextChanged; // Автоматический поиск
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            string query = searchTextBox.Text.Trim();
            Results.Clear();

            if (string.IsNullOrWhiteSpace(query)) return;

            if (searchStudents.IsChecked == true)
            {
                resultsListBox.View = new GridView
                {
                    Columns =
                    {
                        new GridViewColumn { Header = "Фамилия", DisplayMemberBinding = new Binding("LastName"), Width = 200 },
                        new GridViewColumn { Header = "Имя", DisplayMemberBinding = new Binding("FirstName"), Width = 200 },
                        new GridViewColumn { Header = "Группа", DisplayMemberBinding = new Binding("GroupName"), Width = 200 }
                    }
                };

                var students = db.GetStudents()
    .Where(s => s.FirstName.ToLower().Contains(query.ToLower()) ||
                s.LastName.ToLower().Contains(query.ToLower()) ||
                s.GroupName.ToLower().Contains(query.ToLower()))
    .ToList();


                foreach (var student in students)
                {
                    Results.Add(student);
                }
            }
            else if (searchTeachers.IsChecked == true)
            {
                resultsListBox.View = new GridView
                {
                    Columns =
                    {
                        new GridViewColumn { Header = "Фамилия", DisplayMemberBinding = new Binding("LastName"), Width = 200 },
                        new GridViewColumn { Header = "Имя", DisplayMemberBinding = new Binding("FirstName"), Width = 200 },
                        new GridViewColumn { Header = "Дисциплина", DisplayMemberBinding = new Binding("DisciplineName"), Width = 200 }
                    }
                };

                var teachers = db.GetTeachers()
    .Where(t => t.FirstName.ToLower().Contains(query.ToLower()) ||
                t.LastName.ToLower().Contains(query.ToLower()) ||
                t.DisciplineName.ToLower().Contains(query.ToLower()))
    .ToList();


                foreach (var teacher in teachers)
                {
                    Results.Add(teacher);
                }
            }
            else if (searchGroups.IsChecked == true)
            {
                resultsListBox.View = new GridView
                {
                    Columns =
                    {
                        new GridViewColumn { Header = "Название группы", DisplayMemberBinding = new Binding("Name"), Width = 300 },
                        new GridViewColumn { Header = "Студенты", DisplayMemberBinding = new Binding("StudentCount"), Width = 100 }
                    }
                };

                var groups = db.GetGroups()
    .Where(g => g.Name.ToLower().Contains(query.ToLower()))
    .Select(g => new { g.Name, StudentCount = db.GetStudentsByGroup(g.Id).Count })
    .ToList();


                foreach (var group in groups)
                {
                    Results.Add(group);
                }
            }
            else if (searchDisciplines.IsChecked == true)
            {
                resultsListBox.View = new GridView
                {
                    Columns =
                    {
                        new GridViewColumn { Header = "Название дисциплины", DisplayMemberBinding = new Binding("Name"), Width = 300 },
                        new GridViewColumn { Header = "Преподаватели", DisplayMemberBinding = new Binding("TeacherCount"), Width = 100 }
                    }
                };

                var disciplines = db.GetDisciplines()
    .Where(d => d.Name.ToLower().Contains(query.ToLower()))
    .Select(d => new { d.Name, TeacherCount = db.GetTeachersByDiscipline(d.Id)?.Count ?? 0 })
    .ToList();


                foreach (var discipline in disciplines)
                {
                    Results.Add(discipline);
                }

            }

        }
    }
}
