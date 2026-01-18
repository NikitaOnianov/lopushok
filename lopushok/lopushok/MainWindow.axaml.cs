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
                .OrderBy(p => p.Product_Title)
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



        private void ApplyFilters()
        {
            if (!allProducts.Any()) return;

            var query = allProducts.AsEnumerable();

            // Поиск
            if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                var search = SearchTextBox.Text.ToLower();
                query = query.Where(p =>
                    (p.Product_Title?.ToLower().Contains(search) ?? false) ||
                    (p.Product_ArticleNumber?.ToLower().Contains(search) ?? false) ||
                    (p.Product_Type?.ToLower().Contains(search) ?? false) ||
                    (p.MaterialTypes?.ToLower().Contains(search) ?? false)
                );
            }

            // Фильтрация по типу
            if (FilterComboBox.SelectedItem is string selectedType && selectedType != "Все типы")
            {
                query = query.Where(p => p.Product_Type == selectedType);
            }

            // Сортировка
            if (SortComboBox.SelectedItem is string sortOption)
            {
                query = sortOption switch
                {
                    "Название (А-Я)" => query.OrderBy(p => p.Product_Title),
                    "Название (Я-А)" => query.OrderByDescending(p => p.Product_Title),
                    "Стоимость ↑" => query.OrderBy(p => p.MinCostForAgent),
                    "Стоимость ↓" => query.OrderByDescending(p => p.MinCostForAgent),
                    _ => query
                };
            }

            filteredProducts.Clear();
            foreach (var product in query)
            {
                filteredProducts.Add(product);
            }

            ProductsItemsControl.ItemsSource = filteredProducts;
        }

        // Обработчики событий
        private void SearchTextBox_TextChanged(object? sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SortComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void FilterComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ResetFiltersButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
            SortComboBox.SelectedIndex = 0;
            FilterComboBox.SelectedIndex = 0;
            ApplyFilters();
        }

        private void RefreshButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            LoadData();
        }

        private void Search_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {

        }

        private void ProductsItemsControl_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
           /* int id = (ProductsItemsControl.SelectedItem as product_list_layout_DTO).Product_Id;
            new ChangeProduct().Show(this);
            Close();*/
        }
    }
}