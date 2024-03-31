using Avalonia;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntranseTesting.Models
{
    public class ItemLine
    {
        Point? startPoint = new Point(0,0);
        Point? endPoint = new Point(0, 0);
        Border? border;        
        ElementOfGroup? elem1 = null;
        ElementOfGroup? elem2 = null;

        public Point? StartPoint { get => startPoint; set => startPoint = value; }
        public Point? EndPoint { get => endPoint; set => endPoint = value; }
        public ElementOfGroup? Elem1 { get => elem1; set => elem1 = value; }
        public ElementOfGroup? Elem2 { get => elem2; set => elem2 = value; }
        public Border? Border { get => border; set => border = value; }
    }
}
