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
        [ObservableProperty] private ResultMainViewModel results;
        [ObservableProperty] private TaskMainViewModel taskMainPage;
        [ObservableProperty] bool enabledSettings = false;
        [ObservableProperty] bool enabledTaskMain = true;
        [ObservableProperty] bool enabledResults = true;

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
            EnabledResults = true;
            EditingVisible = false;
            if(Results != null)Results.Timer.Stop();
            SettingsApp = new TestSettingsViewModel();
            EditorUC = new TestSettings();
        }

        [RelayCommand]
        private void ClickToTaskMain()
        {
            EnabledTaskMain = false;
            EnabledSettings = true;
            EnabledResults = true;
            EditingVisible = true;
            if (Results != null) Results.Timer.Stop();
            TaskMainPage = new TaskMainViewModel();
            EditorUC = new TaskMain();
        }

        [RelayCommand]
        private void ClickToUserResults()
        {
            EnabledTaskMain = true;
            EnabledSettings = true;
            EnabledResults = false;
            EditingVisible = false;
            Results = new ResultMainViewModel();
            EditorUC = new ResultMain();
        }
        [RelayCommand]
        private async void SaveSettings()
        {
            try
            {
                //проверка на пустое начение
                if (SettingsApp.Settings is null)
                {
                    await MessageBoxManager.GetMessageBoxStandard("", "Значение оказалось пустым", ButtonEnum.Ok).ShowAsync();
                    return;
                }
                
                string mess = checkSetting();
                if(mess != "")
                {
                    var resultat = await MessageBoxManager.GetMessageBoxStandard("", mess, ButtonEnum.OkCancel).ShowAsync();
                    switch (resultat)
                    {
                        case ButtonResult.Ok:
                            break;
                        case ButtonResult.Cancel:
                            return;
                    }
                }

                //сохраняем в базе
                EntranceTestingContext baseConnection = new EntranceTestingContext();
                baseConnection.AppSettings.Add(SettingsApp.Settings);
                baseConnection.SaveChanges();
                await MessageBoxManager.GetMessageBoxStandard("", "Текущие настройки теста сохранены", ButtonEnum.Ok).ShowAsync();

                SettingsApp = new TestSettingsViewModel();
#if DEBUG
                Debug.WriteLine("Данные о настройках теста успешно сохранены");
#endif
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("", "По какой-то причине не удалось сохранить данные", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
            }

        }
        private string checkSetting()
        {
            string mess = "";
            string erMess = "";
            AppSetting _set = SettingsApp.Settings;
            _set.Id = 0;
            _set.DateOfChanging = DateTime.Now;

            //настройка подсказок
            if (!_set.HintVisibility)
            {
                _set.CountOfHints = 0;
            }
            else if (_set.CountOfHints == 0)
            {
                mess += "Вы включили функцию подсказок, но количество подсказок = 0. Поэтому функция подсказок отключена\n";
                _set.HintVisibility = false;
            }
            
            //настройка оценивания
            if(_set.Raiting5 > _set.CountOfQuestions)
            {
                _set.Raiting5 = _set.CountOfQuestions;
                mess += "Критерии оценивания для оценки 5 изменены, так как количество вопросов было меньше чем количество требуемых баллов\n";
            }
            if (_set.Raiting4 >= _set.Raiting5)
            {
                _set.Raiting4 = _set.Raiting5 - 1;
                mess += "Критерии оценивания для оценки 4 изменены, так как количество баллов больше чем для оценки 5\n";
            }
            if (_set.Raiting3 >= _set.Raiting4)
            {
                _set.Raiting3 = _set.Raiting4 - 1;
                mess += "Критерии оценивания для оценки 3 изменены, так как количество баллов больше чем для оценки 4\n";
            }

            //проверка времени для прохождения задания
            if(_set.Time < new TimeSpan(0,0,45) * _set.CountOfQuestions)
            {
                erMess += "Времени, которое выделенно на прохождение данного теста может не хватить.\nРекумендуемое время: " + (new TimeSpan(0, 0, 45) * _set.CountOfQuestions) + " (45 секунд на задание)";
            }

            SettingsApp.Settings = _set;
            
            mess = (mess == "") ? mess : "\tИнформация об изменениях*\n" + mess;
            erMess = (erMess == "") ? erMess : "\n\tСправочная информация\n" + erMess;
            return  mess + erMess;
        }
    }
}