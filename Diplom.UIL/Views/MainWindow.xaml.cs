using System.Windows;
using System.Windows.Input;

namespace Diplom.UIL;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
    {
        dynamic viewModel = DataContext;
        viewModel.ExtendedWindow();
    }
}
