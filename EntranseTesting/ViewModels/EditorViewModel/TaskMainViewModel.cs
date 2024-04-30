using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using EntranseTesting.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using ReactiveUI;

namespace EntranseTesting.ViewModels
{
    public partial class TaskMainViewModel : ObservableObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        string? searchLine = "";
        bool selectedInTest = true;
        string selectedCategory = "��� ���� �������";
        string selectedSort = "��� ����������";
        [ObservableProperty] List<String> categoryList = ["��� ���� �������", "������ � 1 ��������� ������", "������ � ����������� ���������� ������", "�������������� ���������", "����������� �������", "����������� ���������", "����������� �������"];
        [ObservableProperty] List<String> sortList = ["��� ����������", "����� ����������� �������", "���� ����������� �������", "�� ���������� + �������", "�� ���������� - �������"];
        ObservableCollection<Question> questions = new ObservableCollection<Question>();

        public ObservableCollection<Question> Questions { get => questions; set { questions = value; OnPropertyChanged("Questions"); } }
        public string SelectedCategory { get => selectedCategory; set { selectedCategory = value; OnPropertyChanged("SelectedCategory"); filter(); } }
        public string SelectedSort { get => selectedSort; set { selectedSort = value; OnPropertyChanged("SelectedSort"); filter(); } }
        public string? SearchLine { get => searchLine; set { searchLine = value; OnPropertyChanged("SearchLine"); filter(); } }
        public bool SelectedInTest { get => selectedInTest; set { selectedInTest = value; OnPropertyChanged("SelectedInTest"); filter(); } }

        public TaskMainViewModel()
        {
            filter();
        }
        public async void DeleteQuestion(int idQuestion)
        {
            try
            {
                var result = await MessageBoxManager.GetMessageBoxStandard("�������� �������", "�� �� ����������� ��� ������� �������� �� ������������� ������ ������� ���� ������?", ButtonEnum.YesNo).ShowAsync();
                switch (result)
                {
                    case ButtonResult.Yes:
                        {
                            EntranceTestingContext connection = new EntranceTestingContext();
                            Question q = connection.Questions.FirstOrDefault(tb => tb.Id == idQuestion);
                            if (q != null)
                                connection.Questions.Remove(q);
                            connection.SaveChanges();
                            filter();
                            await MessageBoxManager.GetMessageBoxStandard("�������� ��������", "������ ������� ������", ButtonEnum.Ok).ShowAsync();
                            break;
                        }
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("������ ��������", "�� ����� �������� �������� ������", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
            }
        }
        private void filter()
        {
            try
            {
                Questions.Clear();
                List<Question> _list = new EntranceTestingContext().Questions
                    .Include(tb => tb.IdCategoryNavigation).ThenInclude(tb => tb.IdOrientationNavigation)
                    .Include(tb => tb.ElementOfChooses)
                    .Include(tb => tb.ElementOfArrangements)
                    .Include(tb => tb.TextOfPuttings).ThenInclude(tb => tb.ElementOfPuttings)
                    .Include(tb => tb.ElementOfEqualities).ThenInclude(tb => tb.RatioOfElementEqualityIdElement1Navigations)
                    .Include(tb => tb.ElementOfEqualities).ThenInclude(tb => tb.RatioOfElementEqualityIdElement2Navigations)
                    .Include(tb => tb.Groups).ThenInclude(tb => tb.ElementOfGroups)
                    .Include(tb => tb.UserResponses).ToList();

                //comboBox wiht typing Question (selected type)
                if (SelectedCategory != "��� ���� �������" && SelectedCategory != null)
                    _list = _list.Where(tb => tb.IdCategoryNavigation.Name.ToLower().Contains(SelectedCategory.ToLower())).ToList();
                //search 
                if (!string.IsNullOrWhiteSpace(SearchLine))
                    _list = _list.Where(tb => tb.FillName.ToLower().Contains(SearchLine.ToLower())).ToList();
                if (SelectedInTest)
                    _list = _list.Where(tb => tb.InTest == true).ToList();
                //sort
                switch (SelectedSort)
                {
                    case "����� ����������� �������":
                        {
                            _list = _list.OrderByDescending(tb => tb.CountInResponse).ToList();
                            break;
                        }
                    case "���� ����������� �������": 
                        {
                            _list = _list.OrderBy(tb => tb.CountInResponse).ToList();
                            break;
                        }
                    case "�� ���������� + �������": 
                        {
                            _list = _list.OrderByDescending(tb => tb.CountCorrectly).ToList();
                            break;
                        }
                    case "�� ���������� - �������": 
                        {
                            _list = _list.OrderByDescending(tb => tb.UnCountCorrectly).ToList();
                            break;
                        }
                    default:
                        break;
                }

                if (_list.Count > 0)
                    foreach (Question q in _list)
                        Questions.Add(q);
                else
                    Questions = new ObservableCollection<Question>();
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
            }

        }
    }
}