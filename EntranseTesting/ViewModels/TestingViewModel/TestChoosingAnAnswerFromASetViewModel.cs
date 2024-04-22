using System;
using System.Collections.Generic;
using EntranseTesting.Models;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using ReactiveUI;
using System.Linq;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.ComponentModel;

namespace EntranseTesting.ViewModels
{
    public class TestChoosingAnAnswerFromASetViewModel : ReactiveObject
    {
        EntranceTestingContext baseConnection = new EntranceTestingContext();

        int numberTask;
        string question;
        ObservableCollection<ItemCAFS> element = new ObservableCollection<ItemCAFS>();
        List<QuestionImage> qImage = new List<QuestionImage>();
        public TestChoosingAnAnswerFromASetViewModel(int numberTask)
        {
            this.numberTask = numberTask;

            Question = baseConnection.Questions.FirstOrDefault(tb => tb.Id == numberTask).Name;
            QImage = baseConnection.QuestionImages.Where(tb => tb.IdQuestion == numberTask).ToList();

            List<TextOfPutting> _text = baseConnection.TextOfPuttings.Where(tb => tb.IdQuestion == numberTask).ToList();
            foreach (TextOfPutting text in _text)
            {
                List<string> _list = baseConnection.ElementOfPuttings.Where(tb => tb.IdText == text.Id).Select(tb => tb.Name).ToList();
                Random.Shared.Shuffle(CollectionsMarshal.AsSpan(_list));
                Element.Add(new ItemCAFS(text.Name,_list));
            }
            /*int responseIndex = Response.IndexResponse(numberTask);
            if (Response.responseUsers[responseIndex].UserResponseMultiplyAnswes.Count() > 0)//если пользователь отвечал
            {
                //заполняем данные
                List<TextOfPutting> _list = baseConnection.TextOfPuttings.Include(tb => tb.ElementOfPuttings).Where(tb => tb.IdQuestion == numberTask).ToList();
                List<UserResponseMultiplyAnswer> _response = Response.responseUsers[responseIndex].UserResponseMultiplyAnswes.ToList();
                for(int i = 0; i < _response.Count; i++)
                {
                    TextOfPutting t = _list.FirstOrDefault(tb => tb.Id == _response[i].IdText);
                    ElementOfPutting e = baseConnection.ElementOfPuttings.FirstOrDefault(tb => tb.Id == _response[i].IdElement);
                    int index = Element.IndexOf(Element.FirstOrDefault(tb => tb.Text == t.Name));
                    Element[index].SelectedItem = e.Name;
                }
            }*/
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
        public List<QuestionImage> QImage { get => qImage; set => qImage = value; }
    }
}