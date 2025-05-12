using Diplom.BLL.Models;
using Diplom.DAL;
using Diplom.UIL.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows;
namespace Diplom.UIL.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        private readonly DataBase _dataBase;

        public ObservableCollection<Item> Items { get; } = [];

        [Reactive] public Item? SelectedItem { get; set; }

        public ReactiveCommand<Unit, Unit> AddItemCommand { get; }
        public ReactiveCommand<Unit, Unit> EditItemCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteItemCommand { get; }

        public MainWindowViewModel()
        {
            AddItemCommand = ReactiveCommand.Create(Add);
            var canExecuteEdit = this.WhenAnyValue(
                x => x.SelectedItem,
                x => x.Items,
                (item, items) => item is not null && items is not null);
            EditItemCommand = ReactiveCommand.Create(Edit, canExecuteEdit);
            DeleteItemCommand = ReactiveCommand.Create(Delete, canExecuteEdit);
            _dataBase = new DataBase();
            LoadItems();
        }

        private async Task LoadItems()
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
            var viewModel = new AddEditItemWindow(new Item());
            viewModel.ShowDialog();
            LoadItems();
        }
        private void Edit() {
            var viewModel = new AddEditItemWindow(SelectedItem);
            viewModel.ShowDialog();
            LoadItems();
        }

        private void Delete()
        {
            var res = MessageBox.Show("Удалить позицию?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (SelectedItem is not null && MessageBoxResult.Yes == res)
            {
                _dataBase.Delete(SelectedItem.id);
                LoadItems();
            }
        }
    }
}
