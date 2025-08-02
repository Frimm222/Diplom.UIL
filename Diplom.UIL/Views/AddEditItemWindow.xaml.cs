using Diplom.BLL.Models;
using Diplom.UIL.ViewModels;
using System.Windows;

namespace Diplom.UIL.Views
{
    public partial class AddEditItemWindow : Window
    {
        public AddEditItemWindow(Item item)
        {
            InitializeComponent();
            DataContext = new AddEditItemViewModel(item);
        }
    }
}
