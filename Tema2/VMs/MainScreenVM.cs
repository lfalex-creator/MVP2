using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using Tema2.Commands;
using Tema2.Models;
using Tema2.Services;
using Tema2.Views;

namespace Tema2.VMs
{
    internal class MainScreenVM : INotifyPropertyChanged
    {

        public ICommand NextImageCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void NextImage()
        {
            int imageNumber = int.Parse(SelectedListItem.ImagePath.Split('/').Last().Split(".")[0]);
            imageNumber++;
            if (imageNumber == 5)
                imageNumber = 1;
            SelectedListItem.ImagePath = $"pack://application:,,,/imgs/{imageNumber}.jpg";
           //NotifyPropertyChanged("LocalImage");
        }

        public ICommand PreviousImageCommand { get; set; }

        private void PreviousImage()
        {
            int imageNumber = int.Parse(SelectedListItem.ImagePath.Split('/').Last().Split(".")[0]);
            imageNumber--;
            if (imageNumber == 0)
                imageNumber = 4;
            SelectedListItem.ImagePath = $"pack://application:,,,/imgs/{imageNumber}.jpg";
            //NotifyPropertyChanged("LocalImage");
        }
        public ObservableCollection<UserModel> Users { get; set; }

        public ICommand AddUserCommand { get; set; }
        public void AddUser()
        {
            string result=Interaction.InputBox("Insert username","New user");
            if(result!=null)
            {
                foreach(var user in Users)
                    if(user.Name==result)
                    {
                        MessageBox.Show("Username already exists!");
                        return;
                    }
                Users.Add(new UserModel() {Name= result, ImagePath= "pack://application:,,,/imgs/1.jpg" });
            }

        }

        public ICommand RemoveUserCommand { get; set; }

        private UserModel? _selectedListItem;
        public UserModel? SelectedListItem 
        { 
            get
            {
                return _selectedListItem;
            }
            set
            {
                _selectedListItem = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("AUserSelected");
                NotifyPropertyChanged("LocalImage");
            }
        }
        public bool AUserSelected
        {
            get
            {
                return SelectedListItem != null;
            }
        }

        public bool LocalImage
        {
            get
            { 
                if(SelectedListItem != null)
                {
                    return SelectedListItem.ImagePath.StartsWith("pack://application:,,,/imgs/");
                }
                return false;
            }
        }
        public void RemoveUser()
        {
            GameModel game = new GameModel();
            GameSerializationService gss = new GameSerializationService(game);
            string[] files = Directory.GetFiles("games");
            foreach (string file in files)
            {
                gss.Deserialize(file.Split("\\").Last().Split(".")[0]);
                if (game.NameOfUser == SelectedListItem.Name)
                    File.Delete(file);

            }

            Users.Remove(SelectedListItem);
            SelectedListItem = null;
        }
        public MainScreenVM(ObservableCollection<UserModel> users)
        {
            Users= users;
            SelectedListItem= null;

            //imageNumer = 1;
            //ImagePath = "pack://application:,,,/imgs/1.jpg";

            NextImageCommand = new IndependentExecutionCommand(NextImage);
            PreviousImageCommand=new IndependentExecutionCommand(PreviousImage);
            AddUserCommand = new IndependentExecutionCommand(AddUser);
            RemoveUserCommand = new IndependentExecutionCommand(RemoveUser);
            ChooseImageCommand = new IndependentExecutionCommand(ChangeImage);
        }

        public ICommand ChooseImageCommand { get; set;  }
        private void ChangeImage()
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            string result = dialog.FileName.Replace("\\","/");
            SelectedListItem.ImagePath = result;
            //NotifyPropertyChanged("LocalImage");
        }
    }
}
