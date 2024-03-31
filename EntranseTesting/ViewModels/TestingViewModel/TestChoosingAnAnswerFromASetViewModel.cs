using System;
using System.Collections.Generic;
using EntranseTesting.Models;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using ReactiveUI;
using System.Linq;

namespace EntranseTesting.ViewModels
{
    public class TestChoosingAnAnswerFromASetViewModel : ReactiveObject
    {
        EntranceTestingContext baseConnection = new EntranceTestingContext();

        int numberTask;
        string question;
        ObservableCollection<ItemCAFS> element = new ObservableCollection<ItemCAFS>();

        public TestChoosingAnAnswerFromASetViewModel(int numberTask)
        {
            this.numberTask = numberTask;

            Question = baseConnection.Questions.FirstOrDefault(tb => tb.Id == numberTask).Name;

            List<TextOfPutting> _text = baseConnection.TextOfPuttings.Where(tb => tb.IdQuestion == numberTask).ToList();
            foreach (TextOfPutting text in _text)
            {
                List<string> _list = baseConnection.ElementOfPuttings.Where(tb => tb.IdText == text.Id).Select(tb => tb.Name).ToList();
                Random.Shared.Shuffle(CollectionsMarshal.AsSpan(_list));
                Element.Add(new ItemCAFS(text.Name,_list));
            }
            
        }
        public bool QuestionVisible
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Question))
                    return false;
                return true;
            }
        }
        public string? Question { get => question; set => question = value; }
        public ObservableCollection<ItemCAFS> Element { get => element; set => element = value; }
    }
}