using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Task6.Models
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<Product> _allProducts;
        public ICollectionView ProductView { get; private set; }
        public ObservableCollection<string> Manufacturers { get; set; }
        public List<string> SortOptions { get; set; }
        private bool _isPlaceholderVisible;

        private string _searchText = string.Empty;
        private string _selectedManufacturer = "Все производители";
        private decimal? _minPrice;
        private decimal? _maxPrice;
        private string _selectedSortOption;

        private int _totalCount;
        private int _filteredCount;

        public MainViewModel()
        {
            LoadData();

            ProductView = CollectionViewSource.GetDefaultView(_allProducts);
            ProductView.Filter = FilterProduct;

            SortOptions = new List<string>
            {
                "Без сортировки",
                "Наименование (по возрастанию)",
                "Наименование (по убыванию)",
                "Стоимость (по возрастанию)",
                "Стоимость (по убыванию)"
            };
            _selectedSortOption = SortOptions[0];

            UpdateCounts();
        }

        public bool IsPlaceholderVisible
        {
            get => _isPlaceholderVisible;
            set { _isPlaceholderVisible = value; OnPropertyChanged(nameof(IsPlaceholderVisible)); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); RefreshView(); }
        }

        public string SelectedManufacturer
        {
            get => _selectedManufacturer;
            set { _selectedManufacturer = value; OnPropertyChanged(nameof(SelectedManufacturer)); RefreshView(); }
        }

        public decimal? MinPrice
        {
            get => _minPrice;
            set { _minPrice = value; OnPropertyChanged(nameof(MinPrice)); RefreshView(); }
        }

        public decimal? MaxPrice
        {
            get => _maxPrice;
            set { _maxPrice = value; OnPropertyChanged(nameof(MaxPrice)); RefreshView(); }
        }

        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set { _selectedSortOption = value; OnPropertyChanged(nameof(SelectedSortOption)); ApplySort(); }
        }

        public int TotalCount
        {
            get => _totalCount;
            set { _totalCount = value; OnPropertyChanged(nameof(TotalCount)); }
        }

        public int FilteredCount
        {
            get => _filteredCount;
            set { _filteredCount = value; OnPropertyChanged(nameof(FilteredCount)); }
        }

        private void UpdateCounts()
        {
            TotalCount = _allProducts.Count;
            int count = 0;
            foreach (var item in ProductView) count++;
            FilteredCount = count;

            IsPlaceholderVisible = (count == 0 && TotalCount > 0);
        }

        private bool FilterProduct(object obj)
        {
            if (obj is not Product product) return false;

            if (!string.IsNullOrWhiteSpace(SearchText) &&
                product.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) < 0)
                return false;

            if (SelectedManufacturer != "Все производители" &&
                product.Manufacturer != SelectedManufacturer)
                return false;

            if (MinPrice.HasValue && product.Price < MinPrice.Value)
                return false;

            if (MaxPrice.HasValue && product.Price > MaxPrice.Value)
                return false;

            return true;
        }

        private void ApplySort()
        {
            ProductView.SortDescriptions.Clear();

            switch (SelectedSortOption)
            {
                case "Наименование (по возрастанию)":
                    ProductView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                    break;
                case "Наименование (по убыванию)":
                    ProductView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Descending));
                    break;
                case "Стоимость (по возрастанию)":
                    ProductView.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Ascending));
                    break;
                case "Стоимость (по убыванию)":
                    ProductView.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Descending));
                    break;
            }
        }

        private void RefreshView()
        {
            ProductView.Refresh();
            UpdateCounts();
        }

        private void LoadData()
        {
            using (var db = new AppDbContext())
            {
                _allProducts = new ObservableCollection<Product>(db.Products.ToList());
            }

            Manufacturers = new ObservableCollection<string> { "Все производители" };
            foreach (var p in _allProducts)
            {
                if (!Manufacturers.Contains(p.Manufacturer))
                    Manufacturers.Add(p.Manufacturer);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}