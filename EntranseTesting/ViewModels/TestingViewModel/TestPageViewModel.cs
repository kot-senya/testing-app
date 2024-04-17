using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;
using EntranseTesting.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace EntranseTesting.ViewModels
{
    public partial class TestPageViewModel : ObservableObject
    {
        [ObservableProperty] AppSetting settingTest = new AppSetting();
        //контейнер дл€ вопросов
        [ObservableProperty] UserControl testUC;
        //данные о тесте
        [ObservableProperty] string buttonValue = "—ледующий вопрос";
        [ObservableProperty] int numQuestion = 0;
        [ObservableProperty] int countQuestion = 0;
        [ObservableProperty] List<Question> questionsCollection;
        [ObservableProperty] List<ItemProgressButton> progressButtons = new List<ItemProgressButton>();

        //view model дл€ работы с тестом
        [ObservableProperty] TestArrangementOfElementsViewModel testAE;//перестановка элемента
        [ObservableProperty] TestChoosingAnAnswerFromASetViewModel testCAFS;//подстановка ответов в текст
        [ObservableProperty] TestChoosing1AnswerViewModel testC1A;//выбор 1 варианта
        [ObservableProperty] TestChoosingMultipleAnswersViewModel testCMA;//выбор нескольких вариантов ответа
        [ObservableProperty] TestMatchTheElementViewModel testME;//соотношение элементов
        [ObservableProperty] TestMatchTheValueViewModel testMV;//соотношение величин
        public TestPageViewModel()
        {
            EntranceTestingContext baseConnection = new EntranceTestingContext();
            SettingTest = baseConnection.AppSettings.ToList().Last();
            CountQuestion = SettingTest.CountOfQuestions;

            //получаем весь список вопросов из базы
            QuestionsCollection = baseConnection.Questions.Include(tb => tb.IdCategoryNavigation).Where(tb => tb.InTest == true).ToList();          
            Random.Shared.Shuffle(CollectionsMarshal.AsSpan(QuestionsCollection));//перемешиваем вопросы            
            QuestionsCollection = QuestionsCollection.Take(CountQuestion).ToList();//берем нужное количество вопросов
            
            //кнопки навигации
            CountQuestion = (QuestionsCollection.Count == SettingTest.CountOfQuestions)? SettingTest.CountOfQuestions: (QuestionsCollection.Count > SettingTest.CountOfQuestions) ? SettingTest.CountOfQuestions: QuestionsCollection.Count;
            for (int i = 0; i < CountQuestion; i++)
                ProgressButtons.Add(new ItemProgressButton(i + 1));
            ProgressButtons[0].Active = true;
            
            //заполн€ем массив с ответами пользовател€
            Response.responseUsers = new List<UserResponse>();
            for (int i = 0; i < CountQuestion; i++)
                Response.responseUsers.Add(new UserResponse() { IdQuestion = QuestionsCollection[i].Id });
            //начинаем тест
            NumQuestion = 1;
            changingPage();
        }        

        public void changingPage()
        {
            if (QuestionsCollection.Count > 0)
            {
                switch (QuestionsCollection[NumQuestion - 1].IdCategoryNavigation.Name)
                {
                    case "¬опрос с 1 вариантом ответа":
                        {
                            TestC1A = new TestChoosing1AnswerViewModel(QuestionsCollection[NumQuestion - 1].Id);
                            TestUC = new TestChoosing1Answer();
                            break;
                        }
                    case "¬опрос с несколькими вариантами ответа":
                        {
                            TestCMA = new TestChoosingMultipleAnswersViewModel(QuestionsCollection[NumQuestion - 1].Id);
                            TestUC = new TestChoosingMultipleAnswers();
                            break;
                        }
                    case "√оризонтальное упор€дочивание элементов":
                        {
                            TestAE = new TestArrangementOfElementsViewModel(QuestionsCollection[NumQuestion - 1].Id, Orientation.Horizontal);
                            TestUC = new TestArrangementOfElements();
                            break;
                        }
                    case "¬ертикальное упор€дочивание элементов":
                        {
                            TestAE = new TestArrangementOfElementsViewModel(QuestionsCollection[NumQuestion - 1].Id, Orientation.Vertical);
                            TestUC = new TestArrangementOfElements();
                            break;
                        }
                    case "—оотношение величин":
                        {
                            TestMV = new TestMatchTheValueViewModel(QuestionsCollection[NumQuestion - 1].Id);
                            TestUC = new TestMatchTheValue();
                            break;
                        }
                    case "—оотношение элементов":
                        {
                            TestME = new TestMatchTheElementViewModel(QuestionsCollection[NumQuestion - 1].Id);
                            TestUC = new TestMatchTheElement();
                            break;
                        }
                    case "ѕодстановка ответов":
                        {
                            TestCAFS = new TestChoosingAnAnswerFromASetViewModel(QuestionsCollection[NumQuestion - 1].Id);
                            TestUC = new TestChoosingAnAnswerFromASet();
                            break;
                        }
                }
            }
        }
    }
}