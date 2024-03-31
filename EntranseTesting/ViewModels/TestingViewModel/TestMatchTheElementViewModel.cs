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

namespace EntranseTesting.ViewModels
{
    public class TestMatchTheElementViewModel : ReactiveObject
    {
        EntranceTestingContext baseConnection = new EntranceTestingContext();

        int numberTask;
        string question;
        string nameGroup1;
        string nameGroup2;
        ObservableCollection<ElementOfGroup> elementMatchGroup1 = new ObservableCollection<ElementOfGroup>();
        ObservableCollection<ElementOfGroup> elementMatchGroup2 = new ObservableCollection<ElementOfGroup>();
        ObservableCollection<ItemLine> lines = new ObservableCollection<ItemLine>();

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

        public string Question { get => question; set => question = value; }
        public string NameGroup1 { get => nameGroup1; set => nameGroup1 = value; }
        public string NameGroup2 { get => nameGroup2; set => nameGroup2 = value; }
        public ObservableCollection<ElementOfGroup> ElementMatchGroup1 { get => elementMatchGroup1; set => elementMatchGroup1 = value; }
        public ObservableCollection<ElementOfGroup> ElementMatchGroup2 { get => elementMatchGroup2; set => elementMatchGroup2 = value; }
        public ObservableCollection<ItemLine> Lines { get => lines; set => this.RaiseAndSetIfChanged(ref lines, value); }

        public void MatchLine(ElementOfGroup elem, ref Border border)
        {
            /*
            ItemLine item = Lines.FirstOrDefault(tb => tb.Elem1 == null || tb.Elem2 == null);

            if (item == null)
                item = new ItemLine();
            else
                Lines.Remove(item);

            ElementOfGroup elem1 = ElementMatchGroup1.FirstOrDefault(tb => tb.Id == elem.Id);
            ElementOfGroup elem2 = ElementMatchGroup2.FirstOrDefault(tb => tb.Id == elem.Id);
            
            item.Border = border;

            if(elem1 != null && item.Elem1 == null)
            {
                item.Elem1 = elem1;
                TextBlock tb = (TextBlock)border.Child;
                int x = Convert.ToInt32(tb.Bounds.Top - tb.Bounds.Height / 2);
                int y = Convert.ToInt32(tb.Bounds.Left);
                item.StartPoint = new(x, y);

                if(item.Elem2 == null)
                    item.EndPoint = new(x, y);
            }
            else if (elem2 != null && item.Elem2 == null)
            {
                item.Elem2 = elem2;
                TextBlock tb = (TextBlock)border.Child;
                int x = Convert.ToInt32(tb.Bounds.Top - tb.Bounds.Height / 2);
                int y = Convert.ToInt32(tb.Bounds.Right);
                item.EndPoint = new(x, y);

                if (item.Elem1 == null)
                    item.StartPoint = new(x, y);
            }
            else
            {
                Debug.WriteLine($"Elem with id '{elem.Id}' not found");
                return;
            }

            Lines.Add(item);*/
        }
    }
}
