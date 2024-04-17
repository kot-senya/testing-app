using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using EntranseTesting.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntranseTesting.Models
{
    public partial class InterfaceSettings: ObservableObject
    {
        [ObservableProperty]
        private List<FontFamily> fonts = new List<FontFamily>() { "Arial", "Comic Sans MS", "Copperplate Gothic","Gabriola", "Georgia", "Impact", "Times New Roman" };
        [ObservableProperty]
        private FontFamily selectedFont = "Comic Sans MS";
        [ObservableProperty]
        private int myFontSize = 16;
    }
}
