using Diplom.DAL;
using ReactiveUI;
using System.Windows;

namespace Diplom.UIL.ViewModels
{
    public class BaseViewModel : ReactiveObject
    {
        protected internal readonly DataBase _dataBase = ((App)Application.Current).DataBase;
    }
}
