using System.Windows;
using Task6.Models;

namespace Task6
{
    /// <summary>
    /// Логика взаимодействия для AutorizationWindow.xaml
    /// </summary>
    public partial class AutorizationWindow : Window
    {
        public AutorizationWindow()
        {
            InitializeComponent();
        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Password;

            using (var db = new AppDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Login == login && u.Password == password);

                if (user != null)
                {
                    Properties.Settings.Default.CurrentUserId = user.Id;
                    Properties.Settings.Default.CurrentUserFIO = user.FIO;
                    Properties.Settings.Default.CurrentUserRole = user.Role;
                    Properties.Settings.Default.IsAuth = true;
                    Properties.Settings.Default.Save();

                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
