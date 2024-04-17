using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EntranseTesting.Models;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace EntranseTesting.ViewModels
{
	public partial class TestMainViewModel : ObservableObject
    {
        [ObservableProperty] TestPageViewModel testPages = new TestPageViewModel();
        [ObservableProperty] string? groupUser = "";
        [ObservableProperty] string? nameUser = "";

        // Метод для изменения содержимого страницы теста        
        public void NextQuestion()
        {
            changeResponse();
            TestPages.ProgressButtons[TestPages.NumQuestion - 1].Active = false;
            if (TestPages.NumQuestion < TestPages.CountQuestion) TestPages.NumQuestion++;
            else return;
            TestPages.ProgressButtons[TestPages.NumQuestion - 1].Active = true;

            if (TestPages.NumQuestion == TestPages.CountQuestion)
                TestPages.ButtonValue = "Завершить тест";

            TestPages.changingPage();
        }

        public void ClickToQuestion(int numQuestion)
        {
            changeResponse();
            TestPages.ProgressButtons[TestPages.NumQuestion - 1].Active = false;
            TestPages.NumQuestion = numQuestion;
            TestPages.ProgressButtons[TestPages.NumQuestion-1].Active = true;

            if (TestPages.NumQuestion == TestPages.CountQuestion)
                TestPages.ButtonValue = "Завершить тест";
            else
                TestPages.ButtonValue = "Следующий вопрос";

            TestPages.changingPage();
        }

        private void changeResponse()
        {
            if (TestPages.QuestionsCollection.Count > 0)
            {
                switch (TestPages.QuestionsCollection[TestPages.NumQuestion - 1].IdCategoryNavigation.Name)
                {
                    case "Вопрос с 1 вариантом ответа":
                        {
                            Response.SaveChangesChoosing(TestPages.QuestionsCollection[TestPages.NumQuestion - 1].Id, TestPages.TestC1A.Element);
                            break;
                        }
                    case "Вопрос с несколькими вариантами ответа":
                        {
                            Response.SaveChangesChoosing(TestPages.QuestionsCollection[TestPages.NumQuestion - 1].Id, TestPages.TestCMA.Element);
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
                            
                            break;
                        }
                    case "Соотношение элементов":
                        {                           
                            break;
                        }
                    case "Подстановка ответов":
                        {
                           // Response.SaveChangesCAFS(TestPages.QuestionsCollection[TestPages.NumQuestion - 1].Id, TestPages.TestCAFS.Element);
                            break;
                        }
                }
            }
         
        }
    }
}