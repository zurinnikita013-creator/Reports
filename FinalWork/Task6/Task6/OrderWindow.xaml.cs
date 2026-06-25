using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Task6.Models;
//Журин Никита

namespace Task6
{
    public partial class OrderWindow : Window
    {
        private ObservableCollection<OrderItem> _cartItems;

        public OrderWindow()
        {
            InitializeComponent();
            Loaded += OrderWindow_Loaded;
        }

        private void OrderWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _cartItems = new ObservableCollection<OrderItem>(ShoppingCart.Items);
            CartGrid.ItemsSource = _cartItems;

            OrderIdTextBlock.Text = GetNewOrderId().ToString();
            ClientNameTextBlock.Text = Properties.Settings.Default.IsAuth
                ? Properties.Settings.Default.CurrentUserFIO
                : "Гость";

            UpdateTotal();
        }

        private int GetNewOrderId()
        {
            using var db = new AppDbContext();
            return db.Orders.Any() ? db.Orders.Max(o => o.Id) + 1 : 1;
        }

        private void UpdateTotal()
        {
            decimal total = _cartItems?.Sum(i => i.Product.Price * i.Quantity) ?? 0;
            TotalPriceTextBlock.Text = $"{total:N2} руб.";
        }

        private void QuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb && tb.DataContext is OrderItem item)
            {
                tb.TextChanged -= QuantityTextBox_TextChanged;

                if (int.TryParse(tb.Text, out int qty) && qty > 0)
                {
                    item.Quantity = qty;
                    UpdateTotal();
                }
                else if (qty <= 0)
                {
                    RemoveItem(item);
                }

                tb.TextChanged += QuantityTextBox_TextChanged;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { DataContext: OrderItem item })
                RemoveItem(item);
        }

        private void RemoveItem(OrderItem item)
        {
            _cartItems.Remove(item);
            ShoppingCart.Items.Remove(item);
            UpdateTotal();

            if (_cartItems.Count == 0)
            {
                MessageBox.Show("Все товары удалены. Корзина пуста.", "Уведомление");
                DialogResult = false;
            }
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (_cartItems == null || _cartItems.Count == 0)
            {
                MessageBox.Show("Невозможно оформить пустой заказ.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var random = new Random();
                string code = random.Next(100, 1000).ToString();
                string clientName = Properties.Settings.Default.IsAuth
                    ? Properties.Settings.Default.CurrentUserFIO
                    : "Гость";

                using var db = new AppDbContext();
                int orderId = db.Orders.Any() ? db.Orders.Max(o => o.Id) + 1 : 1;
                decimal total = _cartItems.Sum(i => i.Product.Price * i.Quantity);

                // Создаем заказ
                var order = new Order
                {
                    Id = orderId,
                    OrderDate = DateTime.Now,
                    Status = "новый",
                    TotalPrice = total,
                    CustomerName = clientName,
                    UserId = Properties.Settings.Default.IsAuth ? (int?)Properties.Settings.Default.CurrentUserId : null,
                    PickupCode = code
                };
                db.Orders.Add(order);

                // Добавляем детали
                foreach (var item in _cartItems)
                {
                    var product = db.Products.Find(item.Product.Id);
                    if (product == null)
                    {
                        MessageBox.Show($"Товар '{item.Product.Name}' не найден в БД!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    db.OrderDetails.Add(new OrderDetail
                    {
                        OrderId = orderId,
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        PriceAtOrder = product.Price
                    });
                }

                db.SaveChanges();

                MessageBox.Show($"Заказ №{orderId} успешно оформлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                SaveTicket(orderId, clientName, total, code);

                ShoppingCart.Items.Clear();
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveTicket(int orderId, string clientName, decimal total, string code)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt",
                FileName = $"Талон_Заказа_{orderId}.txt"
            };

            if (dialog.ShowDialog() == true)
            {
                string items = string.Join("\n", _cartItems.Select(i =>
                    $"- {i.Product.Name} x{i.Quantity} ({i.Product.Price * i.Quantity:N2} руб.)"));

                string content = $@"========================================
                                    ТАЛОН ЗАКАЗА № {orderId}
                                    ========================================
                                    Дата заказа: {DateTime.Now:dd.MM.yyyy HH:mm:ss}
                                    Клиент: {clientName}
                                    ----------------------------------------
                                    Состав заказа:
                                    {items}
                                    ----------------------------------------
                                    ИТОГОВАЯ СУММА: {total:N2} руб.
                                    ========================================
                                    КОД ПОЛУЧЕНИЯ: {code}
                                    ========================================";

                System.IO.File.WriteAllText(dialog.FileName, content);
                MessageBox.Show("Талон успешно сохранен!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}