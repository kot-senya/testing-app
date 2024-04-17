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
        //��������� ��� ��������
        [ObservableProperty] UserControl testUC;
        //������ � �����
        [ObservableProperty] string buttonValue = "��������� ������";
        [ObservableProperty] int numQuestion = 0;
        [ObservableProperty] int countQuestion = 0;
        [ObservableProperty] List<Question> questionsCollection;
        [ObservableProperty] List<ItemProgressButton> progressButtons = new List<ItemProgressButton>();

        //view model ��� ������ � ������
        [ObservableProperty] TestArrangementOfElementsViewModel testAE;//������������ ��������
        [ObservableProperty] TestChoosingAnAnswerFromASetViewModel testCAFS;//����������� ������� � �����
        [ObservableProperty] TestChoosing1AnswerViewModel testC1A;//����� 1 ��������
        [ObservableProperty] TestChoosingMultipleAnswersViewModel testCMA;//����� ���������� ��������� ������
        [ObservableProperty] TestMatchTheElementViewModel testME;//����������� ���������
        [ObservableProperty] TestMatchTheValueViewModel testMV;//����������� �������
        public TestPageViewModel()
        {
            EntranceTestingContext baseConnection = new EntranceTestingContext();
            SettingTest = baseConnection.AppSettings.ToList().Last();
            CountQuestion = SettingTest.CountOfQuestions;

            //�������� ���� ������ �������� �� ����
            QuestionsCollection = baseConnection.Questions.Include(tb => tb.IdCategoryNavigation).Where(tb => tb.InTest == true).ToList();          
            Random.Shared.Shuffle(CollectionsMarshal.AsSpan(QuestionsCollection));//������������ �������            
            QuestionsCollection = QuestionsCollection.Take(CountQuestion).ToList();//����� ������ ���������� ��������
            
            //������ ���������
            CountQuestion = (QuestionsCollection.Count == SettingTest.CountOfQuestions)? SettingTest.CountOfQuestions: (QuestionsCollection.Count > SettingTest.CountOfQuestions) ? SettingTest.CountOfQuestions: QuestionsCollection.Count;
            for (int i = 0; i < CountQuestion; i++)
                ProgressButtons.Add(new ItemProgressButton(i + 1));
            ProgressButtons[0].Active = true;
            
            //��������� ������ � �������� ������������
            Response.responseUsers = new List<UserResponse>();
            for (int i = 0; i < CountQuestion; i++)
                Response.responseUsers.Add(new UserResponse() { IdQuestion = QuestionsCollection[i].Id });
            //�������� ����
            NumQuestion = 1;
            changingPage();
        }        

        public void changingPage()
        {
            if (QuestionsCollection.Count > 0)
            {
                switch (QuestionsCollection[NumQuestion - 1].IdCategoryNavigation.Name)
                {
                    case "������ � 1 ��������� ������":
                        {
                            TestC1A = new TestChoosing1AnswerViewModel(QuestionsCollection[NumQuestion - 1].Id);
                            TestUC = new TestChoosing1Answer();
                            break;
                        }
                    case "������ � ����������� ���������� ������":
                        {
                            TestCMA = new TestChoosingMultipleAnswersViewModel(QuestionsCollection[NumQuestion - 1].Id);
                            TestUC = new TestChoosingMultipleAnswers();
                            break;
                        }
                    case "�������������� �������������� ���������":
                        {
                            TestAE = new TestArrangementOfElementsViewModel(QuestionsCollection[NumQuestion - 1].Id, Orientation.Horizontal);
                            TestUC = new TestArrangementOfElements();
                            break;
                        }
                    case "������������ �������������� ���������":
                        {
                            TestAE = new TestArrangementOfElementsViewModel(QuestionsCollection[NumQuestion - 1].Id, Orientation.Vertical);
                            TestUC = new TestArrangementOfElements();
                            break;
                        }
                    case "����������� �������":
                        {
                            TestMV = new TestMatchTheValueViewModel(QuestionsCollection[NumQuestion - 1].Id);
                            TestUC = new TestMatchTheValue();
                            break;
                        }
                    case "����������� ���������":
                        {
                            TestME = new TestMatchTheElementViewModel(QuestionsCollection[NumQuestion - 1].Id);
                            TestUC = new TestMatchTheElement();
                            break;
                        }
                    case "����������� �������":
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