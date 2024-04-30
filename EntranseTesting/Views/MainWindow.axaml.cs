using Avalonia.Controls;
using EntranseTesting.Models;
using EntranseTesting.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

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
            if (((NumericUpDown)sender).Value > 8 && ((NumericUpDown)sender).Value < 30)
                Resize();
        }

        private void Resize(object? sender, SizeChangedEventArgs e)
        {
            if (DataContext is not MainWindowViewModel vm) return;
            Resize();
        }

        public void Resize()
        {
            //главная vm
            if (DataContext is not MainWindowViewModel vm) return;

            //доступное поле
            double widht = TestContent.Bounds.Width - 15.0 * 2;

            //длина кнопки
            double widhtButton = 50.0 * (double)MyFontSize.Value / 14.0;
            double widhrButtonNav = 23.0 * (double)MyFontSize.Value / 14.0;
            widhrButtonNav = (widhrButtonNav < 25) ? 25 : widhrButtonNav;
            widht -= widhrButtonNav * 2;//актуальная длина для поля
                                        //сколько элементов поместиться
            int count = (int)Math.Round(widht / (widhtButton + 5.0));
            if (count * (widhtButton + 5.0) > widht)
                count--;

            vm.TestMain.TestPages.TakeItem = count;

            //настройка линейки
            List<ItemProgressButton> items = vm.TestMain.TestPages.ProgressButtons;
            int num = vm.TestMain.TestPages.NumQuestion;
            ItemProgressButton item = vm.TestMain.TestPages.ProgressButtons[num - 1];


            if (vm.UC.GetType() == typeof(TestPage))
            {
                while (!vm.TestMain.TestPages.TakeProgressButtons.Contains(item))
                {
                    if (num > vm.TestMain.TestPages.TakeProgressButtons.Last().Num)
                        vm.TestMain.TestPages.SkipItem++;
                    else
                        vm.TestMain.TestPages.SkipItem--;
                }
                while (vm.TestMain.TestPages.TakeProgressButtons.Count < count)
                {
                    if (vm.TestMain.TestPages.SkipItem > 0) vm.TestMain.TestPages.SkipItem--;
                    else break;
                }
            }

            InterfaceSettings.take = count;

        }
    }
}