using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CollegeApp
{
    public partial class DisciplinesPage : Page
    {
        private Database db = new Database();
        public ObservableCollection<Discipline> Disciplines { get; set; }

        public DisciplinesPage()
        {
            InitializeComponent();
            Disciplines = new ObservableCollection<Discipline>(db.GetDisciplines());
            disciplinesListBox.ItemsSource = Disciplines;
        }

        private void AddDiscipline_Click(object sender, RoutedEventArgs e)
        {
            AddDisciplineWindow addDisciplineWindow = new AddDisciplineWindow();
            if (addDisciplineWindow.ShowDialog() == true)
            {
                Disciplines.Add(addDisciplineWindow.NewDiscipline);
            }
        }

        private void DeleteDiscipline_Click(object sender, RoutedEventArgs e)
        {
            Discipline selectedDiscipline = (Discipline)disciplinesListBox.SelectedItem;
            if (selectedDiscipline == null)
            {
                MessageBox.Show("Выберите дисциплину для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить дисциплину {selectedDiscipline.Name}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                db.DeleteDiscipline(selectedDiscipline.Id);
                Disciplines.Remove(selectedDiscipline);
            }
        }
    }
}
