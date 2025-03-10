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
        List<QuestionImage> qImage = new List<QuestionImage>();
        ObservableCollection<ElementOfChoose> element = new ObservableCollection<ElementOfChoose>();

        public TestChoosing1AnswerViewModel(int numberTask)
        {
            this.numberTask = numberTask;

            Question = baseConnection.Questions.FirstOrDefault(tb => tb.Id == numberTask).Name;
            QImage = baseConnection.QuestionImages.Where(tb => tb.IdQuestion == numberTask).ToList();

            int responseIndex = Response.IndexResponse(numberTask);
            if (Response.responseUsers[responseIndex].UserResponseChooseAnswers.Count == 0)//���� ������������ �� �������
            {
                List<ElementOfChoose> _list = baseConnection.ElementOfChooses.Where(tb => tb.IdQuestion == numberTask).ToList();
                Random.Shared.Shuffle(CollectionsMarshal.AsSpan(_list));
                foreach (ElementOfChoose elem in _list)
                    Element.Add(elem);
                //���������� � ������ ������            
                foreach (ElementOfChoose elem in _list)
                    Response.responseUsers[responseIndex].UserResponseChooseAnswers.Add(new UserResponseChooseAnswer { IdElement = elem.Id });
            }
            else//���� ������������ �������
            {
                //��������� ������
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
        public List<QuestionImage> QImage { get => qImage; set => qImage = value; }
    }
}