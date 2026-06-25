using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Task6.Models;

namespace Task6
{
    /// <summary>
    /// Логика взаимодействия для окна администратора/менеджера OrdersWindow.
    /// </summary>
    public partial class OrdersWindow : Window
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="OrdersWindow"/>.
        /// </summary>
        public OrdersWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadOrdersData();
        }

        /// <summary>
        /// Загружает все заказы из базы данных в таблицу.
        /// </summary>
        private void LoadOrdersData()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var list = db.Orders.OrderByDescending(o => o.OrderDate).ToList();
                    AllOrdersGrid.ItemsSource = list;

                    if (list.Count == 0)
                    {
                        MessageBox.Show("В базе данных еще нет ни одного оформленного заказа.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки базы данных: {ex.Message}", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AllOrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AllOrdersGrid.SelectedItem is Order selectedOrder)
            {
                PlaceholderText.Visibility = Visibility.Collapsed;
                DetailsPanel.Visibility = Visibility.Visible;
                SaveStatusButton.Visibility = Visibility.Visible;

                DetailIdText.Text = selectedOrder.Id.ToString();
                DetailDateText.Text = selectedOrder.OrderDate.ToString("g");
                DetailClientText.Text = !string.IsNullOrEmpty(selectedOrder.CustomerName) ? selectedOrder.CustomerName : "Неавторизованный клиент (Гость)";
                DetailCodeText.Text = selectedOrder.PickupCode;

                DeliveryDatePicker.SelectedDate = selectedOrder.DeliveryDate;

                foreach (ComboBoxItem item in StatusComboBox.Items)
                {
                    if (item.Content.ToString() == selectedOrder.Status)
                    {
                        StatusComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
            else
            {
                PlaceholderText.Visibility = Visibility.Visible;
                DetailsPanel.Visibility = Visibility.Collapsed;
                SaveStatusButton.Visibility = Visibility.Collapsed;
            }
        }

        private void SaveStatusButton_Click(object sender, RoutedEventArgs e)
        {
            if (AllOrdersGrid.SelectedItem is Order selectedOrder)
            {
                try
                {
                    using (var db = new AppDbContext())
                    {
                        var dbOrder = db.Orders.FirstOrDefault(o => o.Id == selectedOrder.Id);
                        if (dbOrder != null)
                        {
                            dbOrder.DeliveryDate = DeliveryDatePicker.SelectedDate;
                            dbOrder.Status = ((ComboBoxItem)StatusComboBox.SelectedItem).Content.ToString();

                            db.SaveChanges();
                            MessageBox.Show("Параметры заказа успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                            LoadOrdersData();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения изменений: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
