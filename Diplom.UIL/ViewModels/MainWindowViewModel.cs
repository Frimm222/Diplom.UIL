using Diplom.BLL.Models;
using Diplom.DAL;
using Diplom.UIL.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
namespace Diplom.UIL.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        private readonly DataBase _dataBase = ((App)Application.Current).DataBase;

        public ObservableCollection<Item> Items { get; } = [];
        public string CurrentUser { get; set; } = $"{((App)Application.Current).CurrentUser?.Surname} {((App)Application.Current).CurrentUser?.Name} {((App)Application.Current).CurrentUser?.Patronymic}";
        public string CurrentRole { get; set; } = ((App)Application.Current).CurrentUser?.Role ?? String.Empty;
        [Reactive] public Item? SelectedItem { get; set; }
        [Reactive] public string? SearchText { get; set; } = string.Empty;
        [Reactive] public string? LogText { get; set; } = string.Empty;
        public ReactiveCommand<Unit, Unit> AddItemCommand { get; }
        public ReactiveCommand<Unit, Unit> EditItemCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteItemCommand { get; }
        public ReactiveCommand<Unit, Unit> SearchCommand { get; }
        public ReactiveCommand<Unit, Unit> LogoutCommand { get; }

        public MainWindowViewModel()
        {
            AddItemCommand = ReactiveCommand.Create(Add);
            LogoutCommand = ReactiveCommand.Create(() =>
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                Application.Current.MainWindow?.Close();
                Application.Current.MainWindow = loginWindow;
            });
            var canExecuteEdit = this.WhenAnyValue(
                x => x.SelectedItem,
                x => x.Items,
                (item, items) => item is not null && items is not null);
            EditItemCommand = ReactiveCommand.Create(Edit, canExecuteEdit);
            DeleteItemCommand = ReactiveCommand.Create(Delete, canExecuteEdit);
            var canExecuteSearch = this.WhenAnyValue(
                x => x.SearchText,
                (searchText) => !string.IsNullOrEmpty(searchText));
            SearchCommand = ReactiveCommand.Create(Search, canExecuteSearch);
            //_dataBase = new DataBase();
            _ = LoadItems();
            this.WhenAnyValue(x => x.SearchText)
                .Throttle(TimeSpan.FromSeconds(0.3), RxApp.TaskpoolScheduler)
                .Select(query => query?.Trim())
                .DistinctUntilChanged()
                //.Where(query => !string.IsNullOrWhiteSpace(query))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => Search());
        }
        private async Task LoadItems()
        {
            var items = await _dataBase.GetAll();
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
            _ = LoadItems();
        }
        private void Edit() {
            var viewModel = new AddEditItemWindow(SelectedItem);
            viewModel.ShowDialog();
            _ = LoadItems();
        }
        private void Delete()
        {
            var res = MessageBox.Show("Удалить позицию?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (SelectedItem is not null && MessageBoxResult.Yes == res)
            {
                _dataBase.Delete(SelectedItem.id);
                _ = LoadItems();
            }
        }
        private async void Search()
        {
            var items = await _dataBase.GetAll(); // Await the Task<List<Item>> to get the actual list
            Items.Clear();
            foreach (var item in items)
            {
                if (item.name.ToLower().Contains(SearchText.ToLower()) || item.description.ToLower().Contains(SearchText.ToLower()))
                {
                    Items.Add(item);
                }
            }
        }
    }
}
