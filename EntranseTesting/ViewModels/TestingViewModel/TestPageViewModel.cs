using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EntranseTesting.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace EntranseTesting.ViewModels
{
    public partial class TestPageViewModel : ObservableObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        [ObservableProperty] AppSetting settingTest = new AppSetting();
        int hintCount = 0;
        //��������� ��� ��������
        UserControl testUC;
        //������ � �����
        string buttonValue = "��������� ������";
        [ObservableProperty] int numQuestion = 0;
        [ObservableProperty] int countQuestion = 0;
        bool visibleHint = false;
        List<QuestionHint> hints = new List<QuestionHint>();
        [ObservableProperty] List<Question> questionsCollection;
        List<ItemProgressButton> progressButtons = new List<ItemProgressButton>();
        int takeItem = 5;
        int skipItem = 0;
        List<ItemProgressButton> takeProgressButtons = new List<ItemProgressButton>();

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
            SettingTest = baseConnection.AppSettings.ToList().LastOrDefault();
            if (SettingTest == null)
            {
                SettingTest = new AppSetting()
                {
                    Time = new TimeSpan(0, 45, 0),
                    CountOfQuestions = 30,
                    HintVisibility = false,
                    CountOfHints = 0,
                    Raiting5 = 30,
                    Raiting4 = 23,
                    Raiting3 = 16
                };
            }
            CountQuestion = SettingTest.CountOfQuestions;

            //�������� ���� ������ �������� �� ����
            QuestionsCollection = baseConnection.Questions.Include(tb => tb.IdCategoryNavigation).Where(tb => tb.InTest == true).ToList();
            Random.Shared.Shuffle(CollectionsMarshal.AsSpan(QuestionsCollection));//������������ �������            
            QuestionsCollection = QuestionsCollection.Take(CountQuestion).ToList();//����� ������ ���������� ��������
            //��������� ��������� � ��������
            Response.loadHints(QuestionsCollection);

            //������ ���������
            CountQuestion = (QuestionsCollection.Count == SettingTest.CountOfQuestions) ? SettingTest.CountOfQuestions : (QuestionsCollection.Count > SettingTest.CountOfQuestions) ? SettingTest.CountOfQuestions : QuestionsCollection.Count;

            ProgressButtons.Clear();
            for (int i = 0; i < CountQuestion; i++)
                ProgressButtons.Add(new ItemProgressButton(i + 1));
            ProgressButtons[0].Active = true;

            //��������� ������ � �������� ������������
            Response.responseUsers = new List<UserResponse>();
            for (int i = 0; i < CountQuestion; i++)
                Response.responseUsers.Add(new UserResponse() { IdQuestion = QuestionsCollection[i].Id });

            //�������� ����
            NumQuestion = 1;
            TakeItem = InterfaceSettings.take;

            changingPage();
        }

        public List<ItemProgressButton> TakeProgressButtons
        {
            get => ProgressButtons.Skip(SkipItem).Take(TakeItem).ToList();
        }
        public int TakeItem { get => takeItem; set { takeItem = value; OnPropertyChanged("TakeItem"); OnPropertyChanged("TakeProgressButtons"); } }
        public int SkipItem { get => skipItem; set { skipItem = value; OnPropertyChanged("SkipItem"); OnPropertyChanged("TakeProgressButtons"); } }
        public UserControl TestUC { get => testUC; set { testUC = value; OnPropertyChanged("TestUC"); OnPropertyChanged("ProgressText"); OnPropertyChanged("NumQuestion"); } }
        public string ProgressText { get => "{0} �� {3}"; }
        public int HintCount { get => hintCount; set { hintCount = value; OnPropertyChanged("HintCount"); OnPropertyChanged("HintCountLine"); } }
        public string HintCountLine { get => "���������� ���������� ����� " + (SettingTest.CountOfHints - HintCount); }
        public List<QuestionHint> Hints { get => hints; set { hints = value; OnPropertyChanged("Hints"); } }
        public bool VisibleHint { get => visibleHint; set { visibleHint = value; OnPropertyChanged("VisibleHint"); OnPropertyChanged("NoVisibleHint"); } }
        public bool NoVisibleHint { get => !visibleHint; }
        public string ButtonValue { get => buttonValue; set { buttonValue = value; OnPropertyChanged("ButtonValue"); } }

        public List<ItemProgressButton> ProgressButtons { get => progressButtons; set {progressButtons = value; OnPropertyChanged("ProgressButtons"); } }

        public void Next()
        {
            if (TakeProgressButtons.Last() != ProgressButtons.Last())
                SkipItem++;
        }
        public void Back()
        {
            if (TakeProgressButtons.First() != ProgressButtons.First())
                SkipItem--;
        }
        public void changingPage()
        {
            if (QuestionsCollection.Count > 0)
            {
                List<QuestionHint> _listHints = Response.questionHints.Where(tb => tb.IdQuestion == QuestionsCollection[NumQuestion - 1].Id).ToList();
                if (_listHints.Count > 0)
                {
                    VisibleHint = true;
                    Hints = _listHints.OrderBy(tb => tb.Cost).ToList();
                }
                else
                {
                    VisibleHint = false;
                    Hints = new List<QuestionHint>();
                }
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

        public async void BuyAHint(int idQHint)
        {
            try
            {
                QuestionHint item = Hints.FirstOrDefault(tb => tb.Id == idQHint);
                if (SettingTest.CountOfHints - HintCount < item.Cost)
                {
                    await MessageBoxManager.GetMessageBoxStandard("", "��� �� ������� ����� ��� �������� ���������", ButtonEnum.Ok).ShowAsync();
                    return;
                }

                var result = await MessageBoxManager.GetMessageBoxStandard("", "�� ������������� ������ ������ ���������?\n��������� ��������� " + item.CostLine + ".\n����� ������� � ��� ��������� " + Response.CostLine(SettingTest.CountOfHints - HintCount - item.Cost), ButtonEnum.YesNo).ShowAsync();
                switch (result)
                {
                    case ButtonResult.Yes:
                        {
                            HintCount += item.Cost;
                            int index = Hints.IndexOf(item);
                            Hints[index].HintPurchased = true;
                            index = Response.questionHints.IndexOf(item);
                            Response.questionHints[index].HintPurchased = true;
                            int responseIndex = Response.IndexResponse(QuestionsCollection[NumQuestion - 1].Id);
                            Response.responseUsers[responseIndex].HintApply = true;
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("", "���-�� ����� �� ���", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
            }
        }
    }
}