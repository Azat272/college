using System.Windows;

namespace CollegeApp
{
    public partial class AddDisciplineWindow : Window
    {
        private Database db = new Database();
        public Discipline NewDiscipline { get; private set; }

        public AddDisciplineWindow()
        {
            InitializeComponent();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            string disciplineName = disciplineNameTb.Text.Trim();
            if (string.IsNullOrWhiteSpace(disciplineName))
            {
                MessageBox.Show("Введите название дисциплины.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            db.AddDiscipline(disciplineName);
            NewDiscipline = new Discipline { Name = disciplineName };
            MessageBox.Show("Дисциплина добавлена!");
            DialogResult = true;
            Close();
        }
    }
}
