using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EntranseTesting.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace EntranseTesting.ViewModels
{
    public partial class TestMainViewModel : ObservableObject
    {
        [ObservableProperty] TestPageViewModel testPages = new TestPageViewModel();
        [ObservableProperty] TestResultViewModel testResults = new TestResultViewModel();
        [ObservableProperty] string? groupUser = "";
        [ObservableProperty] string? nameUser = "";

        public void ClickToQuestion(int numQuestion)
        {
            changeResponse();
            TestPages.ProgressButtons[TestPages.NumQuestion - 1].Active = false;
            TestPages.NumQuestion = numQuestion;
            TestPages.ProgressButtons[TestPages.NumQuestion - 1].Active = true;

            if (TestPages.NumQuestion == TestPages.CountQuestion)
                TestPages.ButtonValue = "Завершить тест";
            else
                TestPages.ButtonValue = "Следующий вопрос";

            while (!TestPages.TakeProgressButtons.Contains(TestPages.ProgressButtons[TestPages.NumQuestion - 1]))
            {
                if (TestPages.NumQuestion > TestPages.TakeProgressButtons.Last().Num)
                    TestPages.SkipItem++;
                else
                    TestPages.SkipItem--;
            }
            TestPages.changingPage();
        }

        public void changeResponse()
        {
            if (TestPages.QuestionsCollection.Count > 0)
            {
                switch (TestPages.QuestionsCollection[TestPages.NumQuestion - 1].IdCategoryNavigation.Name)
                {
                    case "Вопрос с 1 вариантом ответа":
                        {
                            Response.SaveChangesChoosing(TestPages.QuestionsCollection[TestPages.NumQuestion - 1].Id, false, TestPages.TestC1A.Element);
                            break;
                        }
                    case "Вопрос с несколькими вариантами ответа":
                        {
                            Response.SaveChangesChoosing(TestPages.QuestionsCollection[TestPages.NumQuestion - 1].Id, true, TestPages.TestCMA.Element);
                            break;
                        }
                    case "Горизонтальное упорядочивание элементов":
                    case "Вертикальное упорядочивание элементов":
                        {
                            Response.SaveChangesArrangement(TestPages.QuestionsCollection[TestPages.NumQuestion - 1].Id, TestPages.TestAE.Element);
                            break;
                        }
                    case "Соотношение величин":
                        {
                            Response.SaveChangesMatchTheValue(TestPages.QuestionsCollection[TestPages.NumQuestion - 1].Id, TestPages.TestMV.Elements);
                            break;
                        }
                    case "Соотношение элементов":
                        {
                            Response.SaveChangesMatchTheElement(TestPages.QuestionsCollection[TestPages.NumQuestion - 1].Id, TestPages.TestME.Matches);
                            break;
                        }
                    case "Подстановка ответов":
                        {
                            Response.SaveChangesCAFS(TestPages.QuestionsCollection[TestPages.NumQuestion - 1].Id, TestPages.TestCAFS.Element);
                            break;
                        }
                }
               TestPages.ProgressButtons[TestPages.NumQuestion - 1].Check = Response.change;
            }

        }
    }
}