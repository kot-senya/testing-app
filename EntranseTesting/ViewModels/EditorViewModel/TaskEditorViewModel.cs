using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        //элементы страницы
        [ObservableProperty] UserControl? taskUC = null;
        [ObservableProperty] bool editing = false;
        [ObservableProperty] string header = "";
        [ObservableProperty] List<string> category = ["--", "Выбор ответа", "Упорядочивание элементов", "Соотношение", "Подстановка ответов"];
        [ObservableProperty] List<string> _choseCategory = ["1 правильное значение", "несколько правильных значений"];
        [ObservableProperty] List<string> _arrangmentCategory = ["горизонтально", "вертикально"];
        [ObservableProperty] List<string> _matchCategory = ["группы элементов", "равные величины"];
        string selectedCategory = "--";
        string selectedMatchCategory = "группы элементов";
        [ObservableProperty] bool selectedGroup = true;
        [ObservableProperty] string selectedChoseCategory = "1 правильное значение";
        [ObservableProperty] string selectedArrangmentCategory = "горизонтально";

        //элементы вопроса
        [ObservableProperty] Question q;
        List<QuestionImage> qImage;
        List<QuestionHint> qHint;
        ObservableCollection<ElementOfChoose> chooseElement = new ObservableCollection<ElementOfChoose>();
        [ObservableProperty] string? nameMatchesGroup1 = "";
        [ObservableProperty] string? nameMatchesGroup2 = "";
        ObservableCollection<ItemMatch> matchesElement = new ObservableCollection<ItemMatch>();
        ObservableCollection<ItemCAFS> matchesMultiplyElement = new ObservableCollection<ItemCAFS>();
        ObservableCollection<ElementOfArrangement> arrangementElement = new ObservableCollection<ElementOfArrangement>();

        public TaskEditorViewModel()
        {
            Header = "Добавление вопроса";
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

            Header = "Редактирование вопроса";
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
            //изображения
            QImage = baseConnection.QuestionImages.Where(tb => tb.IdQuestion == Q.Id).ToList();
            if (QImage.Count == 0) QImage = new List<QuestionImage>();
            //подсказки
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
                if (value == "группы элементов")
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
                    case "Вопрос с 1 вариантом ответа":
                        {
                            ChooseElement.Clear();
                            foreach (var item in Q.ElementOfChooses)
                                ChooseElement.Add(item);
                            SelectedChoseCategory = "1 правильное значение";
                            SelectedCategory = "Выбор ответа";
                            break;
                        }
                    case "Вопрос с несколькими вариантами ответа":
                        {
                            ChooseElement.Clear();
                            foreach (var item in Q.ElementOfChooses)
                                ChooseElement.Add(item);
                            SelectedChoseCategory = "несколько правильных значений";
                            SelectedCategory = "Выбор ответа";
                            break;
                        }
                    case "Горизонтальное упорядочивание элементов":
                        {
                            ArrangementElement.Clear();
                            foreach (var item in Q.ElementOfArrangements)
                                ArrangementElement.Add(item);
                            SelectedArrangmentCategory = "горизонтально";
                            SelectedCategory = "Упорядочивание элементов";
                            break;
                        }
                    case "Вертикальное упорядочивание элементов":
                        {
                            ArrangementElement.Clear();
                            foreach (var item in Q.ElementOfArrangements)
                                ArrangementElement.Add(item);
                            SelectedArrangmentCategory = "вертикально";
                            SelectedCategory = "Упорядочивание элементов";
                            break;
                        }
                    case "Соотношение элементов":
                        {
                            MatchesElement.Clear();
                            Group g1 = Q.Groups.ToList()[0];
                            Group g2 = Q.Groups.ToList()[1];
                            List<ElementOfGroup> e1 = g1.ElementOfGroups.ToList();
                            List<ElementOfGroup> e2 = g2.ElementOfGroups.ToList();
                            NameMatchesGroup1 = g1.Name;
                            NameMatchesGroup2 = g2.Name;
                            for (int i = 0; i < e1.Count; i++)
                                MatchesElement.Add(new ItemMatch(e1[i].Name, e2[i].Name));
                            SelectedMatchCategory = "группы элементов";
                            SelectedCategory = "Соотношение";
                            break;
                        }
                    case "Соотношение величин":
                        {
                            MatchesElement.Clear();
                            List<ElementOfEquality> e = Q.ElementOfEqualities.ToList();
                            for (int i = 0; i < e.Count; i++)
                            {
                                RatioOfElementEquality elem = (e[i].RatioOfElementEqualityIdElement1Navigations.Count == 1) ? e[i].RatioOfElementEqualityIdElement1Navigations.First() : e[i].RatioOfElementEqualityIdElement2Navigations.First();
                                ItemMatch item = new ItemMatch(elem.IdElement1Navigation.Name, elem.IdElement2Navigation.Name);
                                if (!MatchesElement.Contains(item))
                                    MatchesElement.Add(item);
                            }
                            SelectedMatchCategory = "равные величины";
                            SelectedCategory = "Соотношение";
                            break;
                        }
                    case "Подстановка ответов":
                        {
                            MatchesMultiplyElement.Clear();
                            List<TextOfPutting> tp = Q.TextOfPuttings.ToList();
                            for (int i = 0; i < tp.Count; i++)
                            {
                                List<ElementOfPutting> ep = tp[i].ElementOfPuttings.ToList();
                                MatchesMultiplyElement.Add(new ItemCAFS(tp[i].Name, ep));
                            }
                                SelectedCategory = "Подстановка ответов";
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Во время загрузки данных возникла ошибка", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.Write(ex.Message);
#endif
            }

        }
        private void loadUC()
        {
            switch (SelectedCategory)
            {
                case "Выбор ответа":
                    {
                        TaskUC = new EditorChoosing();
                        break;
                    }
                case "Упорядочивание элементов":
                    {
                        TaskUC = new EditorArrangement();
                        break;
                    }
                case "Соотношение":
                    {
                        TaskUC = new EditorMatches();
                        break;
                    }
                case "Подстановка ответов":
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
                case "Выбор ответа":
                    {
                        ChooseElement.Add(new ElementOfChoose());
                        break;
                    }
                case "Упорядочивание элементов":
                    {
                        ArrangementElement.Add(new ElementOfArrangement());
                        break;
                    }
                case "Соотношение":
                    {
                        MatchesElement.Add(new ItemMatch());
                        break;
                    }
                case "Подстановка ответов":
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
                await MessageBoxManager.GetMessageBoxStandard("Ошибка удаления", "Во время удаления возникла ошибка", ButtonEnum.Ok).ShowAsync();
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
                    case "Выбор ответа":
                        {
                            ChooseElement.Remove((ElementOfChoose)elem);
                            break;
                        }
                    case "Упорядочивание элементов":
                        {
                            ArrangementElement.Remove((ElementOfArrangement)elem);
                            break;
                        }
                    case "Соотношение":
                        {
                            MatchesElement.Remove((ItemMatch)elem);
                            break;
                        }
                    case "Подстановка ответов":
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
                await MessageBoxManager.GetMessageBoxStandard("Ошибка удаления", "Во время удаления возникла ошибка", ButtonEnum.Ok).ShowAsync();
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
            //если подсказок нет и подсказка содержит текст или изображения
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
                await MessageBoxManager.GetMessageBoxStandard("Ошибка добавления", "У вас уже есть пустая подсказка, заполните сначала ее", ButtonEnum.Ok).ShowAsync();
        }
        public async void DeleteHint(int idHint)
        {
            try
            {
                var result = await MessageBoxManager.GetMessageBoxStandard("", "Вы действительно хотите удалить подсказку? Восстановить ее в дальнейшем будет нельзя", ButtonEnum.YesNo).ShowAsync();
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
                            await MessageBoxManager.GetMessageBoxStandard("Удаление подсказки", "Подсказка успешно удалена", ButtonEnum.Ok).ShowAsync();
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
                await MessageBoxManager.GetMessageBoxStandard("Ошибка удаления", "Во время удаления возникла ошибка", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.Write(ex.Message);
#endif
            }
        }

        public async void DeleteHintImage(int idImage)
        {
            try
            {
                var result = await MessageBoxManager.GetMessageBoxStandard("Удаление картинки", "Вы действительно хотите удалить изображение? Восстановить его в дальнейшем будет нельзя", ButtonEnum.YesNo).ShowAsync();
                switch (result)
                {
                    case ButtonResult.Yes:
                        {
                            EntranceTestingContext connection = new EntranceTestingContext();
                            HintImage hImage = connection.HintImages.FirstOrDefault(tb => tb.Id == idImage);
                            connection.HintImages.Remove(hImage);
                            connection.SaveChanges();
                            QHint = connection.QuestionHints.Include(tb => tb.IdHintNavigation).ThenInclude(tb => tb.HintImages).Where(tb => tb.IdQuestion == Q.Id).ToList();
                            await MessageBoxManager.GetMessageBoxStandard("Удаление картинки", "Картинка успешно удалена", ButtonEnum.Ok).ShowAsync();
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
                await MessageBoxManager.GetMessageBoxStandard("Ошибка удаления", "Во время удаления возникла ошибка", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.Write(ex.Message);
#endif
            }
        }

        public async void DeleteQuestionImage(int idImage)
        {
            try
            {
                var result = await MessageBoxManager.GetMessageBoxStandard("Удаление картинки", "Вы действительно хотите удалить изображение? Восстановить его в дальнейшем будет нельзя", ButtonEnum.YesNo).ShowAsync();
                switch (result)
                {
                    case ButtonResult.Yes:
                        {
                            EntranceTestingContext connection = new EntranceTestingContext();
                            QuestionImage qImage = connection.QuestionImages.FirstOrDefault(tb => tb.Id == idImage);
                            connection.QuestionImages.Remove(qImage);
                            connection.SaveChanges();
                            QImage = connection.QuestionImages.Where(tb => tb.IdQuestion == Q.Id).ToList();
                            await MessageBoxManager.GetMessageBoxStandard("Удаление картинки", "Картинка успешно удалена", ButtonEnum.Ok).ShowAsync();
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
                await MessageBoxManager.GetMessageBoxStandard("Ошибка удаления", "Во время удаления возникла ошибка", ButtonEnum.Ok).ShowAsync();
#if DEBUG
                Debug.Write(ex.Message);
#endif
            }
        }
    }
}