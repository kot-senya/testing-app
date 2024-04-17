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
using CommunityToolkit.Mvvm.ComponentModel;

namespace EntranseTesting.ViewModels
{
    public partial class TestArrangementOfElementsViewModel : ObservableObject
    {
        EntranceTestingContext baseConnection = new EntranceTestingContext();

        public const string CustomFormat = "element-item-format";
        [ObservableProperty] int numberTask;
        [ObservableProperty] string question;
        [ObservableProperty] Orientation stackLayoutOrientation;
        [ObservableProperty] private ElementOfArrangement? draggingElementItem;
        ObservableCollection<ElementOfArrangement> element = new ObservableCollection<ElementOfArrangement>();

        public ObservableCollection<ElementOfArrangement> Element { get => element; set => element = value; }

        public TestArrangementOfElementsViewModel(int numberTask, Orientation orientation)
        {
            this.numberTask = numberTask;

            Question = baseConnection.Questions.FirstOrDefault(tb => tb.Id == numberTask).Name;
            StackLayoutOrientation = orientation;

            int responseIndex = Response.IndexResponse(numberTask);
            if (Response.responseUsers[responseIndex].UserResponseArrangements.Count == 0)//если пользователь не отвечал
            {
                //заполняем данные
                List<ElementOfArrangement> _list = baseConnection.ElementOfArrangements.Where(tb => tb.IdQuestion == numberTask).ToList();
                Random.Shared.Shuffle(CollectionsMarshal.AsSpan(_list));
                foreach (ElementOfArrangement elem in _list)
                    Element.Add(elem);
                int i = 1;
                //записываем в шаблон ответа            
                foreach (ElementOfArrangement elem in _list)
                    Response.responseUsers[responseIndex].UserResponseArrangements.Add(new UserResponseArrangement { IdElement = elem.Id, Position = i++ });
            }
            else//если пользователь отвечал
            {
                //заполняем данные
                List<ElementOfArrangement> _list = baseConnection.ElementOfArrangements.Where(tb => tb.IdQuestion == numberTask).ToList();
                List<UserResponseArrangement> _response = Response.responseUsers[responseIndex].UserResponseArrangements.ToList();
                _response = _response.OrderBy(tb => tb.Position).ToList();
                foreach (UserResponseArrangement item in _response)
                {
                    ElementOfArrangement elem = _list.FirstOrDefault(tb => tb.Id == item.IdElement);
                    Element.Add(elem);
                }
            }
        }

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
                difference = (int)Math.Round((endPosition.X - startPosition.X + 10) / DraggingElementItem.Width);
            else if (StackLayoutOrientation == Orientation.Vertical)
                difference = (int)Math.Round((endPosition.Y - startPosition.Y + 10) / DraggingElementItem.Height);

            if (difference != 0)
            {
                Element.Remove(item);
                difference = (difference + indexItem < 0) ? 0 : (difference + indexItem > Element.Count) ? Element.Count : difference + indexItem;
                Element.Insert(difference, item);
            }
        }
   
    }
}
