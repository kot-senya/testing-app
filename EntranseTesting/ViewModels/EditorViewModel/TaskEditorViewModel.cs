using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using EntranseTesting.Models;
using EntranseTesting.Models.customClass;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace EntranseTesting.ViewModels
{
    public partial class TaskEditorViewModel : ObservableObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        //�������� ��������
        [ObservableProperty] UserControl? taskUC = null;
        [ObservableProperty] bool editing = false;
        [ObservableProperty] bool editingCategory = true;
        [ObservableProperty] string header = "";
        [ObservableProperty] List<string> category = ["--", "����� ������", "�������������� ���������", "�����������", "����������� �������"];
        [ObservableProperty] List<string> _choseCategory = ["1 ���������� ��������", "��������� ���������� ��������"];
        [ObservableProperty] List<string> _arrangmentCategory = ["�������������", "�����������"];
        [ObservableProperty] List<string> _matchCategory = ["������ ���������", "������ ��������"];
        string selectedCategory = "--";
        string selectedMatchCategory = "������ ���������";
        [ObservableProperty] bool selectedGroup = true;
        [ObservableProperty] string selectedChoseCategory = "1 ���������� ��������";
        [ObservableProperty] string selectedArrangmentCategory = "�������������";

        //�������� �������
        [ObservableProperty] Question q;
        List<QuestionImage> qImage;
        List<QuestionHint> qHint;
        ObservableCollection<ElementOfChoose> chooseElement = new ObservableCollection<ElementOfChoose>();
        [ObservableProperty] Group matchesGroup1 = new Group();
        [ObservableProperty] Group matchesGroup2 = new Group();
        ObservableCollection<ItemMatch> matchesElement = new ObservableCollection<ItemMatch>();
        ObservableCollection<ItemCAFS> matchesMultiplyElement = new ObservableCollection<ItemCAFS>();
        ObservableCollection<ElementOfArrangement> arrangementElement = new ObservableCollection<ElementOfArrangement>();

        public TaskEditorViewModel()
        {
            Header = "���������� �������";
            Q = new Question() { IdCategory = 1 };
            QImage = new List<QuestionImage>();
            QHint = new List<QuestionHint>();
            EntranceTestingContext connection = new EntranceTestingContext();
            connection.Questions.Add(Q);
            connection.SaveChanges();
        }
        public TaskEditorViewModel(int idQuestion)
        {
            EntranceTestingContext baseConnection = new EntranceTestingContext();

            Header = "�������������� �������";
            Editing = true;
            OnPropertyChanged("NoEditing");
            Q = baseConnection.Questions.Include(tb => tb.IdCategoryNavigation)
                .Include(tb => tb.ElementOfChooses)
                .Include(tb => tb.ElementOfEqualities).ThenInclude(tb => tb.RatioOfElementEqualityIdElement1Navigations)
                .Include(tb => tb.ElementOfEqualities).ThenInclude(tb => tb.RatioOfElementEqualityIdElement2Navigations)
                .Include(tb => tb.ElementOfArrangements)
                .Include(tb => tb.Groups).ThenInclude(tb => tb.ElementOfGroups)
                .Include(tb => tb.TextOfPuttings).ThenInclude(tb => tb.ElementOfPuttings)
                .FirstOrDefault(tb => tb.Id == idQuestion);
            //�����������
            QImage = baseConnection.QuestionImages.Where(tb => tb.IdQuestion == Q.Id).ToList();
            if (QImage.Count == 0) QImage = new List<QuestionImage>();
            //���������
            QHint = baseConnection.QuestionHints
                .Include(tb => tb.IdHintNavigation).ThenInclude(tb => tb.HintImages)
                .Where(tb => tb.IdQuestion == Q.Id).ToList();
            if (QHint.Count == 0) QHint = new List<QuestionHint>();

            loadSettingCategory();
        }
        public string SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                loadUC();
                OnPropertyChanged("TaskUC");
            }
        }
        public string SelectedMatchCategory
        {
            get => selectedMatchCategory;
            set
            {
                selectedMatchCategory = value;
                if (value == "������ ���������")
                    SelectedGroup = true;
                else
                    SelectedGroup = false;
                OnPropertyChanged("SelectedGroup");
            }
        }
        public List<QuestionImage> QImage { get => qImage; set { qImage = value; OnPropertyChanged("QImage"); } }
        public List<QuestionHint> QHint { get => qHint; set { qHint = value; OnPropertyChanged("QHint"); } }
        public bool NoEditing { get => !Editing; }
        public ObservableCollection<ElementOfChoose> ChooseElement { get => chooseElement; set { chooseElement = value; OnPropertyChanged("ChooseElement"); } }
        public ObservableCollection<ElementOfArrangement> ArrangementElement { get => arrangementElement; set { arrangementElement = value; OnPropertyChanged("ArrangementElement"); } }
        public ObservableCollection<ItemMatch> MatchesElement { get => matchesElement; set { matchesElement = value; OnPropertyChanged("MatchesElement"); } }
        public ObservableCollection<ItemCAFS> MatchesMultiplyElement { get => matchesMultiplyElement; set { matchesMultiplyElement = value; OnPropertyChanged("MatchesMultiplyElement"); } }

        private async void loadSettingCategory()
        {
            try
            {
                switch (Q.IdCategoryNavigation.Name)
                {
                    case "������ � 1 ��������� ������":
                        {
                            ChooseElement.Clear();
                            foreach (var item in Q.ElementOfChooses)
                                ChooseElement.Add(item);
                            SelectedChoseCategory = "1 ���������� ��������";
                            SelectedCategory = "����� ������";
                            break;
                        }
                    case "������ � ����������� ���������� ������":
                        {
                            ChooseElement.Clear();
                            foreach (var item in Q.ElementOfChooses)
                                ChooseElement.Add(item);
                            SelectedChoseCategory = "��������� ���������� ��������";
                            SelectedCategory = "����� ������";
                            break;
                        }
                    case "�������������� �������������� ���������":
                        {
                            ArrangementElement.Clear();
                            foreach (var item in Q.ElementOfArrangements.OrderBy(tb => tb.Position))
                                ArrangementElement.Add(item);
                            SelectedArrangmentCategory = "�������������";
                            SelectedCategory = "�������������� ���������";
                            break;
                        }
                    case "������������ �������������� ���������":
                        {
                            ArrangementElement.Clear();
                            foreach (var item in Q.ElementOfArrangements.OrderBy(tb => tb.Position))
                                ArrangementElement.Add(item);
                            SelectedArrangmentCategory = "�����������";
                            SelectedCategory = "�������������� ���������";
                            break;
                        }
                    case "����������� ���������":
                        {
                            MatchesElement.Clear();
                            MatchesGroup1 = Q.Groups.ToList()[0];
                            MatchesGroup2 = Q.Groups.ToList()[1];
                            List<ElementOfGroup> e1 = MatchesGroup1.ElementOfGroups.OrderBy(tb => tb.RatioNumeri).ToList();
                            List<ElementOfGroup> e2 = MatchesGroup2.ElementOfGroups.OrderBy(tb => tb.RatioNumeri).ToList();

                            for (int i = 0; i < e1.Count; i++)
                                MatchesElement.Add(new ItemMatch(e1[i], e2[i]));
                            SelectedMatchCategory = "������ ���������";
                            SelectedCategory = "�����������";
                            EditingCategory = false;
                            break;
                        }
                    case "����������� �������":
                        {
                            MatchesElement.Clear();
                            List<ElementOfEquality> e = Q.ElementOfEqualities.ToList();
                            for (int i = 0; i < e.Count; i++)
                            {
                                RatioOfElementEquality elem = e[i].RatioOfElementEqualityIdElement1Navigations.FirstOrDefault();
                                if (elem != null)
                                {
                                    ItemMatch item = new ItemMatch(elem.IdElement1Navigation, elem.IdElement2Navigation);
                                    MatchesElement.Add(item);
                                }
                            }
                            SelectedMatchCategory = "������ ��������";
                            SelectedCategory = "�����������";
                            EditingCategory = false;
                            break;
                        }
                    case "����������� �������":
                        {
                            MatchesMultiplyElement.Clear();
                            List<TextOfPutting> tp = Q.TextOfPuttings.ToList();
                            for (int i = 0; i < tp.Count; i++)
                            {
                                List<ElementOfPutting> ep = tp[i].ElementOfPuttings.ToList();
                                MatchesMultiplyElement.Add(new ItemCAFS(tp[i], ep));
                            }
                            SelectedCategory = "����������� �������";
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("������", "�� ����� �������� ������ �������� ������", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.Write(ex.Message);
#endif
            }

        }
        private void loadUC()
        {
            switch (SelectedCategory)
            {
                case "����� ������":
                    {
                        TaskUC = new EditorChoosing();
                        break;
                    }
                case "�������������� ���������":
                    {
                        TaskUC = new EditorArrangement();
                        break;
                    }
                case "�����������":
                    {
                        TaskUC = new EditorMatches();
                        break;
                    }
                case "����������� �������":
                    {
                        TaskUC = new EditorMatchesMultiply();
                        break;
                    }
                default:
                    {
                        TaskUC = null;
                        break;
                    }
            }
        }
        public async void AddItem()
        {
            switch (SelectedCategory)
            {
                case "����� ������":
                    {
                        ChooseElement.Add(new ElementOfChoose());
                        break;
                    }
                case "�������������� ���������":
                    {
                        ArrangementElement.Add(new ElementOfArrangement());
                        break;
                    }
                case "�����������":
                    {
                        MatchesElement.Add(new ItemMatch());
                        break;
                    }
                case "����������� �������":
                    {
                        MatchesMultiplyElement.Add(new ItemCAFS());
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        public async void AddItemCAFS(ItemCAFS elem)
        {
            int index = MatchesMultiplyElement.IndexOf(elem);
            MatchesMultiplyElement[index].ElementEditor.Add(new ElementOfPutting());
        }
        public async void DeleteItemCAFS(object? collection)
        {
            try
            {
                var values = (IList<object?>)collection;
                ItemCAFS item = (ItemCAFS)values[0];
                ElementOfPutting elem = (ElementOfPutting)values[1];
                int index = MatchesMultiplyElement.IndexOf(item);
                MatchesMultiplyElement[index].ElementEditor.Remove(elem);
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("������ ��������", "�� ����� �������� �������� ������", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.Write(ex.Message);
#endif
            }
        }
        public async void DeleteItem(object? elem)
        {
            try
            {
                switch (SelectedCategory)
                {
                    case "����� ������":
                        {
                            ChooseElement.Remove((ElementOfChoose)elem);
                            break;
                        }
                    case "�������������� ���������":
                        {
                            ArrangementElement.Remove((ElementOfArrangement)elem);
                            break;
                        }
                    case "�����������":
                        {
                            MatchesElement.Remove((ItemMatch)elem);
                            break;
                        }
                    case "����������� �������":
                        {
                            MatchesMultiplyElement.Remove((ItemCAFS)elem);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("������ ��������", "�� ����� �������� �������� ������", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.Write(ex.Message);
#endif
            }

        }
        public async void AddHint()
        {
            EntranceTestingContext connection = new EntranceTestingContext();

            bool countFlag = QHint.Count == 0;
            List<QuestionHint> list = connection.QuestionHints.Include(tb => tb.IdHintNavigation).ThenInclude(tb => tb.HintImages).Where(tb => tb.IdQuestion == Q.Id).ToList();
            QuestionHint? qh = (list.Count > 0) ? list.Last() : null;
            //���� ��������� ��� � ��������� �������� ����� ��� �����������
            if (qh == null || countFlag || !string.IsNullOrWhiteSpace(qh.IdHintNavigation.Text) || qh.IdHintNavigation.HintImages.Count > 0)
            {
                HintText hint = new HintText();
                connection.HintTexts.Add(hint);
                connection.SaveChanges();
                QuestionHint qhint = new QuestionHint() { IdQuestion = Q.Id, IdHint = hint.Id };
                connection.QuestionHints.Add(qhint);
                connection.SaveChanges();
                QHint = connection.QuestionHints
                    .Include(tb => tb.IdHintNavigation).ThenInclude(tb => tb.HintImages)
                    .Where(tb => tb.IdQuestion == Q.Id).ToList();
            }
            else
                await MessageBoxManager.GetMessageBoxStandard("������ ����������", "� ��� ��� ���� ������ ���������, ��������� ������� ��", ButtonEnum.Ok).ShowAsync();
        }
        public async void DeleteHint(int idHint)
        {
            try
            {
                var result = await MessageBoxManager.GetMessageBoxStandard("", "�� ������������� ������ ������� ���������? ������������ �� � ���������� ����� ������", ButtonEnum.YesNo).ShowAsync();
                switch (result)
                {
                    case ButtonResult.Yes:
                        {
                            EntranceTestingContext connection = new EntranceTestingContext();
                            int countHint = connection.QuestionHints.Where(tb => tb.IdHint == idHint).Count();
                            QuestionHint qh = connection.QuestionHints.FirstOrDefault(tb => tb.IdHint == idHint && tb.IdQuestion == Q.Id);
                            HintText hint = connection.HintTexts.FirstOrDefault(tb => tb.Id == idHint);
                            if (countHint > 1)
                            {
                                connection.QuestionHints.Remove(qh);
                            }
                            else
                            {
                                connection.HintTexts.Remove(hint);
                            }
                            connection.SaveChanges();
                            QHint = connection.QuestionHints.Include(tb => tb.IdHintNavigation).ThenInclude(tb => tb.HintImages).Where(tb => tb.IdQuestion == Q.Id).ToList();
                            await MessageBoxManager.GetMessageBoxStandard("�������� ���������", "��������� ������� �������", ButtonEnum.Ok).ShowAsync();
                            break;
                        }
                    case ButtonResult.No:
                        {
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("������ ��������", "�� ����� �������� �������� ������", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.Write(ex.Message);
#endif
            }
        }
        public async void DeleteHintImage(int idImage)
        {
            try
            {
                var result = await MessageBoxManager.GetMessageBoxStandard("�������� ��������", "�� ������������� ������ ������� �����������? ������������ ��� � ���������� ����� ������", ButtonEnum.YesNo).ShowAsync();
                switch (result)
                {
                    case ButtonResult.Yes:
                        {
                            EntranceTestingContext connection = new EntranceTestingContext();
                            HintImage hImage = connection.HintImages.FirstOrDefault(tb => tb.Id == idImage);
                            connection.HintImages.Remove(hImage);
                            connection.SaveChanges();
                            QHint = connection.QuestionHints.Include(tb => tb.IdHintNavigation).ThenInclude(tb => tb.HintImages).Where(tb => tb.IdQuestion == Q.Id).ToList();
                            await MessageBoxManager.GetMessageBoxStandard("�������� ��������", "�������� ������� �������", ButtonEnum.Ok).ShowAsync();
                            break;
                        }
                    case ButtonResult.No:
                        {
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("������ ��������", "�� ����� �������� �������� ������", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.Write(ex.Message);
#endif
            }
        }
        public async void DeleteQuestionImage(int idImage)
        {
            try
            {
                var result = await MessageBoxManager.GetMessageBoxStandard("�������� ��������", "�� ������������� ������ ������� �����������? ������������ ��� � ���������� ����� ������", ButtonEnum.YesNo).ShowAsync();
                switch (result)
                {
                    case ButtonResult.Yes:
                        {
                            EntranceTestingContext connection = new EntranceTestingContext();
                            QuestionImage qImage = connection.QuestionImages.FirstOrDefault(tb => tb.Id == idImage);
                            connection.QuestionImages.Remove(qImage);
                            connection.SaveChanges();
                            QImage = connection.QuestionImages.Where(tb => tb.IdQuestion == Q.Id).ToList();
                            await MessageBoxManager.GetMessageBoxStandard("�������� ��������", "�������� ������� �������", ButtonEnum.Ok).ShowAsync();
                            break;
                        }
                    case ButtonResult.No:
                        {
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("������ ��������", "�� ����� �������� �������� ������", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.Write(ex.Message);
#endif
            }
        }

        public async void QSaveChanges()
        {
            if (Editing)//���� ��������������
            {
                editSaveChanges();
            }
            else//���� ����������
            {
                bool flag = SelectedCategory == "�����������";
                string mess = "�� ������� ��� ������ ��������� ������? ��������� ��������� ����� ���������� ����� ����������.\n ��������: \n\t - ��������� ��������� �������;\n\t - ���������� � �������� ��������� �������" + ((flag) ? ";\n\t - ��������� �����������." : ".");
                var result = await MessageBoxManager.GetMessageBoxStandard("", mess, ButtonEnum.YesNo).ShowAsync();
                switch (result)
                {
                    case ButtonResult.Yes:
                        {
                            createSaveChanges();
                            break;
                        }
                    default:
                        break;
                }
            }

            if (QHint.Count > 0)
                hintSaveChanges();
        }
        private async void hintSaveChanges()
        {
            try
            {
                EntranceTestingContext connection = new EntranceTestingContext();
                int index = 0;

                while(true)
                {
                    bool flag = QHint[index].Cost > 0 && (!string.IsNullOrEmpty(QHint[index].IdHintNavigation.Text) || QHint[index].IdHintNavigation.HintImages.Count > 0);
                    if (flag)
                    {
                        connection.QuestionHints.Update(QHint[index]);
                        index++;
                    }
                    if (index == QHint.Count)
                        break;
                }
                connection.SaveChanges();
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("", "�� ����� ���������� ��������� ��������� ������.", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.Write(ex.Message);
#endif
            }
        }

        private async void editSaveChanges()
        {
            try
            {
                EntranceTestingContext connection = new EntranceTestingContext();
                int idCategory = CategoryIdFromSelectedComboBoxItem();
                if (idCategory != Q.IdCategory)
                    Q.IdCategory = idCategory;
                connection.Update(Q);
                switch (idCategory)
                {
                    case 1:
                    case 2:
                        {
                            connection.ElementOfChooses.UpdateRange(ChooseElement);
                            break;
                        }
                    case 3:
                    case 4:
                        {
                            connection.ElementOfArrangements.UpdateRange(ArrangementElement);
                            break;
                        }
                    case 5:
                        {
                            foreach (var item in MatchesElement)
                            {
                                item.Elem1_e.Name = item.Name1;
                                item.Elem2_e.Name = item.Name2;
                                connection.ElementOfEqualities.Update(item.Elem1_e);
                                connection.ElementOfEqualities.Update(item.Elem2_e);
                            }
                            break;
                        }
                    case 6:
                        {
                            connection.Groups.Update(MatchesGroup1);
                            connection.Groups.Update(MatchesGroup2);
                            foreach (var item in MatchesElement)
                            {
                                item.Elem1.Name = item.Name1;
                                item.Elem2.Name = item.Name2;
                                connection.ElementOfGroups.Update(item.Elem1);
                                connection.ElementOfGroups.Update(item.Elem2);
                            }
                            break;
                        }
                    case 7:
                        {
                            foreach (var item in MatchesMultiplyElement)
                            {
                                item.TP.Name = item.Text;
                                connection.TextOfPuttings.Update(item.TP);
                                connection.ElementOfPuttings.UpdateRange(item.ElementEditor);
                            }
                            break;
                        }
                }
                connection.SaveChanges();
                await MessageBoxManager.GetMessageBoxStandard("", "������ ������� ��������", ButtonEnum.Ok).ShowAsync();
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("", "�� ����� ���������� ������� �������� ������.", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.Write(ex.Message);
#endif
            }
        }

        private async void createSaveChanges()
        {
            try
            {
                EntranceTestingContext connection = new EntranceTestingContext();
                int idCategory = CategoryIdFromSelectedComboBoxItem();
                if (idCategory == -1)
                {
                    await MessageBoxManager.GetMessageBoxStandard("", "�� �� ������� ��������� �������, ���������� ���������� ����������", ButtonEnum.Ok, Icon.Error).ShowAsync();
                    return;
                }
                Q.IdCategory = idCategory;
                bool flag = true; //������ ��������
                switch (idCategory)
                {
                    case 1:
                    case 2:
                        {
                            Q.ElementOfChooses.Clear();
                            if (string.IsNullOrWhiteSpace(Q.Name))
                            {
                                await MessageBoxManager.GetMessageBoxStandard("", "�� �� ����� ������. ���������� ��������", ButtonEnum.Ok).ShowAsync();
                                return;
                            }
                            if (ChooseElement.Count() == 0 || ChooseElement.Where(tb => tb.Name == "").Count() == ChooseElement.Count())
                            {
                                await MessageBoxManager.GetMessageBoxStandard("", "�� �� �������� ��������. ���������� ��������", ButtonEnum.Ok).ShowAsync();
                                return;
                            }

                            bool correctly = ChooseElement.Where(tb => tb.Correctly == true).Count() > 0;
                            if (!correctly)
                            {
                                await MessageBoxManager.GetMessageBoxStandard("", "�� �� ������� �� ���� ���������� �����. ���������� ��������", ButtonEnum.Ok).ShowAsync();
                                return;
                            }
                            foreach (var item in ChooseElement)
                            {
                                if (!string.IsNullOrWhiteSpace(item.Name))
                                    Q.ElementOfChooses.Add(item);
                                else
                                    flag = false;
                            }
                            break;
                        }
                    case 3:
                    case 4:
                        {
                            Q.ElementOfArrangements.Clear();
                            if (string.IsNullOrWhiteSpace(Q.Name))
                            {
                                await MessageBoxManager.GetMessageBoxStandard("", "�� �� ����� ������. ���������� ��������", ButtonEnum.Ok).ShowAsync();
                                return;
                            }
                            if (ArrangementElement.Count() == 0 || ArrangementElement.Where(tb => tb.Name == "").Count() == ArrangementElement.Count())
                            {
                                await MessageBoxManager.GetMessageBoxStandard("", "�� �� �������� ��������. ���������� ��������", ButtonEnum.Ok).ShowAsync();
                                return;
                            }
                            //�������� �������
                            bool positionFlag = true;
                            int index = 1;
                            foreach (var item in ArrangementElement.OrderBy(tb => tb.Position))
                            {
                                if (item.Position != index)
                                    positionFlag = false;
                                index++;
                            }
                            if (!positionFlag)
                            {
                                await MessageBoxManager.GetMessageBoxStandard("", "�� �� ���������� ������� ���������. ���������� ��������", ButtonEnum.Ok).ShowAsync();
                                return;
                            }
                            //���������� ���������
                            Random.Shared.Shuffle(CollectionsMarshal.AsSpan(ArrangementElement.ToList()));
                            foreach (var item in ArrangementElement)
                            {
                                if (!string.IsNullOrWhiteSpace(item.Name))
                                    Q.ElementOfArrangements.Add(item);
                                else
                                    flag = false;
                            }
                            break;
                        }
                    case 5:
                        {
                            if (string.IsNullOrWhiteSpace(Q.Name))
                            {
                                await MessageBoxManager.GetMessageBoxStandard("", "�� �� ����� ������. ���������� ��������", ButtonEnum.Ok).ShowAsync();
                                return;
                            }

                            if (MatchesElement.Count() == 0 || MatchesElement.Where(tb => tb.Name1 == "" || tb.Name2 == "").Count() == MatchesElement.Count())
                            {
                                await MessageBoxManager.GetMessageBoxStandard("", "�� �� �������� �������� �����������. ���������� ��������", ButtonEnum.Ok).ShowAsync();
                                return;
                            }
                            //������ ��������, ������ ����� ������������� ��� ��� �� ������� � ���������� ��������
                            foreach (var item in MatchesElement)
                            {
                                if (string.IsNullOrWhiteSpace(item.Name1) || string.IsNullOrWhiteSpace(item.Name2))
                                    flag = false;
                            }
                            break;
                        }
                    case 6:
                        {
                            Q.Groups.Clear();
                            if (string.IsNullOrWhiteSpace(Q.Name))
                            {
                                await MessageBoxManager.GetMessageBoxStandard("", "�� �� ����� ������. ���������� ��������", ButtonEnum.Ok).ShowAsync();
                                return;
                            }
                            if (MatchesElement.Count() == 0 || MatchesElement.Where(tb => tb.Name1 == "" || tb.Name2 == "").Count() == MatchesElement.Count())
                            {
                                await MessageBoxManager.GetMessageBoxStandard("", "�� �� �������� �������� �����������. ���������� ��������", ButtonEnum.Ok).ShowAsync();
                                return;
                            }
                            if (string.IsNullOrWhiteSpace(MatchesGroup1.Name) || string.IsNullOrWhiteSpace(MatchesGroup2.Name))
                            {
                                await MessageBoxManager.GetMessageBoxStandard("", "�� �� ��������� �������� �����. ���������� ��������", ButtonEnum.Ok).ShowAsync();
                                return;
                            }

                            Group group1 = new Group() { Name = MatchesGroup1.Name };
                            Group group2 = new Group() { Name = MatchesGroup2.Name };
                            int index = 1;
                            foreach (var item in MatchesElement)
                            {
                                if (!string.IsNullOrWhiteSpace(item.Name1) && !string.IsNullOrWhiteSpace(item.Name2))
                                {
                                    group1.ElementOfGroups.Add(new ElementOfGroup() { Name = item.Name1, RatioNumeri = index });
                                    group2.ElementOfGroups.Add(new ElementOfGroup() { Name = item.Name2, RatioNumeri = index });
                                    index++;
                                }
                                else
                                    flag = false;


                            }
                            Q.Groups.Add(group1);
                            Q.Groups.Add(group2);
                            break;
                        }
                    case 7:
                        {
                            Q.TextOfPuttings.Clear();
                            if (MatchesMultiplyElement.Count() == 0)
                            {
                                await MessageBoxManager.GetMessageBoxStandard("", "�� �� �������� �������� �����������. ���������� ��������", ButtonEnum.Ok).ShowAsync();
                                return;
                            }
                            bool firstFlag = true;
                            foreach (var item in MatchesMultiplyElement)
                            {
                                if (string.IsNullOrEmpty(item.Text))
                                {
                                    flag = (firstFlag) ? true : false;
                                }
                                TextOfPutting tp = new TextOfPutting() { Name = item.Text };
                                bool correctly = item.ElementEditor.Where(tb => tb.Correctly == true).Count() > 0;
                                if (!correctly)
                                {
                                    await MessageBoxManager.GetMessageBoxStandard("", "�� �� ������� �� ���� ���������� �����. ���������� ��������", ButtonEnum.Ok).ShowAsync();
                                    return;
                                }
                                foreach (var elem in item.ElementEditor)
                                {
                                    if (!string.IsNullOrWhiteSpace(elem.Name))
                                        tp.ElementOfPuttings.Add(new ElementOfPutting() { Name = elem.Name, Correctly = elem.Correctly });
                                    else
                                        flag = false;
                                }
                                Q.TextOfPuttings.Add(tp);
                                firstFlag = false;
                            }
                            break;
                        }
                }
                if (!flag)
                {
                    var result = await MessageBoxManager.GetMessageBoxStandard("", "�� �� ��������� ��� ��������, �������  ���������� ��������", ButtonEnum.Ok).ShowAsync();
                    return;
                }
                if (idCategory == 5)
                {
                    Q.ElementOfEqualities.Clear();
                    foreach (var item in MatchesElement)
                    {
                        if (!string.IsNullOrWhiteSpace(item.Name1) && !string.IsNullOrWhiteSpace(item.Name2))
                        {
                            ElementOfEquality elem1 = new ElementOfEquality() { Name = item.Name1 };
                            ElementOfEquality elem2 = new ElementOfEquality() { Name = item.Name2 };
                            Q.ElementOfEqualities.Add(elem1);
                            Q.ElementOfEqualities.Add(elem2);
                            connection.Questions.Update(Q);
                            connection.SaveChanges();
                            RatioOfElementEquality ratio = new RatioOfElementEquality() { IdElement1 = elem1.Id, IdElement2 = elem2.Id };
                            connection.RatioOfElementEqualities.Add(ratio);
                        }
                    }
                }
                else
                    connection.Questions.Update(Q);
                connection.SaveChanges();
                await MessageBoxManager.GetMessageBoxStandard("", "������ ������� ��������", ButtonEnum.Ok).ShowAsync();

                Editing = true;
                Header = "�������������� �������";
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("", "�� ����� ���������� ������� �������� ������.", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.Write(ex.Message);
#endif
            }
        }
        /// <summary>
        /// ����� ��� ��������� Id ��������� ������� � ����������� �� ��������� �������� ComboBox
        /// </summary>
        /// <returns></returns>
        private int CategoryIdFromSelectedComboBoxItem()
        {
            switch (SelectedCategory)
            {
                case "����� ������":
                    {
                        if (SelectedChoseCategory == "1 ���������� ��������")
                            return 1;
                        else
                            return 2;
                    }
                case "�������������� ���������":
                    {
                        if (SelectedArrangmentCategory == "�������������")
                            return 3;
                        else
                            return 4;
                        break;
                    }
                case "�����������":
                    {
                        if (SelectedMatchCategory == "������ ���������")
                            return 6;
                        else
                            return 5;
                        break;
                    }
                case "����������� �������":
                    {
                        return 7;
                    }
                default:
                    return -1;
            }

        }
    }
}