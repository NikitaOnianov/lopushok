using Avalonia.Controls;
using Avalonia.Media.Imaging;
using lopushok.DTO;
using lopushok.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

namespace lopushok
{
    public partial class MainWindow : Window
    {
        private MydatabaseContext? db;
        private ObservableCollection<product_list_layout_DTO> allProducts = new();
        private ObservableCollection<product_list_layout_DTO> filteredProducts = new();
        private List<string> productTypes = new();

        public MainWindow()
        {
            InitializeComponent();
            db = new MydatabaseContext();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                allProducts = LoadProducts();

                // Загружаем типы продуктов
                productTypes = db.ProductTypes
                    .Select(pt => pt.Title)
                    .Distinct()
                    .ToList();

                // Наполняем фильтр
                FilterComboBox.Items.Clear();
                FilterComboBox.Items.Add("Все типы");
                foreach (var type in productTypes)
                {
                    FilterComboBox.Items.Add(type);
                }
                FilterComboBox.SelectedIndex = 0;

                // Показываем все продукты
                filteredProducts = new ObservableCollection<product_list_layout_DTO>(allProducts);
                ProductsItemsControl.ItemsSource = filteredProducts;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private ObservableCollection<product_list_layout_DTO> LoadProducts()
        {

            var products = db.Products
                .Include(p => p.ProductType)
                .Include(p => p.ProductMaterials)
                    .ThenInclude(pm => pm.Material)
                    .ThenInclude(m => m.MaterialType)
                .Select(product => new product_list_layout_DTO
                {
                    Product_Id = product.Id,
                    Product_Title = product.Title,
                    Product_Type = product.ProductType.Title,
                    Product_ArticleNumber = product.ArticleNumber,
                    Product_Description = product.Description,
                    MinCostForAgent = product.MinCostForAgent,
                    getLogo = LoadProductImage(product.Image),

                    // Преобразуем список типов материалов в строку
                    MaterialTypes = string.Join(", ", product.ProductMaterials
                        .Select(pm => pm.Material.Title))
                })
                .OrderByDescending(it=>it.Product_Id)
                .ToList();

            return new ObservableCollection<product_list_layout_DTO>(products);
        }

        static public Bitmap LoadProductImage(string? imageName)
        {
            try
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "Assets/products/" + imageName;
                return new Bitmap(path);
            }
            catch
            {
                return new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "Assets/picture.png");
            }
        }

        // Обработчики событий
        private void SearchTextBox_TextChanged(object? sender, TextChangedEventArgs e)
        {
            ApplyCombinedFilters();
        }

        private void SortComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            ApplyCombinedFilters();
        }

        private void FilterComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            ApplyCombinedFilters();
        }

        private void ResetFiltersButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
            SortComboBox.SelectedIndex = 0;
            FilterComboBox.SelectedIndex = 0;
            ApplyCombinedFilters();
        }

        private void RefreshButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            LoadData();

        }

        private void Search_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ApplyCombinedFilters();
        }

        private void ProductsItemsControl_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            int id = ((product_list_layout_DTO)ProductsItemsControl.SelectedItem).Product_Id;
            new ChangeProduct(id).Show();
            Close();
        }

        private void Create_Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            new ChangeProduct().Show();
            Close();
        }

        private void ApplyCombinedFilters()
        {
            if (!allProducts.Any())
            {
                filteredProducts.Clear();
                return;
            }

            IEnumerable<product_list_layout_DTO> query = allProducts.AsEnumerable();

            // 1. Поиск по ключевым полям
            if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                string search = SearchTextBox.Text.ToLower();
                query = query.Where(p =>
                    (p.Product_Title?.ToLower().Contains(search) ?? false) ||
                    (p.Product_ArticleNumber?.ToLower().Contains(search) ?? false) ||
                    (p.Product_Type?.ToLower().Contains(search) ?? false) ||
                    (p.MaterialTypes?.ToLower().Contains(search) ?? false)
                );
            }

            // 2. Фильтрация по типу товара
            if (FilterComboBox.SelectedItem is string selectedType && selectedType != "Все типы")
            {
                query = query.Where(p => p.Product_Type == selectedType);
            }

            // 3. Сортировка по индексу выбранного элемента
            int selectedIndex = SortComboBox.SelectedIndex;

            query = selectedIndex switch
            {
                1 => query.OrderBy(p => p.Product_Title ?? ""),           // "Название (А-Я)"
                2 => query.OrderByDescending(p => p.Product_Title ?? ""), // "Название (Я-А)"
                3 => query.OrderBy(p => p.MinCostForAgent),                // "Стоимость (по воз.)"
                4 => query.OrderByDescending(p => p.MinCostForAgent),     // "Стоимость (по убыв.)"
                _ => query // 0 ("Без сортировки") или любой другой индекс → без изменений
            };

            // 4. Обновление filteredProducts и интерфейса
            filteredProducts.Clear();
            foreach (var product in query)
            {
                filteredProducts.Add(product);
            }

            ProductsItemsControl.ItemsSource = filteredProducts;
        }

    }
}