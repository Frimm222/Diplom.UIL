using Diplom.BLL.Models;
using Diplom.DAL;
using System.Windows;

namespace Diplom.UIL;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public DataBase DataBase { get; set; } = new DataBase();
    public User? CurrentUser { get; set; }
}

