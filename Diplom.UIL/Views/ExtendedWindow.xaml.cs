using Diplom.BLL.Models;
using Diplom.UIL.ViewModels;
using System.Windows;

namespace Diplom.UIL.Views
{
    public partial class ExtendedWindow : Window
    {
        public ExtendedWindow(Item item)
        {
            InitializeComponent();
            DataContext = new ExtendedWindowViewModel(item);
        }
    }
}
