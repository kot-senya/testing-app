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
        }
        public static void SaveChangesChoosing(int idQuestion, ObservableCollection<ElementOfChoose> element)
        {
            int responseIndex = IndexResponse(idQuestion);
            List<UserResponseChooseAnswer> list = responseUsers[responseIndex].UserResponseChooseAnswers.ToList();
            responseUsers[responseIndex].UserResponseChooseAnswers.Clear();
            if (list.Count > 0)
            {
                foreach (var item in element)
                {
                    int index = list.IndexOf(list.FirstOrDefault(tb => tb.IdElement == item.Id));
                    list[index].Usercorrectly = (item.UserCorrectly == null)? false : item.UserCorrectly;
                }
                foreach (var item in list)
                    responseUsers[responseIndex].UserResponseChooseAnswers.Add(item);
            }

        }
        public static void SaveChangesCAFS(int idQuestion, ObservableCollection<ItemCAFS> element)
        {
            /* EntranceTestingContext connection = new EntranceTestingContext();
             int responseIndex = IndexResponse(idQuestion);
             List<UserResponseMultiplyAnswer> list = responseUsers[responseIndex].UserResponseMultiplyAnswes.ToList();
             responseUsers[responseIndex].UserResponseMultiplyAnswes.Clear();

             foreach (var item in element)
             {
                 if(item.SelectedItem != "--")
                 {
                     TextOfPutting t = connection.TextOfPuttings.FirstOrDefault(tb => tb.Name == item.Text && tb.IdQuestion == idQuestion);
                     ElementOfPutting e = connection.ElementOfPuttings.FirstOrDefault(tb => tb.Name == item.SelectedItem && tb.IdText == t.Id);
                     responseUsers[responseIndex].UserResponseMultiplyAnswes.Add(new UserResponseMultiplyAnswer() { IdElement = e.Id, IdText = t.Id});
                 }                
             }*/
        }
    }
}
