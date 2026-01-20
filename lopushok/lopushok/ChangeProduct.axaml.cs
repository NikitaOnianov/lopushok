using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using lopushok.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace lopushok;

public partial class ChangeProduct : Window
{
    static private MydatabaseContext db;
    string? ImageName = null;
    public Bitmap bitmapToBind;

    Product product1;

    public ChangeProduct()
    {
        InitializeComponent();
        db = new MydatabaseContext();
        Listtype.ItemsSource = new ObservableCollection<ProductType>(db.ProductTypes);
        ImageName = "";
        bitmapToBind = LoadProductImage(ImageName);
        ImagePath.Source = bitmapToBind;
    }
    public ChangeProduct(int id)
    {
        InitializeComponent();
        db = new MydatabaseContext();
        Listtype.ItemsSource = new ObservableCollection<ProductType>(db.ProductTypes);

        product1 = db.Products.First(x => x.Id == id);
        ImageName = product1.Image;
        bitmapToBind = LoadProductImage(ImageName);
        ImagePath.Source = bitmapToBind;
        title.Text = product1.Title;
        Article.Text = product1.ArticleNumber;
        description.Text = product1.Description;
        Listtype.SelectedItem = db.ProductTypes.Where(it => it.Id == product1.ProductTypeId).First();
        price.Value = (decimal)product1.MinCostForAgent;
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

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            if (product1 != null) return;
            if (title.Text != "" &&
                Listtype.SelectedItem != null &&
                Article.Text != "" &&
                Article.MaxLength < 10 &&
                price.Value != null
                )
            {
                createProduct();
                new MainWindow().Show();
                Close();
            }
        }
        catch
        {
            mes.Text = "Ошибка создания продукта";
        }
    }

    void createProduct()
    {
        Product productNew = new Product()
        {
            Title = title.Text,
            ProductTypeId = (int)(Listtype.SelectedItem as ProductType).Id,
            ArticleNumber = Article.Text,
            Description = description.Text,
            Image = ImageName,
            MinCostForAgent = (decimal)price.Value
        };
        db.Products.Add(productNew);
        db.SaveChanges();
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            changeProduct();
            new MainWindow().Show();
            Close();
        }
        catch
        {
            mes.Text = "Ошибка изменения";
        }
    }

    void changeProduct()
    {
        if (product1 == null)  return; 

        Product productNew = product1;
        if (title.Text != product1.Title) { productNew.Title = title.Text; }
        if ((int)(Listtype.SelectedItem as ProductType).Id != product1.ProductTypeId) { productNew.ProductTypeId = (int)(Listtype.SelectedItem as ProductType).Id; }
        if (Article.Text != product1.ArticleNumber) { productNew.ArticleNumber = Article.Text; }
        if (description.Text != product1.Description) { productNew.Description = description.Text; }
        if (ImageName != product1.Image) { productNew.Image = ImageName; }
        if (price.Value != product1.MinCostForAgent) { productNew.MinCostForAgent = (decimal)price.Value; }

        Product product = db.Products.Where(it => it.Id == product1.Id).First();
        product.Title = productNew.Title;
        product.Description = productNew.Description;
        product.ArticleNumber = productNew.ArticleNumber;
        product.Image = productNew.Image;
        product.MinCostForAgent = productNew.MinCostForAgent;

        db.SaveChanges();
    }

    private void Button_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string targetFolder = AppDomain.CurrentDomain.BaseDirectory + @"Assets\products";
        CopyImageToFolder(targetFolder);
    }

    public async void CopyImageToFolder(string targetFolderPath)
    {
        OpenFileDialog _openFileDialog = new OpenFileDialog();
        var result = await _openFileDialog.ShowAsync(this);

        if (result == null || result.Length == 0) return;

        string sourceFilePath = result[0]; // Получаем первый выбранный файл

        try
        {
            bitmapToBind = new Bitmap(sourceFilePath);
            string fileName = Path.GetFileName(sourceFilePath);
            string destPath = Path.Combine(targetFolderPath, fileName);

            Directory.CreateDirectory(targetFolderPath);
            File.Copy(sourceFilePath, destPath, overwrite: true);

            ImagePath.Source = bitmapToBind;
            ImageName = fileName;
        }
        catch (Exception ex)
        {
            // Обработка ошибок (можно вывести сообщение пользователю)
            mes.Text = "Произошла ошибка: " + ex.Message;
        }
    }

    private void Button_Click_3(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Product product = db.Products.First(it=>it.Id == product1.Id);
        db.Products.Remove(product);
        db.SaveChanges();

        new MainWindow().Show();
        Close();
    }
}