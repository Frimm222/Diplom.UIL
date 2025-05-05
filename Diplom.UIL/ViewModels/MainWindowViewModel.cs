using Diplom.BLL.Models;
using Diplom.DAL;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Reactive;
namespace Diplom.UIL.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        private readonly DataBase _dataBase;

        public ObservableCollection<Item> Items { get; } = [];

        [Reactive]
        public Item? SelectedItem { get; set; }

        public ReactiveCommand<Unit, Unit> AddItemCommand { get; }
        public ReactiveCommand<Unit, Unit> EditItemCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteItemCommand { get; }

        public MainWindowViewModel()
        {

            _dataBase = new DataBase();
            LoadItems();
        }

        private void LoadItems()
        {
            var items = _dataBase.GetAll();
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        private void Add()
        {
            var newItem = new Item();
            var viewModel = new AddEditItemViewModel(newItem);
            var window = new AddEditItemWindow(viewModel);
            if (window.ShowDialog() == true)
            {
                _dataBase.Create(newItem);
                LoadItems();
            }
        }
    }
}
