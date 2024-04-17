using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using EntranseTesting.Models;
using EntranseTesting.ViewModels;
using System.Diagnostics;

namespace EntranseTesting;

public partial class TestArrangementOfElements : UserControl
{
    private Point _ghostPosition = new(0, 0);
    private readonly Point _mouseOffset = new(-5, -5);
    private Point _startItemPosition = new(0, 0);
    private Point _endItemPosition = new(0, 0);
    public TestArrangementOfElements()
    {
        InitializeComponent();
        AddHandler(DragDrop.DragOverEvent, DragOver);
        AddHandler(DragDrop.DropEvent, Drop);
    }
    
    protected override void OnLoaded(RoutedEventArgs e)
    {
        GhostItem.IsVisible = false;
        base.OnLoaded(e);
    }
    private async void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        Debug.WriteLine("DoDrag start");

        if (sender is not Border border) return;
        if (border.DataContext is not ElementOfArrangement elem) return;

        var ghostPos = GhostItem.Bounds.Position;
        _ghostPosition = new Point(ghostPos.X + _mouseOffset.X, ghostPos.Y + _mouseOffset.Y);

        var mousePos = e.GetPosition(MainContainer);
        var offsetX = mousePos.X - ghostPos.X;
        var offsetY = mousePos.Y - ghostPos.Y + _mouseOffset.X;
        GhostItem.RenderTransform = new TranslateTransform(offsetX, offsetY);

        _startItemPosition = new Point(offsetX, offsetY);
        elem.Width = border.Bounds.Width;
        elem.Height = border.Bounds.Height;

        if (DataContext is not MainWindowViewModel vm) return;
        vm.TestMain.TestPages.TestAE.StartDrag(elem);

        GhostItem.IsVisible = true;

        var dragData = new DataObject();
        dragData.Set(TestArrangementOfElementsViewModel.CustomFormat, elem);
        var result = await DragDrop.DoDragDrop(e, dragData, DragDropEffects.Move);
        Debug.WriteLine($"DragAndDrop result: {result}");
        GhostItem.IsVisible = false;
    }

    private void DragOver(object? sender, DragEventArgs e)
    {
        var currentPosition = e.GetPosition(MainContainer);

        var offsetX = currentPosition.X - _ghostPosition.X;
        var offsetY = currentPosition.Y - _ghostPosition.Y;

        GhostItem.RenderTransform = new TranslateTransform(offsetX, offsetY);
        _endItemPosition = new Point(offsetX, offsetY);

        e.DragEffects = DragDropEffects.Move;
        if (DataContext is not MainWindowViewModel vm) return;
        var data = e.Data.Get(TestArrangementOfElementsViewModel.CustomFormat);
        if (data is not ElementOfArrangement elem) return;
    }
    private void Drop(object? sender, DragEventArgs e)
    {
        Debug.WriteLine("Drop");

        var data = e.Data.Get(TestArrangementOfElementsViewModel.CustomFormat);

        if (data is not ElementOfArrangement elem)
        {
            Debug.WriteLine("No task item");
            return;
        }

        if (DataContext is not MainWindowViewModel vm) return;
        vm.TestMain.TestPages.TestAE.Drop(elem, _startItemPosition, _endItemPosition);
    }
}