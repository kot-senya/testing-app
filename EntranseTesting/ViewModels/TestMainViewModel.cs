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

        // ����� ��� ��������� ����������� �������� �����        
        public async void NextQuestion()
        {
            changeResponse();

            if (TestPages.NumQuestion < TestPages.CountQuestion)
            {
                TestPages.ProgressButtons[TestPages.NumQuestion - 1].Active = false;
                TestPages.NumQuestion++;
            }
            else if(TestPages.NumQuestion == TestPages.CountQuestion)//���� ����� �����
            {
                if (TestPages.QuestionsCollection.Count() != TestPages.ProgressButtons.Where(tb => tb.Check == true).Count())
                {
                    var result = await MessageBoxManager.GetMessageBoxStandard("", "� ��� �������� ������������� �������� �� ����� ������ ���������?", ButtonEnum.YesNo).ShowAsync();
                    switch (result)
                    {
                        case ButtonResult.Yes:
                                break;
                        case ButtonResult.No:
                                return;
                    }
                }
                changeResponse();//��������� ����� ���������� �������
                SaveResults();               
            }

            TestPages.ProgressButtons[TestPages.NumQuestion - 1].Active = true;

            if (TestPages.NumQuestion == TestPages.CountQuestion)
                TestPages.ButtonValue = "��������� ����";

            while (!TestPages.TakeProgressButtons.Contains(TestPages.ProgressButtons[TestPages.NumQuestion - 1]))
            {
                if (TestPages.NumQuestion > TestPages.TakeProgressButtons.Last().Num)
                    TestPages.SkipItem++;
                else
                    TestPages.SkipItem--;
            }

            TestPages.changingPage();
        }

        public async void SaveResults()
        {
            try
            {
                EntranceTestingContext connection = new EntranceTestingContext();
                connection.UserSessions.Add(Response.userSession);
                connection.SaveChanges();
                for(int i = 0; i < Response.responseUsers.Count; i ++)
                {
                    Response.responseUsers[i].IdSession = Response.userSession.Id;
                    connection.UserResponses.Add(Response.responseUsers[i]);
                }
                connection.SaveChanges();
                //�������� ������
                TestResults = new TestResultViewModel(null);
                //UC = new TestResult();
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("", "�� �����-�� ������� �� ������� ��������� ������", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
            }
        }
        public void ClickToQuestion(int numQuestion)
        {
            changeResponse();
            TestPages.ProgressButtons[TestPages.NumQuestion - 1].Active = false;
            TestPages.NumQuestion = numQuestion;
            TestPages.ProgressButtons[TestPages.NumQuestion - 1].Active = true;

            if (TestPages.NumQuestion == TestPages.CountQuestion)
                TestPages.ButtonValue = "��������� ����";
            else
                TestPages.ButtonValue = "��������� ������";

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
                Response.change = false;
                switch (TestPages.QuestionsCollection[TestPages.NumQuestion - 1].IdCategoryNavigation.Name)
                {
                    case "������ � 1 ��������� ������":
                        {
                            Response.SaveChangesChoosing(TestPages.QuestionsCollection[TestPages.NumQuestion - 1].Id, false, TestPages.TestC1A.Element);
                            break;
                        }
                    case "������ � ����������� ���������� ������":
                        {
                            Response.SaveChangesChoosing(TestPages.QuestionsCollection[TestPages.NumQuestion - 1].Id, true, TestPages.TestCMA.Element);
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
                            Response.SaveChangesMatchTheValue(TestPages.QuestionsCollection[TestPages.NumQuestion - 1].Id, TestPages.TestMV.Elements);
                            break;
                        }
                    case "����������� ���������":
                        {
                            Response.SaveChangesMatchTheElement(TestPages.QuestionsCollection[TestPages.NumQuestion - 1].Id, TestPages.TestME.Matches);
                            break;
                        }
                    case "����������� �������":
                        {
                            // Response.SaveChangesCAFS(TestPages.QuestionsCollection[TestPages.NumQuestion - 1].Id, TestPages.TestCAFS.Element);
                            break;
                        }
                }
                if (Response.change)
                    TestPages.ProgressButtons[TestPages.NumQuestion - 1].Check = true;
            }

        }
    }
}