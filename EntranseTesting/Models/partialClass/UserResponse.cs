using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Media;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntranseTesting.Models
{
    public partial class UserResponse
    {
        [NotMapped]
        public TextBlock Result
        {
            get
            {
                bool flag = UserResponseChooseAnswers.Count == 0 && UserResponseArrangements.Count == 0 && UserResponseMatchTheElements.Count == 0 && UserResponseMatchTheValues.Count == 0 && UserResponseMultiplyAnswers.Count == 0;
                TextBlock tb = new TextBlock();

                if (flag && HintApply) tb.Inlines.Add(new Run("(Пропущен/Применена подсказка)\n"));
                else if (flag) tb.Inlines.Add(new Run("(Пропущен)\n"));
                else if (HintApply) tb.Inlines.Add(new Run("(Применена подсказка)\n"));

                tb.Inlines.Add(new Run(" Вопрос: ") { FontWeight = FontWeight.Bold, Background = (Correctly) ? new SolidColorBrush(Colors.LightGreen) : new SolidColorBrush(Colors.LightCoral) });//вопрос
                tb.Inlines.Add(new Run(IdQuestionNavigation.Name + "\n"));

                SolidColorBrush color = (Correctly) ? new SolidColorBrush(Colors.LightGreen) : new SolidColorBrush(Colors.Yellow);
                /* 
                 Colors.LightGray - правильно, но не выбрали
                 Colors.LightGreen - правильно, выбрали
                 Colors.Yellow - частично правильно, выбрали
                 Colors.LightSalmon - не правильно, но выбрали
                 Colors.Transparent - не правильно, не выбрали
                 */
                switch (IdQuestionNavigation.IdCategory)
                {
                    case 1://Вопрос с 1 вариантом ответа
                    case 2://Вопрос с несколькими вариантами ответа
                        {
                            List<ElementOfChoose> _listEl = IdQuestionNavigation.ElementOfChooses.OrderBy(tb => tb.Id).ToList();
                            if (!flag)
                            {
                                List<UserResponseChooseAnswer> _listRes = UserResponseChooseAnswers.OrderBy(tb => tb.IdElement).ToList();
                                for (int i = 0; i < _listEl.Count(); i++)
                                {
                                    if (_listEl[i].Correctly)
                                    {
                                        SolidColorBrush _color = (_listRes[i].Usercorrectly == true) ? color : new SolidColorBrush(Colors.LightGray);
                                        tb.Inlines.Add(new Run("\t* " + _listEl[i].Name + " \n") { Background = _color });
                                    }
                                    else
                                    {
                                        SolidColorBrush _color = (_listRes[i].Usercorrectly == true) ? new SolidColorBrush(Colors.LightSalmon) : new SolidColorBrush(Colors.Transparent);
                                        tb.Inlines.Add(new Run("\t* " + _listEl[i].Name + " \n") { Background = _color });
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < _listEl.Count(); i++)
                                {
                                    SolidColorBrush _color = (_listEl[i].Correctly == true) ? new SolidColorBrush(Colors.LightGray) : new SolidColorBrush(Colors.Transparent);
                                    tb.Inlines.Add(new Run("\t* " + _listEl[i].Name + " \n") { Background = _color });
                                }
                            }

                            break;
                        }
                    case 3://Горизонтальное упорядочивание элементов
                    case 4://Вертикальное упорядочивание элементов
                        {
                            List<ElementOfArrangement> _listEl = IdQuestionNavigation.ElementOfArrangements.OrderBy(tb => tb.Position).ToList();
                            if (!flag)
                            {
                                tb.Inlines.Add(new Run("\tОтвет пользователя: ") { FontWeight = FontWeight.Bold });
                                List<UserResponseArrangement> _listRes = UserResponseArrangements.OrderBy(tb => tb.Position).ToList();

                                for (int i = 0; i < _listEl.Count(); i++)
                                {
                                    var item = _listEl.FirstOrDefault(tb => tb.Id == _listRes[i].IdElement);
                                    string line = (i < _listEl.Count() - 1) ? item.Name + ", " : item.Name + " ";
                                    if (_listEl[i].Id == _listRes[i].IdElement)
                                        tb.Inlines.Add(new Run(line) { Background = color });
                                    else
                                        tb.Inlines.Add(new Run(line) { Background = new SolidColorBrush(Colors.LightSalmon) });
                                }
                                tb.Inlines.Add(new Run("\n"));
                            }
                            tb.Inlines.Add(new Run("\tПравильный ответ: ") { FontWeight = FontWeight.Bold });
                            int j = 0;
                            foreach (var item in _listEl)
                            {
                                string line = (j < _listEl.Count() - 1) ? item.Name + ", " : item.Name + " ";
                                tb.Inlines.Add(new Run(line));
                                j++;
                            }
                            tb.Inlines.Add(new Run("\n"));
                            break;
                        }
                    case 5://Соотношение величин
                        {
                            if (!flag)
                            {
                                tb.Inlines.Add(new Run("\tОтвет пользователя: \n") { FontWeight = FontWeight.Bold });
                                List<ElementOfEquality> _list = IdQuestionNavigation.ElementOfEqualities.Where(tb => tb.RatioOfElementEqualityIdElement1Navigations != null).ToList();
                                foreach (var item in UserResponseMatchTheValues)
                                {
                                    ElementOfEquality elem = _list.Where(tb => tb.Id == item.IdElement1 || tb.Id == item.IdElement2).ToList()[0];
                                    ElementOfEquality elemD = _list.Where(tb => tb.Id == item.IdElement1 || tb.Id == item.IdElement2).ToList()[1];
                                    int index = _list.IndexOf(elem);
                                    RatioOfElementEquality ratio = _list[index].RatioOfElementEqualityIdElement1Navigations.FirstOrDefault();
                                    if(ratio == null)
                                        ratio = _list[index].RatioOfElementEqualityIdElement2Navigations.FirstOrDefault();                                    
                                    bool correct = ratio.IdElement1 == item.IdElement1 && ratio.IdElement2 == item.IdElement2 || ratio.IdElement2 == item.IdElement1 && ratio.IdElement1 == item.IdElement2;
                                    SolidColorBrush _color = (correct) ? color : new SolidColorBrush(Colors.LightSalmon);
                                    tb.Inlines.Add(new Run("\t" + elem.Name + " = " + elemD.Name + "\n") { Background = _color });
                                }
                                tb.Inlines.Add(new Run("\n"));
                            }
                            tb.Inlines.Add(new Run("\tПравильный ответ: \n") { FontWeight = FontWeight.Bold});
                            string value = "";
                            foreach (var item in IdQuestionNavigation.ElementOfEqualities)
                            {
                                RatioOfElementEquality ratio = item.RatioOfElementEqualityIdElement1Navigations.FirstOrDefault();
                                if (ratio != null)
                                    value += "\t" + ratio.IdElement1Navigation.Name + " = " + ratio.IdElement2Navigation.Name + "\n";
                            }
                            tb.Inlines.Add(new Run(value + "\n"));
                            break;
                        }
                    case 6://Соотношение элементов
                        {                           
                            List<Group> _list = IdQuestionNavigation.Groups.ToList();
                            tb.Inlines.Add(new Run("\t["));
                            tb.Inlines.Add(new Run(" " + _list[0].Name + " ") { FontWeight = FontWeight.Bold, Background = new SolidColorBrush(Colors.LightBlue) });
                            tb.Inlines.Add(new Run("]\t["));
                            tb.Inlines.Add(new Run(" " + _list[1].Name + " ") { FontWeight = FontWeight.Bold, Background = new SolidColorBrush(Colors.LightCoral) });
                            tb.Inlines.Add(new Run("]\n\n"));
                            if (!flag)
                            {
                                tb.Inlines.Add(new Run("\tОтвет пользователя: \n") { FontWeight = FontWeight.Bold });
                                foreach (var item in UserResponseMatchTheElements)
                                {
                                    ElementOfGroup elem1 = _list[0].ElementOfGroups.First(tb => tb.Id == item.IdElement1);
                                    ElementOfGroup elem2 = _list[1].ElementOfGroups.First(tb => tb.Id == item.IdElement2);
                                    bool correct = elem2.RatioNumeri == elem1.RatioNumeri;
                                    SolidColorBrush _color = (correct) ? color : new SolidColorBrush(Colors.LightSalmon);
                                    tb.Inlines.Add(new Run("   ") { Background = new SolidColorBrush(Colors.LightBlue) });
                                    tb.Inlines.Add(new Run(" " + elem1.Name) { Background = _color });
                                    tb.Inlines.Add(new Run("\n   ") { Background = new SolidColorBrush(Colors.LightCoral) });
                                    tb.Inlines.Add(new Run(" " + elem2.Name) { Background = _color });
                                    tb.Inlines.Add(new Run("\n\n"));
                                }
                            }

                            tb.Inlines.Add(new Run("\tПравильный ответ: \n") { FontWeight = FontWeight.Bold });
                            for (int i = 0; i < _list[0].ElementOfGroups.Count; i++)
                            {
                                tb.Inlines.Add(new Run("   ") { Background = new SolidColorBrush(Colors.LightBlue) });
                                tb.Inlines.Add(new Run(" " + _list[0].ElementOfGroups.First(tb => tb.RatioNumeri == (i + 1)).Name));
                                tb.Inlines.Add(new Run("\n   ") { Background = new SolidColorBrush(Colors.LightCoral) });
                                tb.Inlines.Add(new Run(" " + _list[1].ElementOfGroups.First(tb => tb.RatioNumeri == (i + 1)).Name));
                                tb.Inlines.Add(new Run("\n\n"));
                            }
                            break;
                        }
                    case 7://Подстановка ответов
                        {
                            if (!flag)
                            {
                                tb.Inlines.Add(new Run("\tОтвет пользователя: ") { FontWeight = FontWeight.Bold });
                                List<UserResponseMultiplyAnswer> list = UserResponseMultiplyAnswers.ToList();
                                foreach (var text in IdQuestionNavigation.TextOfPuttings)
                                {
                                    if (text.Name != null && text.Name != "") tb.Inlines.Add(new Run(text.Name + " "));
                                    UserResponseMultiplyAnswer elem = list.FirstOrDefault(tb => tb.IdText == text.Id);

                                    if(elem != null)
                                    {
                                        ElementOfPutting putting = text.ElementOfPuttings.FirstOrDefault(tb => tb.Id == elem.IdElement);
                                        SolidColorBrush _color = (putting.Correctly) ? color : new SolidColorBrush(Colors.LightSalmon);
                                        tb.Inlines.Add(new Run(putting.Name + " ") { Background = _color});
                                    }
                                    else
                                    {
                                        tb.Inlines.Add(new Run(" [Пропущено] ") { FontWeight = FontWeight.Bold, Background = new SolidColorBrush(Colors.LightCoral) });
                                    }                                    
                                }
                                tb.Inlines.Add(new Run("\n"));
                            }
                            tb.Inlines.Add(new Run("\tПравильный ответ: ") { FontWeight = FontWeight.Bold });
                            foreach (var text in IdQuestionNavigation.TextOfPuttings)
                            {
                                if (text.Name != null && text.Name != "") tb.Inlines.Add(new Run(text.Name + " "));
                                List<ElementOfPutting> _list = text.ElementOfPuttings.Where(tb=>tb.Correctly == true).ToList();
                                if(_list.Count > 1) tb.Inlines.Add(new Run(" ["));
                                for (int i = 0; i < _list.Count; i++)
                                {
                                    if (_list[i].Correctly)
                                    {
                                        tb.Inlines.Add(new Run(_list[i].Name + " ") { FontWeight = FontWeight.Bold});
                                        if(i  < _list.Count - 1) tb.Inlines.Add(new Run("/"));

                                    }                                        
                                }
                                if (_list.Count > 1) tb.Inlines.Add(new Run("] "));
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
    }
}

