using Avalonia.Threading;
using DynamicData.Binding;
using EntranseTesting.Models.customClass;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntranseTesting.Models
{
    public class Response
    {
        public static UserSession userSession = new UserSession();
        public static List<UserResponse> responseUsers = new List<UserResponse>();
        public static List<QuestionHint> questionHints = new List<QuestionHint>();
        public static bool change = false;
        public static DispatcherTimer timer = new DispatcherTimer();
        public static void loadHints(List<Question> questionsCollection)
        {
            EntranceTestingContext connection = new EntranceTestingContext();
            questionHints.Clear();
            foreach (var item in questionsCollection)
            {
                List<QuestionHint> list = connection.QuestionHints.Include(tb => tb.IdHintNavigation).ThenInclude(tb => tb.HintImages).Where(tb => tb.IdQuestion == item.Id).ToList();
                questionHints.AddRange(list);
            }
        }
        public static string CostLine(int Cost)
        {
            switch (Cost % 10)
            {
                case 1:
                    {
                        return Cost + " очко";
                    }
                case 2:
                case 3:
                case 4:
                    {
                        return Cost + " очка";
                    }
                default:
                    {
                        return Cost + " очков";
                    }
            }
        }
        public static int IndexResponse(int idQuestion)
        {
            UserResponse item = responseUsers.FirstOrDefault(tb => tb.IdQuestion == idQuestion);
            if (item == null)
                return -1;
            return responseUsers.IndexOf(item);
        }
        public static void SaveChangesArrangement(int idQuestion, ObservableCollection<ElementOfArrangement> element)
        {
            int responseIndex = IndexResponse(idQuestion);
            List<UserResponseArrangement> list = responseUsers[responseIndex].UserResponseArrangements.ToList();
            responseUsers[responseIndex].UserResponseArrangements.Clear();
            int i = 1;
            foreach (var item in element)
            {
                int index = list.IndexOf(list.FirstOrDefault(tb => tb.IdElement == item.Id));
                list[index].Position = i++;
            }
            foreach (var item in list)
                responseUsers[responseIndex].UserResponseArrangements.Add(item);
            change = true;
            //проверка задания
            bool correctly = true;
            List<ElementOfArrangement> elem = element.OrderBy(tb => tb.Position).ToList();
            list = responseUsers[responseIndex].UserResponseArrangements.OrderBy(tb => tb.Position).ToList();
            for (int j = 0; i < elem.Count; j++)
            {
                if (elem[j].Id != list[j].IdElement)
                    correctly = false;
            }
            responseUsers[responseIndex].Correctly = correctly;
        }
        public static void SaveChangesChoosing(int idQuestion, bool manyAnswer, ObservableCollection<ElementOfChoose> element)
        {
            change = false;
            int responseIndex = IndexResponse(idQuestion);
            List<UserResponseChooseAnswer> list = responseUsers[responseIndex].UserResponseChooseAnswers.ToList();
            responseUsers[responseIndex].UserResponseChooseAnswers.Clear();
            if (list.Count > 0)
            {
                foreach (var item in element)
                {
                    int index = list.IndexOf(list.FirstOrDefault(tb => tb.IdElement == item.Id));
                    list[index].Usercorrectly = (item.UserCorrectly == null || item.UserCorrectly == false) ? false : item.UserCorrectly;
                    if ((bool)list[index].Usercorrectly) change = true;
                }                
            }
            //проверка задания
            if (change)
            {
                foreach (var item in list) responseUsers[responseIndex].UserResponseChooseAnswers.Add(item);

                bool correctly = true;
                if (manyAnswer)
                {
                    foreach (var item in list)
                    {
                        var correctItem = element.FirstOrDefault(tb => tb.Id == item.IdElement);
                        if (correctItem.Correctly != item.Usercorrectly)
                            correctly = false;
                    }
                }
                else
                {
                    var userCorrectly = list.FirstOrDefault(tb => tb.Usercorrectly == true);
                    var elem = element.FirstOrDefault(tb => tb.Id == userCorrectly.IdElement);
                    if (elem.Correctly != true)
                        correctly = false;
                }
                responseUsers[responseIndex].Correctly = correctly;
            }
            else
                responseUsers[responseIndex].Correctly = false;
        }
        public static void SaveChangesCAFS(int idQuestion, ObservableCollection<ItemCAFS> element)
        {
            change = true;
            EntranceTestingContext connection = new EntranceTestingContext();

            int responseIndex = IndexResponse(idQuestion);
            responseUsers[responseIndex].UserResponseMultiplyAnswers.Clear();

            foreach (var item in element)
            {
                if (item.SelectedItem != "--")
                {
                    TextOfPutting t = connection.TextOfPuttings.FirstOrDefault(tb => tb.Name == item.Text && tb.IdQuestion == idQuestion);
                    ElementOfPutting e = connection.ElementOfPuttings.FirstOrDefault(tb => tb.Name == item.SelectedItem && tb.IdText == t.Id);
                    responseUsers[responseIndex].UserResponseMultiplyAnswers.Add(new UserResponseMultiplyAnswer() { IdElement = e.Id, IdText = t.Id });
                }
                else
                {
                    change = false;
                }
            }

            //проверка задания
            if (change)
            {
                bool correctly = true;
                List<UserResponseMultiplyAnswer> _list = responseUsers[responseIndex].UserResponseMultiplyAnswers.ToList();

                for (int i = 0; i < _list.Count; i++)
                {
                    ElementOfPutting e = connection.ElementOfPuttings.FirstOrDefault(tb => tb.Id == _list[i].IdElement);
                    if (e.Correctly != true)
                        correctly = false;
                }
                responseUsers[responseIndex].Correctly = correctly;
            }
            else
                responseUsers[responseIndex].Correctly = false;

        }
        public static void SaveChangesMatchTheElement(int idQuestion, List<ItemMatch> element)
        {
            EntranceTestingContext connection = new EntranceTestingContext();
            int countMatch = connection.Groups.Include(tb => tb.ElementOfGroups).FirstOrDefault(tb => tb.IdQuestion == idQuestion).ElementOfGroups.Count();
            change = countMatch == element.Count;

            int responseIndex = IndexResponse(idQuestion);
            responseUsers[responseIndex].UserResponseMatchTheElements.Clear();
            if (element.Count > 0)
            {
                element = element.OrderBy(tb => tb.Num).ToList();
                foreach (var item in element)
                {
                    UserResponseMatchTheElement ume = new UserResponseMatchTheElement()
                    {
                        IdElement1 = item.Elem1.Id,
                        IdElement2 = item.Elem2.Id
                    };
                    responseUsers[responseIndex].UserResponseMatchTheElements.Add(ume);
                }
            }
            //проверка задания
            if (change)
            {
                bool correctly = true;
                List<UserResponseMatchTheElement> list = responseUsers[responseIndex].UserResponseMatchTheElements.ToList();
                var group1 = connection.Groups.Where(tb => tb.IdQuestion == idQuestion).Include(tb => tb.ElementOfGroups).ToList()[0];
                var group2 = connection.Groups.Where(tb => tb.IdQuestion == idQuestion).Include(tb => tb.ElementOfGroups).ToList()[1];

                for (int i = 0; i < countMatch; i++)
                {
                    var elem1 = group1.ElementOfGroups.FirstOrDefault(tb => tb.Id == list[i].IdElement1 || tb.Id == list[i].IdElement2);
                    var elem2 = group2.ElementOfGroups.FirstOrDefault(tb => tb.Id == list[i].IdElement1 || tb.Id == list[i].IdElement2);

                    if (elem1.RatioNumeri != elem2.RatioNumeri)
                        correctly = false;
                }
                responseUsers[responseIndex].Correctly = correctly;
            }
            else
                responseUsers[responseIndex].Correctly = false;
        }
        public static void SaveChangesMatchTheValue(int idQuestion, List<ItemMatchTheValue> element)
        {
            EntranceTestingContext connection = new EntranceTestingContext();
            List<ElementOfEquality> _list = connection.ElementOfEqualities.Include(tb => tb.RatioOfElementEqualityIdElement1Navigations).Include(tb => tb.RatioOfElementEqualityIdElement2Navigations).Where(tb => tb.IdQuestion == idQuestion).ToList();
            int countMatch = _list.Count() / 2;
            change = true;

            int responseIndex = IndexResponse(idQuestion);
            responseUsers[responseIndex].UserResponseMatchTheValues.Clear();
            if (element.Count > 0)
            {
                foreach (var item in element)
                {
                    if (item.Elem1 != "--" && item.Elem2 != "--")
                    {
                        var elem1 = _list.FirstOrDefault(tb => tb.Name == item.Elem1);
                        var elem2 = _list.FirstOrDefault(tb => tb.Name == item.Elem2);
                        UserResponseMatchTheValue umv = new UserResponseMatchTheValue()
                        {
                            IdElement1 = elem1.Id,
                            IdElement2 = elem2.Id
                        };
                        responseUsers[responseIndex].UserResponseMatchTheValues.Add(umv);
                    }
                    else
                    {
                        change = false;
                    }

                }
            }

            //проверка задания
            if (change)
            {
                bool correctly = true;
                List<UserResponseMatchTheValue> list = responseUsers[responseIndex].UserResponseMatchTheValues.ToList();

                for (int i = 0; i < list.Count; i++)
                {
                    var item = _list.FirstOrDefault(tb => tb.Id == list[i].IdElement1);
                    var elem1 = item.RatioOfElementEqualityIdElement1Navigations.FirstOrDefault();
                    var elem2 = item.RatioOfElementEqualityIdElement2Navigations.FirstOrDefault();
                    if (elem1 != null)
                    {
                        correctly = (elem1.IdElement1 == list[i].IdElement1 && elem1.IdElement2 == list[i].IdElement2) || (elem1.IdElement2 == list[i].IdElement1 && elem1.IdElement1 == list[i].IdElement2);
                    }
                    else if (elem2 != null)
                    {
                        correctly = (elem2.IdElement1 == list[i].IdElement1 && elem2.IdElement2 == list[i].IdElement2) || (elem2.IdElement2 == list[i].IdElement1 && elem2.IdElement1 == list[i].IdElement2);
                    }
                }
                responseUsers[responseIndex].Correctly = correctly;
            }
            else
                responseUsers[responseIndex].Correctly = false;
        }
    }
}
