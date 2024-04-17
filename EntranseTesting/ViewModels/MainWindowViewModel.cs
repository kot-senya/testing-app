using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EntranseTesting.Models;
using HarfBuzzSharp;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace EntranseTesting.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public static EntranceTestingContext baseConnection = new EntranceTestingContext();

        //основные элементы окна 
        [ObservableProperty] private UserControl uC = new TestMain();
        [ObservableProperty] private bool _isPaneOpen = false;
        [ObservableProperty] private bool _isTextChangeOpen = false;
        [ObservableProperty] private bool _isHintOpen = false;
        [ObservableProperty] private bool _textChangeVisible = false;
        [ObservableProperty] private bool _hintVisible = false;
        [ObservableProperty] private bool _info = true;

        //локальные классы для работы
        [ObservableProperty] InterfaceSettings iS = new InterfaceSettings();//пользовательский интерфейс
        [ObservableProperty] Messages mess = new Messages();//большие сообщения

        //view model для работы с основными user control
        [ObservableProperty] TestMainViewModel testMain = new TestMainViewModel();
        [ObservableProperty] EditorPageViewModel editorPages = new EditorPageViewModel(false);

        //кнопки панели
        [ObservableProperty] bool isAuth = true;
        [ObservableProperty] bool isExit = false;


        [RelayCommand]
        private void IsClickPain()
        {
            IsPaneOpen = !IsPaneOpen;
            TextChangeVisible = (IsPaneOpen && IsTextChangeOpen);
            HintVisible = (IsPaneOpen && IsHintOpen && Info);
        }

        [RelayCommand]
        private void IsTextClick()
        {
            if (!IsTextChangeOpen)
            {
                IsPaneOpen = true;
                IsTextChangeOpen = true;
                TextChangeVisible = true;
            }
            else
            {
                IsTextChangeOpen = false;
                TextChangeVisible = false;
            }
        }
        [RelayCommand]
        private void IsHintClick()
        {
            if (!IsHintOpen)
            {
                IsPaneOpen = true;
                IsHintOpen = true;
                HintVisible = true;
            }
            else
            {
                IsHintOpen = false;
                HintVisible = false;
            }
        }
        [RelayCommand]
        private async void StartTesting()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(TestMain.NameUser) && !string.IsNullOrWhiteSpace(TestMain.GroupUser))
                {
                    IsAuth = false;
                    IsExit = true;
                    TestMain.TestPages = new TestPageViewModel();
                    UC = new TestPage();
                    //настраиваем ответы
                    Response.userSession = new UserSession()
                    {
                        Date = DateTime.Now,
                        UserGroup = TestMain.GroupUser.ToString(),
                        UserName = TestMain.NameUser.ToString(),
                        CountHint = 0,
                        IdAppSettings = TestMain.TestPages.SettingTest.Id
                    };
                }
                else
                {
                    await MessageBoxManager.GetMessageBoxStandard("Ошибка формы", "Заполните поля ФИО и Группа", ButtonEnum.Ok).ShowAsync();
                }

            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("Ошибка формы", "Возникла неопознаная ошибка. Возможно вы сделали сто-то не так", ButtonEnum.Ok).ShowAsync();
            }

        }

        [RelayCommand]
        private void Authorization()
        {
            Info = false;
            IsHintOpen = false;
            HintVisible = false;

            IsAuth = false;
            IsExit = true;
            EditorPages = new EditorPageViewModel(false);
            UC = new EditorPage();
        }

        [RelayCommand]
        private async void ToBack()
        {
            if (typeof(TestPage) == UC.GetType())
            {
                var result = await MessageBoxManager.GetMessageBoxStandard("Выход из теста", "Если вы выйдите из теста, то результат не сохраниться", ButtonEnum.YesNo).ShowAsync();
                switch (result)
                {
                    case ButtonResult.Yes:
                        {
                            break;
                        }
                    case ButtonResult.No:
                        {
                            return;
                        }
                }
            }
            Info = true;

            IsAuth = true;
            IsExit = false;
            UC = new TestMain();
        }

        public void AddQuestion()
        {
            EditorPages.AddQuestionVisible = false;
            EditorPages.TaskEditorPage = new TaskEditorViewModel();
            UC = new TaskEditor();
        }

        public void EditQuestion(int idQuestion)
        {
            EditorPages.AddQuestionVisible = false;
            EditorPages.TaskEditorPage = new TaskEditorViewModel(idQuestion);
            UC = new TaskEditor();
        }

        public async void ExitFromEditor()
        {
            try
            {
                var result = await MessageBoxManager.GetMessageBoxStandard("Выйти из редактора", "Вы действительно хотите выйти из редактора? Все введенные вами изменения могут удалиться", ButtonEnum.YesNo).ShowAsync();
                switch (result)
                {
                    case ButtonResult.Yes:
                        {
                            if (EditorPages.TaskEditorPage.Header == "Добавление вопроса")
                            {
                                EntranceTestingContext connection = new EntranceTestingContext();
                                Question q = connection.Questions.FirstOrDefault(tb => tb.Id == EditorPages.TaskEditorPage.Q.Id);
                                connection.Questions.Remove(q);
                                connection.SaveChanges();
                            }
                            EditorPages.AddQuestionVisible = true;
                            EditorPages = new EditorPageViewModel(true);
                            UC = new EditorPage();
                            break;
                        }
                    case ButtonResult.No:
                        {
                            break;
                        }
                }

            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.Write(ex.Message);
#endif
            }
        }
        public async void LoadImageForQuestion()
        {
            try
            {
                var topLevel = TopLevel.GetTopLevel(UC);
                var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = "Выберите изображения для вопроса",
                    AllowMultiple = true,
                    FileTypeFilter = new[] { FilePickerFileTypes.ImageAll }
                });

                if (files.Count >= 1)
                {
                    EntranceTestingContext connection = new EntranceTestingContext();
                    foreach (var file in files)
                    {
                        QuestionImage qImage = new QuestionImage();
                        qImage.IdQuestion = EditorPages.TaskEditorPage.Q.Id;
                        qImage.Image = File.ReadAllBytes(file.Path.ToString().Replace("file:///", ""));
                        EditorPages.TaskEditorPage.QImage.Add(qImage);
                        connection.QuestionImages.Add(qImage);
                    }
                    connection.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("Ошибка добавления", "Во время добавления возникла ошибка", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.Write(ex.Message);
#endif
            }

        }

        public async void LoadImageForHint(int idHint)
        {
            try
            {
                var topLevel = TopLevel.GetTopLevel(UC);
                var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = "Выберите изображения для вопроса",
                    AllowMultiple = true,
                    FileTypeFilter = new[] { FilePickerFileTypes.ImageAll }
                });

                if (files.Count >= 1)
                {
                    QuestionHint qh = EditorPages.TaskEditorPage.QHint.FirstOrDefault(tb => tb.IdHint == idHint);
                    int index = EditorPages.TaskEditorPage.QHint.IndexOf(qh);

                    EntranceTestingContext connection = new EntranceTestingContext();
                    foreach (var file in files)
                    {
                        HintImage hImage = new HintImage();
                        hImage.IdHint = idHint;
                        hImage.Image = File.ReadAllBytes(file.Path.ToString().Replace("file:///", ""));
                        connection.HintImages.Add(hImage);
                    }
                    connection.SaveChanges();
                    EditorPages.TaskEditorPage.QHint = connection.QuestionHints.Include(tb => tb.IdHintNavigation).ThenInclude(tb => tb.HintImages).Where(tb => tb.IdQuestion == qh.IdQuestion).ToList();
                }
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("Ошибка добавления", "Во время добавления возникла ошибка", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.Write(ex.Message);
#endif
            }
        }

    }
}

