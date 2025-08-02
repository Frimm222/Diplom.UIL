using Diplom.BLL.Models;
using Diplom.UIL.ViewModels;
using System.Windows;

namespace Diplom.UIL.Views
{
    /// <summary>
    /// Логика взаимодействия для ExtendedWindow.xaml
    /// </summary>
    public partial class ExtendedWindow : Window
    {
        public ExtendedWindow(Item item)
        {
            InitializeComponent();
            DataContext = new ExtendedWindowViewModel(item);
        }
    }
}
