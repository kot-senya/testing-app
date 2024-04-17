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

        // ����� ��� ��������� ����������� �������� �����        
        public void NextQuestion()
        {
            changeResponse();
            TestPages.ProgressButtons[TestPages.NumQuestion - 1].Active = false;
            if (TestPages.NumQuestion < TestPages.CountQuestion) TestPages.NumQuestion++;
            else return;
            TestPages.ProgressButtons[TestPages.NumQuestion - 1].Active = true;

            if (TestPages.NumQuestion == TestPages.CountQuestion)
                TestPages.ButtonValue = "��������� ����";

            TestPages.changingPage();
        }

        public void ClickToQuestion(int numQuestion)
        {
            changeResponse();
            TestPages.ProgressButtons[TestPages.NumQuestion - 1].Active = false;
            TestPages.NumQuestion = numQuestion;
            TestPages.ProgressButtons[TestPages.NumQuestion-1].Active = true;

            if (TestPages.NumQuestion == TestPages.CountQuestion)
                TestPages.ButtonValue = "��������� ����";
            else
                TestPages.ButtonValue = "��������� ������";

            TestPages.changingPage();
        }

        private void changeResponse()
        {
            if (TestPages.QuestionsCollection.Count > 0)
            {
                switch (TestPages.QuestionsCollection[TestPages.NumQuestion - 1].IdCategoryNavigation.Name)
                {
                    case "������ � 1 ��������� ������":
                        {
                            Response.SaveChangesChoosing(TestPages.QuestionsCollection[TestPages.NumQuestion - 1].Id, TestPages.TestC1A.Element);
                            break;
                        }
                    case "������ � ����������� ���������� ������":
                        {
                            Response.SaveChangesChoosing(TestPages.QuestionsCollection[TestPages.NumQuestion - 1].Id, TestPages.TestCMA.Element);
                            break;
                        }
                    case "�������������� �������������� ���������":
                    case "������������ �������������� ���������":
                        {
                           Response.SaveChangesArrangement(TestPages.QuestionsCollection[TestPages.NumQuestion - 1].Id, TestPages.TestAE.Element);
                           break;
                        }
                    case "����������� �������":
                        {
                            
                            break;
                        }
                    case "����������� ���������":
                        {                           
                            break;
                        }
                    case "����������� �������":
                        {
                           // Response.SaveChangesCAFS(TestPages.QuestionsCollection[TestPages.NumQuestion - 1].Id, TestPages.TestCAFS.Element);
                            break;
                        }
                }
            }
         
        }
    }
}