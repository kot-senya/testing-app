using Avalonia;
using Avalonia.Controls;
using DynamicData.Binding;
using EntranseTesting.Models;
using EntranseTesting.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;

namespace EntranseTesting.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TestContent.SizeChanged += Resize;
            MyFontSize.ValueChanged += ChangeSize;
        }

        private void ChangeSize(object? sender, NumericUpDownValueChangedEventArgs e)
        {
            Resize();
        }

        private void Resize(object? sender, SizeChangedEventArgs e)
        {
            Resize();
        }

        public void Resize()
        {
            //��������� ����
            double widht = TestContent.Bounds.Width - 15.0 * 2;
            //������� vm
            if (DataContext is not MainWindowViewModel vm) return;
            //����� ������
            double widhtButton = 50.0 * (double)MyFontSize.Value / 14.0;
            double widhrButtonNav = 23.0 * (double)MyFontSize.Value / 14.0;
            widhrButtonNav = (widhrButtonNav < 25) ? 25 : widhrButtonNav;
            widht -= widhrButtonNav * 2;//���������� ����� ��� ����
            //������� ��������� �����������
            int count = (int)Math.Round(widht / (widhtButton + 5.0));
            if (count * (widhtButton + 5.0) > widht)
                count--;
            
            vm.TestMain.TestPages.TakeItem = count;

            //��������� �������
            List<ItemProgressButton> items = vm.TestMain.TestPages.ProgressButtons;
            int num = vm.TestMain.TestPages.NumQuestion;
            ItemProgressButton item = vm.TestMain.TestPages.ProgressButtons[num - 1];
            
            while (!vm.TestMain.TestPages.TakeProgressButtons.Contains(item))
            {
                if (num > vm.TestMain.TestPages.TakeProgressButtons.Last().Num)
                    vm.TestMain.TestPages.SkipItem++;
                else
                    vm.TestMain.TestPages.SkipItem--;
            }
            while (vm.TestMain.TestPages.TakeProgressButtons.Count < count)
            {
                vm.TestMain.TestPages.SkipItem--;
            }
            InterfaceSettings.take = count;
        }
    }
}