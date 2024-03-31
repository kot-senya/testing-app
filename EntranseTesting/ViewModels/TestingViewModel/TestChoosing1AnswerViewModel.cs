using System;
using System.Collections.Generic;
using Avalonia.Layout;
using EntranseTesting.Models;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.Linq;
using System.Runtime.InteropServices;

namespace EntranseTesting.ViewModels
{
	public class TestChoosing1AnswerViewModel : ReactiveObject
	{
        EntranceTestingContext baseConnection = new EntranceTestingContext();

        int numberTask;
        string question;
        ObservableCollection<ElementOfChoose> element = new ObservableCollection<ElementOfChoose>();

        public TestChoosing1AnswerViewModel(int numberTask)
        {
            this.numberTask = numberTask;

            Question = baseConnection.Questions.FirstOrDefault(tb => tb.Id == numberTask).Name;

            List<ElementOfChoose> _list = baseConnection.ElementOfChooses.Where(tb => tb.IdQuestion == numberTask).ToList();
            Random.Shared.Shuffle(CollectionsMarshal.AsSpan(_list));
            foreach (ElementOfChoose elem in _list)
                Element.Add(elem);
        }

        public string Question { get => question; set => question = value; }
        public ObservableCollection<ElementOfChoose> Element { get => element; set => element = value; }
    }
}