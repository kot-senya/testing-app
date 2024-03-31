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
        string nameUser = "�� �����������";
        int numVariant = 0;
        int numQuestion = 0;
        int countQuestion = 0;
        List<QuestionVariant> questions;

        //view model ��� ������ � ������
        TestArrangementOfElementsViewModel testAE;//������������ ��������
        TestChoosingAnAnswerFromASetViewModel testCAFS;//����������� ������� � �����
        TestChoosing1AnswerViewModel testC1A;//����� 1 ��������
        TestChoosingMultipleAnswersViewModel testCMA;//����� ���������� ��������� ������
        TestMatchTheElementViewModel testME;//����������� ���������
        public TestPageViewModel(int numVariant)
        {
            this.numVariant = numVariant;
            questions = baseConnection.QuestionVariants //�������� ������ �������� �� ����
                .Where(tb => tb.Variant == numVariant)
                .Include(tb => tb.IdQuestionNavigation)
                .ThenInclude(tb => tb.IdCategoryNavigation)
                .ThenInclude(tb => tb.IdOrientationNavigation)
                .ToList();
            Random.Shared.Shuffle(CollectionsMarshal.AsSpan(questions));//������������ �������
            CountQuestion = questions.Count();

            NextQuestion();
        }

        //��������� ��� ��������
        public UserControl TestUC { get => testUC; set => this.RaiseAndSetIfChanged(ref testUC, value); }
        //������ � �����
        public string NameUser { get => nameUser; set => nameUser = value; }
        public int NumVariant { get => numVariant; set => this.RaiseAndSetIfChanged(ref numVariant, value); }
        public int NumQuestion { get => numQuestion; set => this.RaiseAndSetIfChanged(ref numQuestion, value); }
        public int CountQuestion { get => countQuestion; set => countQuestion = value; }
        //view model ��� �������
        public TestArrangementOfElementsViewModel TestAE { get => testAE; set => testAE = value; }
        public TestChoosingAnAnswerFromASetViewModel TestCAFS { get => testCAFS; set => testCAFS = value; }
        public TestChoosing1AnswerViewModel TestC1A { get => testC1A; set => testC1A = value; }
        public TestChoosingMultipleAnswersViewModel TestCMA { get => testCMA; set => testCMA = value; }
        public TestMatchTheElementViewModel TestME { get => testME; set => testME = value; }


        /// <summary>
        /// ����� ��� ��������� ����������� ��������
        /// </summary>
        public void NextQuestion()
        {            
            if (questions.Count > 0)
            {
                if (numQuestion < questions.Count) NumQuestion++;
                else return;

                switch (questions[NumQuestion - 1].IdQuestionNavigation.IdCategoryNavigation.Name)
                {
                    case "������ � 1 ��������� ������":
                        {
                            TestC1A = new TestChoosing1AnswerViewModel(questions[numQuestion - 1].IdQuestion);
                            TestUC = new TestChoosing1Answer();
                            break;
                        }
                    case "������ � ����������� ���������� ������":
                        {
                            TestCMA = new TestChoosingMultipleAnswersViewModel(questions[numQuestion - 1].IdQuestion);
                            TestUC = new TestChoosingMultipleAnswers();
                            break;
                        }
                    case "�������������� �������������� ���������":
                        {
                            TestAE = new TestArrangementOfElementsViewModel(questions[NumQuestion - 1].IdQuestion, Orientation.Horizontal);
                            TestUC = new TestArrangementOfElements();
                            break;
                        }
                    case "������������ �������������� ���������":
                        {
                            TestAE = new TestArrangementOfElementsViewModel(questions[NumQuestion - 1].IdQuestion, Orientation.Vertical);
                            TestUC = new TestArrangementOfElements();
                            break;
                        }
                    case "����������� �������":
                        {
                            break;
                        }
                    case "����������� ���������":
                        {
                            TestME = new TestMatchTheElementViewModel(questions[NumQuestion - 1].IdQuestion);
                            TestUC = new TestMatchTheElement();
                            break;
                        }
                    case "����������� �������":
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