using Diplom.BLL.Models;
using Diplom.UIL.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Windows;

namespace Diplom.UIL.ViewModels
{
    public class AddEditItemViewModel : BaseViewModel
    {
        private bool isNew = true;
        public Guid Id { get; set; }
        [Reactive] public string Name { get; set; }
        [Reactive] public string? Description { get; set; } = string.Empty;
        [Reactive] public string? CategoryId { get; set; } = 0.ToString();
        [Reactive] public double Price { get; set; }
        [Reactive] public int Quantity { get; set; }
        [Reactive] public string? Barcode { get; set; } = string.Empty;
        [Reactive] public string? Image {  get; set; } = string.Empty;
        public List<Category> Categories { get; set; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }
        public ReactiveCommand<Unit, Unit> ScanCommand { get; }

        public AddEditItemViewModel(Item item)
        {
            Categories = _dataBase.GetCategories();
            CancelCommand = ReactiveCommand.Create(Cancel);
            ScanCommand = ReactiveCommand.Create(Scan);
            var canExecuteSave = this.WhenAnyValue(
                x => x.Name,
                x => x.Price,
                x => x.Quantity,
                (name, price, quantity) => !string.IsNullOrEmpty(name) && price > 0 && quantity > 0);
            SaveCommand = ReactiveCommand.Create(Save, canExecuteSave);
            if (!string.IsNullOrEmpty(item.name))
            {
                isNew = false;
                var category = Categories.FirstOrDefault(c => c.name == item.category_id);
                Id = item.id;
                Name = item.name;
                Description = item.description;
                CategoryId = category.id.ToString();
                Price = item.price;
                Quantity = item.quantity;
                Barcode = item.barcode;
                Image = item.image;
            }
        }
        public void Save()
        {
            if (isNew)
            {
                _dataBase.Create(new Item
                {
                    name = Name,
                    description = Description,
                    category_id = CategoryId,
                    price = Price,
                    quantity = Quantity,
                    barcode = Barcode,
                    image = Image
                });
            }
            else
            {
                _dataBase.Update(new Item
                {
                    id = Id,
                    name = Name,
                    description = Description,
                    category_id = CategoryId,
                    price = Price,
                    quantity = Quantity,
                    barcode = Barcode,
                    image = Image
                });
            }
            Cancel();
        }
        public void Cancel()
        {
            foreach (Window win in Application.Current.Windows)
            {
                if (win is AddEditItemWindow)
                {
                    win.Close();
                    break;
                }
            }
        }
        public void Scan()
        {
            var rnd = new Random();
            var barcode = new StringBuilder();
            for (int i = 0; i < 12; i++)
            {
                barcode.Append(rnd.Next(0, 10));
            }
            Barcode = barcode.ToString();
        }
    }
}
