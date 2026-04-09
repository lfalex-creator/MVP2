using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tema2.VMs
{
    public abstract class BaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Page _currentView;
        public Page CurrentView
        {
            get
            {
                return _currentView;
            }
            set
            {
                if (value != _currentView)
                {
                    _currentView = value;
                    NotifyPropertyChanged();
                }
            }
        }

        protected abstract void SwitchView(Type classType);
        public ICommand SwitchViewsCommand { get; set; }
    }
}
