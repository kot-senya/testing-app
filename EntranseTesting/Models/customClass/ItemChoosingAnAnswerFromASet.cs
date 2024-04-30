using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntranseTesting.Models
{
    public partial class ItemCAFS: INotifyPropertyChanged
    {
        string text = "";        
        string selectedItem = "--";
        List<string> collectionElement = new List<string>() { "--"};
        TextOfPutting tP = new TextOfPutting();
        ObservableCollection<ElementOfPutting> elementEditor = new ObservableCollection<ElementOfPutting>();

        public ItemCAFS() { }
        public ItemCAFS(string _text, List<string> _collection)
        {
            Text = _text;
            CollectionElement.AddRange(_collection);
        }
        public ItemCAFS(TextOfPutting _text, List<ElementOfPutting> _collection)
        {
            Text = _text.Name;
            TP = _text;
            foreach(var item in _collection)
                ElementEditor.Add(item);
        }
        public string Text { get => text; set => text = (value == null)?"":value ; }
        public string SelectedItem { get => selectedItem; set => selectedItem = value; }
        public List<string> CollectionElement { get => collectionElement; set => collectionElement = value; }  
        public ObservableCollection<ElementOfPutting> ElementEditor { get => elementEditor; set { elementEditor = value; OnPropertyChanged("ElementEditor"); } }
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

        public TextOfPutting TP { get => tP; set => tP = value; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
