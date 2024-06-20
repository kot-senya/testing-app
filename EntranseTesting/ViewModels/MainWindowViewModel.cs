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
using System.Threading.Tasks;
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
        [ObservableProperty] private bool _isOpenAuthorization = false;
        [ObservableProperty] private bool _isTextChangeOpen = false;
        [ObservableProperty] private bool _isHintOpen = false;
        [ObservableProperty] private bool _textChangeVisible = false;
        [ObservableProperty] private bool _hintVisible = false;
        [ObservableProperty] private bool _info = true;
        [ObservableProperty] private bool _buttonVisible = false;
        [ObservableProperty] private bool _userResultVisible = false;

        //локальные классы для работы
        [ObservableProperty] InterfaceSettings iS = new InterfaceSettings();//пользовательский интерфейс
        [ObservableProperty] Messages mess = new Messages();//большие сообщения

        //view model для работы с основными user control
        [ObservableProperty] TestMainViewModel testMain = new TestMainViewModel();
        [ObservableProperty] EditorPageViewModel editorPages = new EditorPageViewModel(false);
        [ObservableProperty] UserResultViewModel userResult;

        //кнопки панели
        [ObservableProperty] bool isAuth = true;
        [ObservableProperty] bool isExit = false;
        private bool timerEnd = false;
        private bool reSave = false;

        //для авторизации
        [ObservableProperty] string login = "";
        [ObservableProperty] string password = "";

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
                    ButtonVisible = true;
                    TestMain.TestPages = new TestPageViewModel();
                    UC = new TestPage();
                    //настраиваем ответы
                    Response.userSession = new UserSession()
                    {
                        Date = DateTime.Now,
                        Time = new TimeSpan(0, 0, 0),
                        UserGroup = TestMain.GroupUser.Replace(" ", "").ToString(),
                        UserName = TestMain.NameUser.ToString(),
                        CountHint = 0,
                        IdAppSettings = TestMain.TestPages.SettingTest.Id
                    };
                    Response.timer.Interval = TestMain.TestPages.SettingTest.Time;
                    Response.timer.Tick += stopTimer;
                    Response.timer.Start();
                    timerEnd = false;
                    reSave = false;
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

        private void stopTimer(object? sender, EventArgs e)
        {
            timerEnd = true;
            SaveResults();
        }
        [RelayCommand]
        private void OpenAuthorization()
        {
            IsOpenAuthorization = true;
        }
        [RelayCommand]
        private async void Authorization()
        {
            EntranceTestingContext connection = new EntranceTestingContext();
            RootUser user = connection.RootUsers.FirstOrDefault(tb => tb.Login == Login && tb.Password == Password);
            if (user != null)
            {
                Info = false;
                IsHintOpen = false;
                HintVisible = false;
                IsOpenAuthorization = false;

                IsAuth = false;
                IsExit = true;
                EditorPages = new EditorPageViewModel(false);
                UC = new EditorPage();
            }
            else
                await MessageBoxManager.GetMessageBoxStandard("", "Пользователь не найден").ShowAsync();

        }

        [RelayCommand]
        private async void ToBack()
        {
            if (typeof(TaskEditor) == UC.GetType())
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
                                    Question q = connection.Questions
                                    .Include(tb => tb.ElementOfArrangements)
                                    .Include(tb => tb.ElementOfEqualities)
                                    .Include(tb => tb.ElementOfChooses)
                                    .Include(tb => tb.TextOfPuttings)
                                    .Include(tb => tb.Groups)
                                    .FirstOrDefault(tb => tb.Id == EditorPages.TaskEditorPage.Q.Id);
                                    bool flag = q.ElementOfArrangements.Count == 0 && q.ElementOfEqualities.Count == 0 && q.ElementOfChooses.Count == 0 && q.TextOfPuttings.Count == 0 && q.Groups.Count == 0;
                                    if (flag)
                                    {
                                        connection.Questions.Remove(q);
                                        connection.SaveChanges();
                                    }
                                }
                                break;
                            }
                        case ButtonResult.No:
                            {
                                return;
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
            if (typeof(EditorPage) == UC.GetType())
            {
                EditorPages.EditingVisible = false;
                if (EditorPages.Results != null) EditorPages.Results.Timer.Stop();
            }
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

                foreach (var item in TestMain.TestPages.ProgressButtons)
                    item.Check = false;
                ButtonVisible = false;
                Response.timer.Stop();
            }
            Login = "";
            Password = "";
            Info = true;
            IsAuth = true;
            IsExit = false;
            UserResultVisible = false;
            TestMain = new TestMainViewModel();
            UC = new TestMain();
        }

        public void AddQuestion()
        {
            EditorPages.EditingVisible = false;
            EditorPages.TaskEditorPage = new TaskEditorViewModel();
            UC = new TaskEditor();
        }

        public void EditQuestion(int idQuestion)
        {
            EditorPages.EditingVisible = false;
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
                                Question q = connection.Questions
                                    .Include(tb => tb.ElementOfArrangements)
                                    .Include(tb => tb.ElementOfEqualities)
                                    .Include(tb => tb.ElementOfChooses)
                                    .Include(tb => tb.TextOfPuttings)
                                    .Include(tb => tb.Groups)
                                    .FirstOrDefault(tb => tb.Id == EditorPages.TaskEditorPage.Q.Id);
                                bool flag = q.ElementOfArrangements.Count == 0 && q.ElementOfEqualities.Count == 0 && q.ElementOfChooses.Count == 0 && q.TextOfPuttings.Count == 0 && q.Groups.Count == 0;
                                if (flag)
                                {
                                    connection.Questions.Remove(q);
                                    connection.SaveChanges();
                                }
                            }
                            EditorPages.EditingVisible = true;
                            EditorPages = new EditorPageViewModel(true);
                            UC = new EditorPage();
                            break;
                        }
                    case ButtonResult.No:
                        {
                            return;
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

        // Метод для изменения содержимого страницы теста        
        public async void NextQuestion()
        {
            TestMain.changeResponse();

            if (TestMain.TestPages.NumQuestion < TestMain.TestPages.CountQuestion)
            {
                TestMain.TestPages.ProgressButtons[TestMain.TestPages.NumQuestion - 1].Active = false;
                TestMain.TestPages.NumQuestion++;
            }
            else if (TestMain.TestPages.NumQuestion == TestMain.TestPages.CountQuestion)//если конец теста
            {
                if (TestMain.TestPages.QuestionsCollection.Count() != TestMain.TestPages.ProgressButtons.Where(tb => tb.Check == true).Count())
                {
                    var result = await MessageBoxManager.GetMessageBoxStandard("", "У вас остались невыполненные заданияю Вы точно хотите закончить?", ButtonEnum.YesNo).ShowAsync();
                    switch (result)
                    {
                        case ButtonResult.Yes:
                            break;
                        case ButtonResult.No:
                            return;
                    }
                }
                SaveResults();
            }

            TestMain.TestPages.ProgressButtons[TestMain.TestPages.NumQuestion - 1].Active = true;

            if (TestMain.TestPages.NumQuestion == TestMain.TestPages.CountQuestion)
                TestMain.TestPages.ButtonValue = "Завершить тест";

            while (!TestMain.TestPages.TakeProgressButtons.Contains(TestMain.TestPages.ProgressButtons[TestMain.TestPages.NumQuestion - 1]))
            {
                if (TestMain.TestPages.NumQuestion > TestMain.TestPages.TakeProgressButtons.Last().Num)
                    TestMain.TestPages.SkipItem++;
                else
                    TestMain.TestPages.SkipItem--;
            }

            TestMain.TestPages.changingPage();
        }
        public async void SaveResults()
        {
            if (reSave) return;
            try
            {
                foreach (var item in TestMain.TestPages.ProgressButtons)
                    item.Check = false;
                ButtonVisible = false;
                TestMain.changeResponse();//сохраняем ответ последнего задания
                EntranceTestingContext connection = new EntranceTestingContext();
                Response.userSession.Time = DateTime.Now.TimeOfDay - Response.userSession.Date.TimeOfDay;
                Response.userSession.UserGroup = Response.userSession.UserGroup.ToUpper().Trim();
                Response.userSession.UserName = Response.userSession.UserName.Trim();
                Response.userSession.CountHint = TestMain.TestPages.HintCount;
                connection.UserSessions.Add(Response.userSession);
                connection.SaveChanges();
                for (int i = 0; i < Response.responseUsers.Count; i++)
                {
                    Response.responseUsers[i].IdSession = Response.userSession.Id;
                    connection.UserResponses.Add(Response.responseUsers[i]);
                }
                connection.SaveChanges();

                reSave = true;
                if (timerEnd)
                {
                    UC = new TestEnd();
                }
                else
                {
                    Response.timer.Stop();
                    EndTest();
                }
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("", "По какой-то причине не удалось сохранить данные", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
            }
        }
        [RelayCommand]
        private void EndTest()
        {
            //страница итогов
            if (TestMain.TestPages.SettingTest.ResultVisibiliry)
            {
                TestMain.TestResults = new TestResultViewModel(null);
                UC = new TestResult();
            }
            else
            {
                TestMain = new TestMainViewModel();
                UC = new TestMain();
            }
        }

        public void ClickToUserResult(int idSession)
        {
            UserResultVisible = true;
            UserResult = new UserResultViewModel(idSession);
            UC = new UserResultPage();
            EditorPages.Results.Timer.Stop();
        }
        public void ClickToAllResult()
        {
            UserResultVisible = false;
            UC = new EditorPage();
            EditorPages.Results.Timer.Start();
        }
    }
}

