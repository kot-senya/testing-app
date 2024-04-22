using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using EntranseTesting.Models;
using EntranseTesting.Models.customClass;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace EntranseTesting.ViewModels
{
	public partial class TestMatchTheValueViewModel : ObservableObject
	{
        EntranceTestingContext baseConnection = new EntranceTestingContext();
        [ObservableProperty] int numberTask;
        [ObservableProperty] string question;
        [ObservableProperty] List<ItemMatchTheValue> elements = new List<ItemMatchTheValue>();
        [ObservableProperty] List<ElementOfEquality> info = new List<ElementOfEquality>();
        [ObservableProperty] List<QuestionImage> qImage = new List<QuestionImage>();

        public TestMatchTheValueViewModel(int numberTask)
        {
            this.numberTask = numberTask;

            Question = baseConnection.Questions.FirstOrDefault(tb => tb.Id == numberTask).Name;
            QImage = baseConnection.QuestionImages.Where(tb => tb.IdQuestion == numberTask).ToList();

            Info = baseConnection.ElementOfEqualities.Include(tb => tb.RatioOfElementEqualityIdElement1Navigations).Include(tb => tb.RatioOfElementEqualityIdElement2Navigations).ToList();

            for (int i = 0; i < Info.Count/2; i++)
                Elements.Add(new ItemMatchTheValue(Info.Select(tb => tb.Name).ToList()));

            //заполняем данные
            int responseIndex = Response.IndexResponse(numberTask);
            if (Response.responseUsers[responseIndex].UserResponseMatchTheValues.Count > 0)//если пользователь не отвечал
            {
                List<UserResponseMatchTheValue> _response = Response.responseUsers[responseIndex].UserResponseMatchTheValues.ToList();
                int i = 0;
                foreach (UserResponseMatchTheValue item in _response)
                {
                    Elements[i].Elem1 = Info.FirstOrDefault(tb => tb.Id == item.IdElement1).Name;
                    Elements[i].Elem2 = Info.FirstOrDefault(tb => tb.Id == item.IdElement2).Name;
                    i++;
                }
            }
        }
    }
}