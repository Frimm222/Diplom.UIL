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
        [Reactive] public string? SearchText { get; set; } = null;
        [Reactive] public string? LogText { get; set; } = string.Empty;
        [Reactive] public bool ShowDeleted { get; set; } = false;
        public ReactiveCommand<Unit, Unit> AddItemCommand { get; }
        public ReactiveCommand<Unit, Unit> EditItemCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteItemCommand { get; }
        public ReactiveCommand<Unit, Unit> SearchCommand { get; }
        public ReactiveCommand<Unit, Unit> LogoutCommand { get; }

        public MainWindowViewModel()
        {
            AddItemCommand = ReactiveCommand.CreateFromTask(Add);
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
            EditItemCommand = ReactiveCommand.CreateFromTask(Edit, canExecuteEdit);
            DeleteItemCommand = ReactiveCommand.CreateFromTask(Delete, canExecuteEdit);
            var canExecuteSearch = this.WhenAnyValue(
                x => x.SearchText,
                (searchText) => !string.IsNullOrEmpty(searchText));
            SearchCommand = ReactiveCommand.Create(Search, canExecuteSearch);
            this.WhenAnyValue(x => x.ShowDeleted)
                .Subscribe(async _ =>
                {
                    await LoadItemsAsync();
                });
            this.WhenAnyValue(x => x.SearchText)
                .Throttle(TimeSpan.FromSeconds(0.3), RxApp.TaskpoolScheduler)
                .Select(query => query?.Trim())
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => Search());
            _ = LoadLogsAsync();

        }
        private async Task LoadItemsAsync()
        {
            var items = await _dataBase.GetAll();
            Items.Clear();
            if (ShowDeleted)
            {
                foreach (var item in items)
                {
                    Items.Add(item); // Добавляем все элементы, включая удаленные
                }
                return;
            }
            else
            {
                foreach (var item in items)
                {
                    if (item.is_deleted) continue; // Пропускаем удаленные элементы
                    else { Items.Add(item); }
                }
            }

        }
        private async Task LoadLogsAsync()
        {
            var logs = await _dataBase.GetLogsAsync();
            LogText = string.Empty;
            foreach (var log in logs)
            {
                LogText += $"{log.time} {log.text}\n";
            }
        }
        private async Task Add()
        {
            var viewModel = new AddEditItemWindow(new Item());
            viewModel.ShowDialog();
            await LoadItemsAsync();
            await LoadLogsAsync();
        }
        private async Task Edit()
        {
            var viewModel = new AddEditItemWindow(SelectedItem);
            viewModel.ShowDialog();
            await LoadItemsAsync();
            await LoadLogsAsync();
        }
        private async Task Delete()
        {
            var res = MessageBox.Show("Удалить позицию?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (SelectedItem is not null && MessageBoxResult.Yes == res)
            {
                _dataBase.Delete(SelectedItem);
                await LoadItemsAsync();
                await LoadLogsAsync();
            }
        }
        private async void Search()
        {
            if (SearchText is not null)
            {
                var items = await _dataBase.GetAll();
                Items.Clear();
                if (ShowDeleted)
                {
                    foreach (var item in items)
                    {
                        if (item.name.ToLower().Contains(SearchText.ToLower()) || item.description.ToLower().Contains(SearchText.ToLower()) || item.barcode.ToLower().Contains(SearchText.ToLower()))
                        {
                            Items.Add(item);
                        }
                    }
                    return;
                }
                else
                {
                    foreach (var item in items)
                    {
                        if ((item.name.ToLower().Contains(SearchText.ToLower()) || item.description.ToLower().Contains(SearchText.ToLower()) || item.barcode.ToLower().Contains(SearchText.ToLower())) && !item.is_deleted)
                        {
                            Items.Add(item);
                        }
                    }
                }
                
            }
        }
    }
}
