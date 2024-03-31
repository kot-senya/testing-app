using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntranseTesting.Models
{
    public class ItemCAFS
    {
        string text = "";
        string selectedItem = "--";
        List<string> collectionElement = new List<string>() { "--"};

        public ItemCAFS(string _text, List<string> _collection)
        {
            Text = _text;
            CollectionElement.AddRange(_collection);
        }

        public string Text { get => text; set => text = (value == null)?"":value ; }
        public string SelectedItem { get => selectedItem; set => selectedItem = value; }
        public List<string> CollectionElement { get => collectionElement; set => collectionElement = value; }  
        public bool VisibleText
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Text))
                    return false;
                else
                    return true;
            }
        }

      
    }
}
