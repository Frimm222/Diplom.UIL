using Diplom.BLL.Models;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diplom.UIL.ViewModels
{
    class ExtendedWindowViewModel : BaseViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = string.Empty;
        public string? CategoryId { get; set; } = 0.ToString();
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string? Barcode { get; set; } = string.Empty;
        public string Img { get; set; } = "https://static.vecteezy.com/system/resources/thumbnails/052/836/202/small_2x/a-black-and-white-image-of-a-warehouse-free-vector.jpg";
        public List<Category> Categories { get; set; }
        public ExtendedWindowViewModel(Item item)
        {
            Categories = _dataBase.GetCategories();
            var category = Categories.FirstOrDefault(c => c.name == item.category_id);
            Id = item.id;
            Name = item.name;
            Description = item.description;
            CategoryId = category.id.ToString();
            Price = item.price;
            Quantity = item.quantity;
            Barcode = item.barcode;
        }
    }
}
