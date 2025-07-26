using Diplom.DAL;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using System.Windows;

namespace Diplom.UIL.ViewModels
{
    class LoginWindowViewModel : BaseViewModel
    {
        private readonly DataBase _dataBase = ((App)Application.Current).DataBase;

        [Reactive] public string? Login { get; set; }
        [Reactive] public string? Password { get; set; }
        public ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public LoginWindowViewModel()
        {
            LoginCommand = ReactiveCommand.Create(Loginning);
        }
        public void Loginning()
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Введите ФИО и пароль");
                return;
            }
            var user = _dataBase.GetUser(Login, Password);
            if (user != null)
            {
                ((App)Application.Current).CurrentUser = user;
                var mainWindow = new MainWindow();
                mainWindow.Show();
                Application.Current.MainWindow?.Close();
                Application.Current.MainWindow = mainWindow;
            }
            else
            {
                MessageBox.Show("Неверные ФИО или пароль");
            }
        }
    }
}
