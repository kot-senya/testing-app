using System;
using System.Collections.Generic;
using EntranseTesting.Models;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using ReactiveUI;
using System.Linq;

namespace EntranseTesting.ViewModels
{
    public class TestChoosingMultipleAnswersViewModel : ReactiveObject
    {
        EntranceTestingContext baseConnection = new EntranceTestingContext();

        int numberTask;
        string question;
        ObservableCollection<ElementOfChoose> element = new ObservableCollection<ElementOfChoose>();

        public TestChoosingMultipleAnswersViewModel(int numberTask)
        {
            this.numberTask = numberTask;

            Question = baseConnection.Questions.FirstOrDefault(tb => tb.Id == numberTask).Name;
            int responseIndex = Response.IndexResponse(numberTask);
            if (Response.responseUsers[responseIndex].UserResponseChooseAnswers.Count == 0)//если пользователь не отвечал
            {
                List<ElementOfChoose> _list = baseConnection.ElementOfChooses.Where(tb => tb.IdQuestion == numberTask).ToList();
                Random.Shared.Shuffle(CollectionsMarshal.AsSpan(_list));
                foreach (ElementOfChoose elem in _list)
                    Element.Add(elem);
                //записываем в шаблон ответа            
                foreach (ElementOfChoose elem in _list)
                    Response.responseUsers[responseIndex].UserResponseChooseAnswers.Add(new UserResponseChooseAnswer { IdElement = elem.Id });
            }
            else//если пользователь отвечал
            {
                //заполняем данные
                List<ElementOfChoose> _list = baseConnection.ElementOfChooses.Where(tb => tb.IdQuestion == numberTask).ToList();
                List<UserResponseChooseAnswer> _response = Response.responseUsers[responseIndex].UserResponseChooseAnswers.ToList();
                foreach (UserResponseChooseAnswer item in _response)
                {
                    ElementOfChoose elem = _list.FirstOrDefault(tb => tb.Id == item.IdElement);
                    elem.UserCorrectly = (item.Usercorrectly == null) ? false : (bool)item.Usercorrectly;
                    Element.Add(elem);
                }
            }
        }

        public string Question { get => question; set => question = value; }
        public ObservableCollection<ElementOfChoose> Element { get => element; set => element = value; }
    }
}