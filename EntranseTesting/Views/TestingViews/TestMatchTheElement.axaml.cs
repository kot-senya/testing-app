using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using EntranseTesting.Models;
using EntranseTesting.ViewModels;
using System.Diagnostics;

namespace EntranseTesting;

public partial class TestMatchTheElement : UserControl
{
    public TestMatchTheElement()
    {
        InitializeComponent();
    }

    private void OnPointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        Debug.WriteLine("Pressed start");

        if (sender is not Border border) return;
        if (border.DataContext is not ElementOfGroup elem) return;

        if (DataContext is not MainWindowViewModel vm) return;
        vm.TestMain.TestPages.TestME.MatchLine(elem, ref border);
    }
}