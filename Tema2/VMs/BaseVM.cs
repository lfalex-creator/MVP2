using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using Tema2.Commands;
using Tema2.Models;
using Tema2.Services;

namespace Tema2.VMs
{
    public class BaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private object _currentVM;
        public object CurrentVM
        {
            get
            {
                return _currentVM;
            }
            set
            {
                if (value != _currentVM)
                {
                    _currentVM = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public BaseVM()
        {
            StartGameCommand = new IndependentExecutionCommand(StartGame);
            BackToMainScreenCommand = new IndependentExecutionCommand(BackToMainScreen);
            ObservableCollection<UserModel> users = new();
            userSerialization = new(users);
            DeserialiseUsers();
            CurrentVM = new MainScreenVM(users);
        }

        public ICommand StartGameCommand { get; set; }
        private void StartGame()
        {
            MainScreenVM temp = CurrentVM as MainScreenVM;
            CurrentVM = new GameVM(temp.SelectedListItem,temp.Users);
        }
        public ICommand BackToMainScreenCommand { get; set; }
        private void BackToMainScreen()
        {
            GameVM temp = CurrentVM as GameVM;
            CurrentVM = new MainScreenVM(new ObservableCollection<UserModel>(temp.Users));
        }


        private CollectionSerializationService<UserModel> userSerialization;

        public void SerialiseUsers()
        {
            userSerialization.Serialize();
        }
        public void DeserialiseUsers()
        {
            userSerialization.Deserialize();
        }

    }
}
