using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Tema2.Models
{
    public class CheckedMenuItemModel : INotifyPropertyChanged
    {
        private bool _checked;
        public bool IsChecked
        {
            get { return _checked; }
            set { _checked = value; OnPropertyChanged(); }
        }

        public string Category { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
