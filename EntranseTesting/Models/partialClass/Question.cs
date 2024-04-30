using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace EntranseTesting.Models
{
    public partial class Question
    {
        [NotMapped]
        public string FillName
        {
            get
            {
                if(string.IsNullOrWhiteSpace(Name))
                    return " пустой\n";
                return Name + "\n";
            }
        }
        [NotMapped]
        public TextBlock tName
        {
            get
            {
                TextBlock tb = new TextBlock();
                tb.TextWrapping = TextWrapping.Wrap;
                tb.Inlines.Add(new Run("Вопрос: ") { FontWeight = FontWeight.Bold });
                tb.Inlines.Add(new Run(FillName));
                return tb;
            }
        }

        [NotMapped]
        public TextBlock Answer
        {
            get
            {
                TextBlock tb = new TextBlock();
                switch (IdCategory)
                {
                    case 1://Вопрос с 1 вариантом ответа
                    case 2://Вопрос с несколькими вариантами ответа
                        {
                            foreach (var item in ElementOfChooses)
                            {
                                tb.Inlines.Add(new Run("\t"));
                                if (item.Correctly)
                                    tb.Inlines.Add(new Run(" + " + item.Name + " \n") { FontWeight = FontWeight.Bold, Background = new SolidColorBrush(Colors.LightGreen) });
                                else tb.Inlines.Add(new Run("  -  " + item.Name + " \n") { FontWeight = FontWeight.Regular, Background = new SolidColorBrush(Colors.Transparent) });
                            }
                            break;
                        }
                    case 3://Горизонтальное упорядочивание элементов
                    case 4://Вертикальное упорядочивание элементов
                        {
                            tb.Inlines.Add(new Run("\tОтвет: ") { FontWeight = FontWeight.Bold });
                            tb.Inlines.Add(new Run(" ") { Background = new SolidColorBrush(Colors.LightGreen) });
                            string value = "";
                            foreach (var item in ElementOfArrangements.OrderBy(tb => tb.Position))
                            {
                                value += item.Name + "; ";
                            }
                            tb.Inlines.Add(new Run(value.Remove(value.Length - 2)) { Background = new SolidColorBrush(Colors.LightGreen) });
                            tb.Inlines.Add(new Run("\n"));
                            break;
                        }
                    case 5://Соотношение величин
                        {
                            string value = "";
                          
                            foreach (var item in ElementOfEqualities)
                            {
                                RatioOfElementEquality ratio = item.RatioOfElementEqualityIdElement1Navigations.FirstOrDefault();
                                if (ratio != null)
                                    value += "\t" + ratio.IdElement1Navigation.Name + " = " + ratio.IdElement2Navigation.Name + "\n";
                            }
                            tb.Inlines.Add(new Run(value));
                            break;
                        }
                    case 6://Соотношение элементов
                        {
                            string value = "";
                            List<Group> _list = Groups.ToList();

                            tb.Inlines.Add(new Run("\t["));
                            tb.Inlines.Add(new Run(" " + _list[0].Name + " ") { FontWeight = FontWeight.Bold, Background = new SolidColorBrush(Colors.LightBlue) });
                            tb.Inlines.Add(new Run("]\t["));
                            tb.Inlines.Add(new Run(" " + _list[1].Name + " ") { FontWeight = FontWeight.Bold, Background = new SolidColorBrush(Colors.LightCoral) });
                            tb.Inlines.Add(new Run("]\n\n"));

                            for (int i = 0; i < _list[0].ElementOfGroups.Count; i++)
                            {
                                tb.Inlines.Add(new Run(" " + (i+1) + " ") { FontWeight = FontWeight.Bold, Background = new SolidColorBrush(Colors.LightBlue) });
                                tb.Inlines.Add(new Run(" " + _list[0].ElementOfGroups.First(tb => tb.RatioNumeri == (i+1)).Name));
                                tb.Inlines.Add(new Run("\n " + (i + 1) + " ") { FontWeight = FontWeight.Bold, Background = new SolidColorBrush(Colors.LightCoral) });
                                tb.Inlines.Add(new Run(" " + _list[1].ElementOfGroups.First(tb => tb.RatioNumeri == (i + 1)).Name));
                                tb.Inlines.Add(new Run("\n\n"));
                            }                            
                            break;
                        }
                    case 7://Подстановка ответов
                        {
                            tb.Inlines.Add(new Run("\tОтвет: ") { FontWeight = FontWeight.Bold });
                            foreach(var text in TextOfPuttings)
                            {
                                tb.Inlines.Add(new Run(text.Name +" ["));
                                List<ElementOfPutting> _list = text.ElementOfPuttings.ToList();
                                for (int i =0; i < _list.Count;i++)
                                {
                                    if (_list[i].Correctly)
                                        tb.Inlines.Add(new Run(_list[i].Name) { FontWeight = FontWeight.Bold, Background = new SolidColorBrush(Colors.LightGreen) });
                                    else
                                        tb.Inlines.Add(new Run(_list[i].Name));
                                    if(i+1 < _list.Count) tb.Inlines.Add(new Run("/"));
                                }
                                tb.Inlines.Add(new Run("] "));
                            }
                            tb.Inlines.Add(new Run("\n"));
                            break;
                        }
                    default:
                        break;
                }
                return tb;
            }
        }
                
        [NotMapped]
        public int CountInResponse
        {
            get => UserResponses.Count;
        }

        [NotMapped]
        public bool IsVisibleDelete
        {
            get
            {
                if (UserResponses == null) return false;
                if (CountInResponse > 0)
                    return false;
                return true;
            }
        }

        [NotMapped]
        public int CountCorrectly
        {
            get => UserResponses.Where(tb => tb.Correctly == true).Count();
        }
        [NotMapped]
        public int UnCountCorrectly
        {
            get => UserResponses.Where(tb => tb.Correctly == false).Count();
        }

        [NotMapped]
        public TextBlock CountInTest
        {
            get
            {
                TextBlock tb = new TextBlock();
                if (UserResponses == null) return tb;

                tb.Inlines.Add(new Run("\nКол-во + ответов: ") { FontWeight = FontWeight.Bold});
                tb.Inlines.Add(new Run(CountCorrectly.ToString()));
                tb.Inlines.Add(new Run("\nКол-во - ответов: ") { FontWeight = FontWeight.Bold});
                tb.Inlines.Add(new Run(UnCountCorrectly.ToString() + "\n"));

                return tb;
            }
        }

        [NotMapped]
        public TextBlock AllCountInTest
        {
            get
            {
                TextBlock tb = new TextBlock();
                if (UserResponses == null) return tb;

                tb.Inlines.Add(new Run("\nКол-во ответов на вопрос: ") { FontWeight = FontWeight.Bold });
                tb.Inlines.Add(new Run(CountInResponse.ToString() + "\n"));

                return tb;
            }
        }
    }
}
