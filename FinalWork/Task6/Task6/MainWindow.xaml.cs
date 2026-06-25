using System.Windows;
using System.Windows.Controls;
using Task6.Models;

namespace Task6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateInterface();
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.IsAuth)
            {
                Logout();
            }
            else
            {
                AutorizationWindow loginWin = new() { Owner = this };
                if (loginWin.ShowDialog() == true)
                {
                    UpdateInterface();
                }
            }
        }

        private void UpdateInterface()
        {
            bool isAuth = Properties.Settings.Default.IsAuth;
            string role = Properties.Settings.Default.CurrentUserRole;
            string fio = Properties.Settings.Default.CurrentUserFIO;

            if (isAuth)
            {
                UserFioTextBlock.Text = fio;
                AuthButton.Content = "Выйти";

                if (role == "Администратор" || role == "Менеджер")
                {
                    OrderWindowButton.Visibility = Visibility.Visible;
                }
                else
                {
                    OrderWindowButton.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                UserFioTextBlock.Text = "Гость";
                AuthButton.Content = "Войти";
                OrderWindowButton.Visibility = Visibility.Collapsed;
            }

            UpdateOrderButtonVisibility();
        }

        private void Logout()
        {
            Properties.Settings.Default.CurrentUserFIO = string.Empty;
            Properties.Settings.Default.CurrentUserRole = string.Empty;
            Properties.Settings.Default.IsAuth = false;
            Properties.Settings.Default.Save();

            UpdateInterface();
            MessageBox.Show("Вы вышли из системы.", "Уведомление");
        }

        private void OrderWindowButton_Click(object sender, RoutedEventArgs e)
        {
            string currentRole = Properties.Settings.Default.CurrentUserRole;

            if (currentRole == "Администратор" || currentRole == "Менеджер")
            {
                OrdersWindow ordersWindow = new() { Owner = this };
                ordersWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Доступ запрещен!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext != null)
            {
                dynamic dynamicContext = button.DataContext;

                Product product = new Product
                {
                    Id = dynamicContext.Id,
                    Name = dynamicContext.Name,
                    Manufacturer = dynamicContext.Manufacturer,
                    Description = dynamicContext.Description,
                    Price = dynamicContext.Price
                };

                ShoppingCart.AddProduct(product);
                UpdateOrderButtonVisibility();
            }
        }

        private void UpdateOrderButtonVisibility()
        {
            if (ShoppingCart.HasItems)
            {
                ViewOrderButton.Visibility = Visibility.Visible;
            }
            else
            {
                ViewOrderButton.Visibility = Visibility.Collapsed;
            }
        }

        private void ViewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new() { Owner = this };
            if (orderWindow.ShowDialog() == true)
            {
                UpdateInterface();
            }
        }
    }
}
