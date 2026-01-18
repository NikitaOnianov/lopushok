using Avalonia.Media.Imaging;
using System;

namespace lopushok.DTO
{
    public partial class product_list_layout_DTO
    {
        // Продукт
        public int Product_Id { get; set; }
        public string Product_Title { get; set; } = null!;
        public string? Product_Type { get; set; }
        public string Product_ArticleNumber { get; set; } = null!;
        public string? Product_Description { get; set; }
        public int? ProductionPersonCount { get; set; }
        public int? ProductionWorkshopNumber { get; set; }
        public decimal MinCostForAgent { get; set; }

        // Изображение
        public Bitmap? getLogo { get; set; }

        // Материалы (в виде строки для простоты)
        public string? MaterialTypes { get; set; }
    }
}