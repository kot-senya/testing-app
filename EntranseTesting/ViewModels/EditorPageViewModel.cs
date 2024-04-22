using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EntranseTesting.Models;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace EntranseTesting.ViewModels
{
    public partial class EditorPageViewModel : ObservableObject
    {
        [ObservableProperty] private UserControl editorUC = new TestSettings();
        [ObservableProperty] private TestSettingsViewModel settingsApp;
        [ObservableProperty] private TaskEditorViewModel taskEditorPage;
        [ObservableProperty] private TaskMainViewModel taskMainPage;
        [ObservableProperty] bool enabledSettings = false;
        [ObservableProperty] bool enabledTaskMain = true;

        [ObservableProperty] private bool _editingVisible = false;

        public EditorPageViewModel(bool Editor)
        {
            SettingsApp = new TestSettingsViewModel();
            if (Editor) ClickToTaskMain();
        }

        [RelayCommand]
        private void ClickToSettings()
        {
            EnabledSettings = false;
            EnabledTaskMain = true;
            EditingVisible = false;
            SettingsApp = new TestSettingsViewModel();
            EditorUC = new TestSettings();
        }

        [RelayCommand]
        private void ClickToTaskMain()
        {
            EnabledTaskMain = false;
            EnabledSettings = true;
            EditingVisible = true;
            TaskMainPage = new TaskMainViewModel();
            EditorUC = new TaskMain();
        }

        [RelayCommand]
        private async void SaveSettings()
        {
            try
            {
                AppSetting _set = SettingsApp.Settings;

                //проверка на пустое начение
                if (_set is null)
                {
                    await MessageBoxManager.GetMessageBoxStandard("", "«начение оказалось пустым", ButtonEnum.Ok).ShowAsync();
                    return;
                }

                //провер€ем значени€, добавл€ем то чего не было
                _set.DateOfChanging = DateTime.Now;
                if (!_set.HintVisibility)
                { 
                    _set.HalfVisibility = false;
                    _set.CountOfHints = 0;
                    _set.HalfCost = 0;
                }
                _set.Id = 0;
                _set.ResultVisibiliry = true;
                //сохран€ем в базе
                EntranceTestingContext baseConnection = new EntranceTestingContext();
                baseConnection.AppSettings.Add(_set);
                baseConnection.SaveChanges();
                await MessageBoxManager.GetMessageBoxStandard("", "“екущие настройки теста сохранены", ButtonEnum.Ok).ShowAsync();

                SettingsApp = new TestSettingsViewModel();
#if DEBUG
                Debug.WriteLine("ƒанные о настройках теста успешно сохранены");
#endif
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("", "ѕо какой-то причине не удалось сохранить данные", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
            }

        }
   }
}