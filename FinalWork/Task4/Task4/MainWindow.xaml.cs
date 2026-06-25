using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Task4.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace Task4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var products = await db.Products.ToListAsync();

                    ProductsListBox.ItemsSource = products;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных из БД: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Product selectedProduct)
            {
                MessageBox.Show($"Товар \"{selectedProduct.Name}\" (ID: {selectedProduct.Id}) добавлен в заказ!", "Заказ оформлен");
            }
        }
    }
}