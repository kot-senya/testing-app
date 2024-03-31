using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Layout;
using EntranseTesting.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace EntranseTesting.ViewModels
{
    public class TestPageViewModel : ReactiveObject
    {
        EntranceTestingContext baseConnection = new EntranceTestingContext();

        UserControl testUC;
        string nameUser = "Ќе авторизован";
        int numVariant = 0;
        int numQuestion = 0;
        int countQuestion = 0;
        List<QuestionVariant> questions;

        //view model дл€ работы с тестом
        TestArrangementOfElementsViewModel testAE;//перестановка элемента
        TestChoosingAnAnswerFromASetViewModel testCAFS;//подстановка ответов в текст
        TestChoosing1AnswerViewModel testC1A;//выбор 1 варианта
        TestChoosingMultipleAnswersViewModel testCMA;//выбор нескольких вариантов ответа
        TestMatchTheElementViewModel testME;//соотношение элементов
        public TestPageViewModel(int numVariant)
        {
            this.numVariant = numVariant;
            questions = baseConnection.QuestionVariants //получаем сптсок вопросов из базы
                .Where(tb => tb.Variant == numVariant)
                .Include(tb => tb.IdQuestionNavigation)
                .ThenInclude(tb => tb.IdCategoryNavigation)
                .ThenInclude(tb => tb.IdOrientationNavigation)
                .ToList();
            Random.Shared.Shuffle(CollectionsMarshal.AsSpan(questions));//перемешиваем вопросы
            CountQuestion = questions.Count();

            NextQuestion();
        }

        //контейнер дл€ вопросов
        public UserControl TestUC { get => testUC; set => this.RaiseAndSetIfChanged(ref testUC, value); }
        //данные о тесте
        public string NameUser { get => nameUser; set => nameUser = value; }
        public int NumVariant { get => numVariant; set => this.RaiseAndSetIfChanged(ref numVariant, value); }
        public int NumQuestion { get => numQuestion; set => this.RaiseAndSetIfChanged(ref numQuestion, value); }
        public int CountQuestion { get => countQuestion; set => countQuestion = value; }
        //view model дл€ заданий
        public TestArrangementOfElementsViewModel TestAE { get => testAE; set => testAE = value; }
        public TestChoosingAnAnswerFromASetViewModel TestCAFS { get => testCAFS; set => testCAFS = value; }
        public TestChoosing1AnswerViewModel TestC1A { get => testC1A; set => testC1A = value; }
        public TestChoosingMultipleAnswersViewModel TestCMA { get => testCMA; set => testCMA = value; }
        public TestMatchTheElementViewModel TestME { get => testME; set => testME = value; }


        /// <summary>
        /// ћетод дл€ изменени€ содержимого страницы
        /// </summary>
        public void NextQuestion()
        {            
            if (questions.Count > 0)
            {
                if (numQuestion < questions.Count) NumQuestion++;
                else return;

                switch (questions[NumQuestion - 1].IdQuestionNavigation.IdCategoryNavigation.Name)
                {
                    case "¬опрос с 1 вариантом ответа":
                        {
                            TestC1A = new TestChoosing1AnswerViewModel(questions[numQuestion - 1].IdQuestion);
                            TestUC = new TestChoosing1Answer();
                            break;
                        }
                    case "¬опрос с несколькими вариантами ответа":
                        {
                            TestCMA = new TestChoosingMultipleAnswersViewModel(questions[numQuestion - 1].IdQuestion);
                            TestUC = new TestChoosingMultipleAnswers();
                            break;
                        }
                    case "√оризонтальное упор€дочивание элементов":
                        {
                            TestAE = new TestArrangementOfElementsViewModel(questions[NumQuestion - 1].IdQuestion, Orientation.Horizontal);
                            TestUC = new TestArrangementOfElements();
                            break;
                        }
                    case "¬ертикальное упор€дочивание элементов":
                        {
                            TestAE = new TestArrangementOfElementsViewModel(questions[NumQuestion - 1].IdQuestion, Orientation.Vertical);
                            TestUC = new TestArrangementOfElements();
                            break;
                        }
                    case "—оотношение величин":
                        {
                            break;
                        }
                    case "—оотношение элементов":
                        {
                            TestME = new TestMatchTheElementViewModel(questions[NumQuestion - 1].IdQuestion);
                            TestUC = new TestMatchTheElement();
                            break;
                        }
                    case "ѕодстановка ответов":
                        {
                            TestCAFS = new TestChoosingAnAnswerFromASetViewModel(questions[NumQuestion - 1].IdQuestion);
                            TestUC = new TestChoosingAnAnswerFromASet();
                            break;
                        }
                }
            }

        }
    }
}