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
                //�������� �� ������ �������
                if (SettingsApp.Settings is null)
                {
                    await MessageBoxManager.GetMessageBoxStandard("", "�������� ��������� ������", ButtonEnum.Ok).ShowAsync();
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

                //��������� � ����
                EntranceTestingContext baseConnection = new EntranceTestingContext();
                baseConnection.AppSettings.Add(SettingsApp.Settings);
                baseConnection.SaveChanges();
                await MessageBoxManager.GetMessageBoxStandard("", "������� ��������� ����� ���������", ButtonEnum.Ok).ShowAsync();

                SettingsApp = new TestSettingsViewModel();
#if DEBUG
                Debug.WriteLine("������ � ���������� ����� ������� ���������");
#endif
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("", "�� �����-�� ������� �� ������� ��������� ������", ButtonEnum.Ok).ShowAsync();
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

            //��������� ���������
            if (!_set.HintVisibility)
            {
                _set.CountOfHints = 0;
            }
            else if (_set.CountOfHints == 0)
            {
                mess += "�� �������� ������� ���������, �� ���������� ��������� = 0. ������� ������� ��������� ���������\n";
                _set.HintVisibility = false;
            }
            
            //��������� ����������
            if(_set.Raiting5 > _set.CountOfQuestions)
            {
                _set.Raiting5 = _set.CountOfQuestions;
                mess += "�������� ���������� ��� ������ 5 ��������, ��� ��� ���������� �������� ���� ������ ��� ���������� ��������� ������\n";
            }
            if (_set.Raiting4 >= _set.Raiting5)
            {
                _set.Raiting4 = _set.Raiting5 - 1;
                mess += "�������� ���������� ��� ������ 4 ��������, ��� ��� ���������� ������ ������ ��� ��� ������ 5\n";
            }
            if (_set.Raiting3 >= _set.Raiting4)
            {
                _set.Raiting3 = _set.Raiting4 - 1;
                mess += "�������� ���������� ��� ������ 3 ��������, ��� ��� ���������� ������ ������ ��� ��� ������ 4\n";
            }

            //�������� ������� ��� ����������� �������
            if(_set.Time < new TimeSpan(0,0,45) * _set.CountOfQuestions)
            {
                erMess += "�������, ������� ��������� �� ����������� ������� ����� ����� �� �������.\n������������� �����: " + (new TimeSpan(0, 0, 45) * _set.CountOfQuestions) + " (45 ������ �� �������)";
            }

            SettingsApp.Settings = _set;
            
            mess = (mess == "") ? mess : "\t���������� �� ����������*\n" + mess;
            erMess = (erMess == "") ? erMess : "\n\t���������� ����������\n" + erMess;
            return  mess + erMess;
        }
    }
}