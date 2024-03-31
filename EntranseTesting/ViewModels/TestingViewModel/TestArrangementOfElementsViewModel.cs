using System;
using System.Collections.Generic;
using Avalonia.Layout;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ReactiveUI;
using Avalonia;
using System.Linq;
using EntranseTesting.Models;
using System.Runtime.InteropServices;

namespace EntranseTesting.ViewModels
{
    public class TestArrangementOfElementsViewModel : ReactiveObject
    {
        EntranceTestingContext baseConnection = new EntranceTestingContext();

        public const string CustomFormat = "element-item-format";
        int numberTask;
        string question;
        Orientation stackLayoutOrientation;
        ObservableCollection<ElementOfArrangement> element = new ObservableCollection<ElementOfArrangement>();
        private ElementOfArrangement? draggingElementItem;

        public TestArrangementOfElementsViewModel(int numberTask, Orientation orientation)
        {
            this.numberTask = numberTask;

            Question = baseConnection.Questions.FirstOrDefault(tb => tb.Id == numberTask).Name;
            StackLayoutOrientation = orientation;

            List<ElementOfArrangement> _list = baseConnection.ElementOfArrangements.Where(tb => tb.IdQuestion == numberTask).ToList();
            Random.Shared.Shuffle(CollectionsMarshal.AsSpan(_list));
            foreach (ElementOfArrangement elem in _list)
                Element.Add(elem);

        }

        public string Question { get => question; set => this.RaiseAndSetIfChanged(ref question, value); }
        public Orientation StackLayoutOrientation { get => stackLayoutOrientation; set => this.RaiseAndSetIfChanged(ref stackLayoutOrientation, value); }
        public ObservableCollection<ElementOfArrangement> Element { get => element; set => this.RaiseAndSetIfChanged(ref element, value); }
        public ElementOfArrangement? DraggingElementItem { get => draggingElementItem; set => this.RaiseAndSetIfChanged(ref draggingElementItem, value); }


        public void StartDrag(ElementOfArrangement elem)
        {
            DraggingElementItem = elem;
        }

        public void Drop(ElementOfArrangement elem, Point startPosition, Point endPosition)
        {
            ElementOfArrangement item = Element.FirstOrDefault(t => t.Name == elem.Name);
            if (item is null)
            {
                Debug.WriteLine($"Elem with id '{elem.Name}' not found");
                return;
            }

            int indexItem = Element.IndexOf(item);
            int difference = 0;

            if (StackLayoutOrientation == Orientation.Horizontal)
                difference = (int)Math.Round((endPosition.X - startPosition.X + 20) / 150);
            else if (StackLayoutOrientation == Orientation.Vertical)
                difference = (int)Math.Round((endPosition.Y - startPosition.Y + 10) / 50);

            if (difference != 0)
            {
                Element.Remove(item);
                difference = (difference + indexItem < 0) ? 0 : (difference + indexItem > Element.Count) ? Element.Count : difference + indexItem;
                Element.Insert(difference, item);
            }

        }
    }
}
