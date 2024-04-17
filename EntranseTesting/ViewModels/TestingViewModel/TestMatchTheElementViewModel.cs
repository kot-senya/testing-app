using System;
using System.Windows;
using System.Collections.Generic;
using EntranseTesting.Models;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using ReactiveUI;
using System.Linq;
using Avalonia.Input;
using System.Diagnostics;
using Avalonia.Interactivity;
using Avalonia.Controls;
using System.Reactive;
using CommunityToolkit.Mvvm.Input;
using Avalonia;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using DynamicData;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace EntranseTesting.ViewModels
{
    public partial class TestMatchTheElementViewModel : ObservableObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        EntranceTestingContext baseConnection = new EntranceTestingContext();

        [ObservableProperty]int numberTask;
        [ObservableProperty] string question;
        [ObservableProperty] string nameGroup1;
        [ObservableProperty] string nameGroup2;
        ObservableCollection<ElementOfGroup> elementMatchGroup1 = new ObservableCollection<ElementOfGroup>();
        ObservableCollection<ElementOfGroup> elementMatchGroup2 = new ObservableCollection<ElementOfGroup>();
        [ObservableProperty] List<ItemMatch> matches = new List<ItemMatch>();

        public TestMatchTheElementViewModel(int numberTask)
        {
            this.numberTask = numberTask;

            Question = baseConnection.Questions.FirstOrDefault(tb => tb.Id == numberTask).Name;

            List<Models.Group> _group = baseConnection.Groups.Where(tb => tb.IdQuestion == numberTask).ToList();
            NameGroup1 = _group[0].Name;
            NameGroup2 = _group[1].Name;

            List<ElementOfGroup> _list1 = baseConnection.ElementOfGroups.Include(tb => tb.IdGroupNavigation).Where(tb => tb.IdGroup == _group[0].Id).ToList();
            Random.Shared.Shuffle(CollectionsMarshal.AsSpan(_list1));
            foreach (ElementOfGroup elem in _list1)
                ElementMatchGroup1.Add(elem);

            List<ElementOfGroup> _list2 = baseConnection.ElementOfGroups.Include(tb => tb.IdGroupNavigation).Where(tb => tb.IdGroup == _group[1].Id).ToList();
            Random.Shared.Shuffle(CollectionsMarshal.AsSpan(_list2));
            foreach (ElementOfGroup elem in _list2)
                ElementMatchGroup2.Add(elem);
        }

        public ObservableCollection<ElementOfGroup> ElementMatchGroup1 { get => elementMatchGroup1; set { elementMatchGroup1 = value; OnPropertyChanged(); } }
        public ObservableCollection<ElementOfGroup> ElementMatchGroup2 { get => elementMatchGroup2; set { elementMatchGroup2 = value; OnPropertyChanged(); } }

        public void MatchLine(ElementOfGroup elem, ref Border border)
        {
            ItemMatch item = Matches.FirstOrDefault(tb => tb.Elem1 == null || tb.Elem2 == null);

            if (item == null)
                item = new ItemMatch();
            else
                Matches.Remove(item);

            ElementOfGroup elem1 = ElementMatchGroup1.FirstOrDefault(tb => tb.Id == elem.Id);
            ElementOfGroup elem2 = ElementMatchGroup2.FirstOrDefault(tb => tb.Id == elem.Id);

            Matches.RemoveMany(matches.Where(tb => tb.Elem1 == elem1 || tb.Elem2 == elem2).ToList());

            item.NumGroup = numMatch();

            nullableValue();

            if (elem1 != null && item.Elem1 == null)
            {
                item.Elem1 = elem1;
            }
            else if (elem2 != null && item.Elem2 == null)
            {
                item.Elem2 = elem2;
            }
            else
            {
                Debug.WriteLine($"Elem with id '{elem.Id}' not found");
                return;
            }
            
            Matches.Add(item);
            if (item.Elem1 != null)
            {
                int i = ElementMatchGroup1.IndexOf(item.Elem1);
                ElementMatchGroup1[i].IsActive = true;
                ElementMatchGroup1[i].NumGroup = item.NumGroup;
            }
            if (item.Elem2 != null) 
            { 
                int i = ElementMatchGroup2.IndexOf(item.Elem2);
                ElementMatchGroup2[i].IsActive = true;
                ElementMatchGroup2[i].NumGroup = item.NumGroup;
            }    
           
        }
        
        private int numMatch() 
        {
            int value = -1;

            if (Matches.Count == 0)
                return 1;

            for(int i = 1; i <= ElementMatchGroup1.Count; i++)
            {
                if(Matches.Where(tb => tb.NumGroup == i).Count() == 0)
                    return i;
            }
            return value;
        }
        private void nullableValue()
        {
            foreach(ElementOfGroup e in ElementMatchGroup1)
            {
                if(Matches.Where(tb => tb.Elem1 == e).Count() == 0)
                {
                    int i = ElementMatchGroup1.IndexOf(e);
                    ElementMatchGroup1[i].IsActive = false;
                    if (Matches.Where(tb=>tb.Elem1 == e).Count() == 0)
                        ElementMatchGroup1[i].NumGroup = 0;
                }
            }
            foreach (ElementOfGroup e in ElementMatchGroup2)
            {
                if (Matches.Where(tb => tb.Elem2 == e).Count() == 0)
                {
                    int i = ElementMatchGroup2.IndexOf(e);
                    ElementMatchGroup2[i].IsActive = false;
                    if (Matches.Where(tb => tb.Elem2 == e).Count() == 0)
                        ElementMatchGroup2[i].NumGroup = 0;
                }
            }
        }
    }
}
