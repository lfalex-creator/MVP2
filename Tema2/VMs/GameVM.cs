using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualBasic;
using Tema2.Commands;
using Tema2.Enums;
using Tema2.Models;
using Tema2.Services;

namespace Tema2.VMs
{
    class GameVM : INotifyPropertyChanged, IDisposable
    {
        public ObservableCollection<ButtonModel> Buttons {  get; set; }
        private Dictionary<char, ButtonModel> buttonsFromLetters;

        public ObservableCollection<WordModel> Words { get; set; }
        private CollectionSerializationService<WordModel> wordSerialization;

        private string _wordType;

        public string WordType
        {
            get { return _wordType; }
            set { _wordType = value; NotifyPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string wordString;
        private char[] backingDisplayArray;
        public string DisplayString {
            get
            {
                return new string(backingDisplayArray);
            }
            set
            {
                backingDisplayArray = value.ToArray();
                NotifyPropertyChanged();
            }
        }

        private void selectRandomWord()
        {
            Random random = new Random();
            int index=random.Next(0, Words.Count);

            if(selectedCategory.ToString() != "All")
                while( (int) Words[index].Type != (int) selectedCategory -1)
                    index = random.Next(0, Words.Count);

            wordString = Words[index].Text;
            WordType = Words[index].Type.ToString();
            backingDisplayArray=new char[wordString.Length];
            for(int i = 0; i < wordString.Length; i++)
            {
                if (wordString[i] == ' ')
                    backingDisplayArray[i] = ' ';
                else
                    backingDisplayArray[i] = '_';
            }
            DisplayString= new string(backingDisplayArray);

        }
        public ICommand KeyboardRevealLetterCommand { get; set; }

        public ICommand ClickRevealLetterCommand { get; set; }
        private void KeyBoardRevealLetter(Key key)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                return;

            if (key >= Key.A  && key <= Key.Z)
            {
                char k=key.ToString()[0];
                AddLetter(k);
            }
        }

        private void ClickRevealLetter(string s)
        {
            AddLetter(s[0]);
        }

        private void AddLetter(char letter)
        {
            if (buttonsFromLetters[letter].IsActive == false)
                return;
            buttonsFromLetters[letter].IsActive = false;
            if (wordString.Contains(letter))
            {
                for (int i = 0; i < wordString.Length; i++)
                    if (letter == wordString[i])
                        backingDisplayArray[i] = letter;

                DisplayString = new string(backingDisplayArray);

                if (backingDisplayArray.Contains('_') == false)
                {
                    timer.Stop();
                    
                    
                    if (LevelNumber == 3)
                    {
                        MessageBox.Show("Congratulations, you've won!");
                        LevelNumber = 1;
                        User.NumberOfGamesWon++;
                        User.NumberOfGamesPlayed++;
                    }
                    else
                        LevelNumber++;
                    Reset();
                    timer.Start();
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                    if (Lives[i].Letter == ' ')
                    {
                        Lives[i].Letter = 'X';
                        break;
                    }
                HangmanStatus++;
                if (HangmanStatus == 6)
                {
                    timer.Stop();
                    MessageBox.Show("You've lost.");
                    timer.Start();
                    User.NumberOfGamesPlayed++;
                    LevelNumber = 1;
                    Reset();
                }
            }
        }

        private void Reset()
        {
            for (int i = 0; i < 6; i++)
                    Lives[i].Letter = ' ';
            selectRandomWord();
            foreach (var button in Buttons)
                button.IsActive = true;
            HangmanStatus = 0;
            ClockValue = 30;
        }
        private UserModel? _user;
        public UserModel? User
        {
            get { return  _user; }
            set
            {
                _user = value;
                NotifyPropertyChanged();
            }
        }
        public List<UserModel> Users { get; set; }
        private int _levelNumber;
        public int LevelNumber
        {
            get
            {
                return _levelNumber;
            }
            set
            {
                _levelNumber = value;
                NotifyPropertyChanged();
            }
        }
        private TimerService timer;
        public GameVM(UserModel? selectedUser, ObservableCollection<UserModel> Users)
        {
            Buttons = new ObservableCollection<ButtonModel>();
            Words = new ObservableCollection<WordModel>();
            Lives = new ObservableCollection<LetterModel>();
            CheckedMenuItems = new ObservableCollection<CheckedMenuItemModel>();

            User = selectedUser;
            this.Users = Users.ToList();

            KeyboardRevealLetterCommand = new RelayCommand<Key>(KeyBoardRevealLetter);
            ClickRevealLetterCommand = new RelayCommand<string>(ClickRevealLetter);
            SelectNewCategoryCommand = new RelayCommand<string>(SelectNewCategory);
            NewGameCommand = new IndependentExecutionCommand(Reset);
            SaveGameCommand = new IndependentExecutionCommand(SaveCurrentGame);
            OpenGameCommand = new IndependentExecutionCommand(OpenGame);
            ShowStatisticsCommand = new IndependentExecutionCommand(ShowStatistics);
            AboutCommand = new IndependentExecutionCommand(About);

            buttonsFromLetters = new();

            timer = new TimerService();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += UpdateValue;
            ClockValue = 30;

            wordSerialization = new(Words);
            wordSerialization.Deserialize();

            for(char c = 'A'; c <='Z';c++)
            {
                ButtonModel model = new ButtonModel() { Letter=new string($"{c}"), IsActive=true };
                Buttons.Add(model);
                buttonsFromLetters.Add(c, model);
            }

            for (int i = 0; i < 6; i++)
                Lives.Add(new LetterModel() { Letter = ' ' });

            for (int i = 0; i < 6; i++)
                CheckedMenuItems.Add(new CheckedMenuItemModel() { IsChecked= i==0, Category=((CategoryEnum)i).ToString() });

            HangmanStatus = 0;
            LevelNumber = 1;

            selectRandomWord();

            CurrentGame = new GameModel();
            gameSerializationService = new(CurrentGame);

            timer.Start();
        }
        
        private int _clockValue;

        public int ClockValue
        {
            get { return _clockValue; }

            set
            {
                _clockValue = value;
                NotifyPropertyChanged();
            }
        }

        private void UpdateValue(object sender, EventArgs e)
        {
            ClockValue--;
            if (ClockValue == -1)
                Reset();
        }

        public ObservableCollection<LetterModel> Lives { get; set; }
        private int _hangmanStatus;
        private int HangmanStatus { 
            get { return _hangmanStatus; }
            set { _hangmanStatus = value; NotifyPropertyChanged("HangmanPath"); }
        }
        public string HangmanPath { get { return $"pack://application:,,,/imgs/hangman/{HangmanStatus}.png"; } }


        public ObservableCollection<CheckedMenuItemModel> CheckedMenuItems { get; set; }
        public ICommand SelectNewCategoryCommand { get; set; }

        private CategoryEnum selectedCategory;

        public void SelectNewCategory(string categoryLabel)
        {
            CategoryEnum category = (CategoryEnum) Enum.Parse(typeof(CategoryEnum),categoryLabel);

            foreach(var item  in CheckedMenuItems)
                item.IsChecked = false;
            CheckedMenuItems[(int)category].IsChecked = true;
            selectedCategory = category;

            Reset();

        }
        private GameSerializationService gameSerializationService;
        public GameModel CurrentGame { get; set; }
        public ICommand NewGameCommand { get; set; }

        public ICommand SaveGameCommand { get; set; }

        public void SaveCurrentGame()
        {
            timer.Stop();
            CurrentGame.Level = LevelNumber;
            CurrentGame.Word = wordString;
            CurrentGame.DisplayWord = DisplayString;
            CurrentGame.NameOfUser = User.Name;
            CurrentGame.ClockValue = ClockValue;
            CurrentGame.Lives = new bool[6];
            CurrentGame.WordType = WordType;
            for (int i = 0; i < 6; i++)
                CurrentGame.Lives[i] = Lives[i].Letter == 'X' ? false : true;
            CurrentGame.ActiveButtons= new bool[26];
            for(int i=0;i<26;i++)
                CurrentGame.ActiveButtons[i]= Buttons[i].IsActive;
            
            string input = Interaction.InputBox("Enter save name:", "Input Dialog");
            CurrentGame.SaveName=input;
            gameSerializationService.Serialize();

            timer.Start();
        }

        public ICommand OpenGameCommand { get; set; }
        public void OpenGame()
        {
            timer.Stop();

            CurrentGame.NameOfUser = User.Name;

            string input = Interaction.InputBox("Enter save name:", "Input Dialog");
            gameSerializationService.Deserialize(input);

            if(CurrentGame.NameOfUser!= User.Name)
            {
                MessageBox.Show("Cannot open saved game, user does not have access");
                CurrentGame.Level = LevelNumber;
                CurrentGame.NameOfUser = User.Name;
                CurrentGame.Word = wordString;
                CurrentGame.DisplayWord = DisplayString;
                CurrentGame.ClockValue = ClockValue;
                CurrentGame.WordType = WordType;
                CurrentGame.Lives = new bool[6];
                for (int i = 0; i < 6; i++)
                    CurrentGame.Lives[i] = Lives[i].Letter == 'X' ? false : true;
                CurrentGame.ActiveButtons = new bool[26];
                for (int i = 0; i < 26; i++)
                    CurrentGame.ActiveButtons[i] = Buttons[i].IsActive;
            }
            else
            {
                LevelNumber = CurrentGame.Level;
                wordString = CurrentGame.Word;
                DisplayString = CurrentGame.DisplayWord;
                ClockValue = CurrentGame.ClockValue;
                WordType= CurrentGame.WordType;
                int temp = 0;
                for (int i = 0; i < 6; i++)
                { 
                    Lives[i].Letter = CurrentGame.Lives[i] == false ? 'X' : ' ';
                    if (CurrentGame.Lives[i] == false)
                        temp++;
                }
                HangmanStatus = temp;

                for (int i = 0; i < 26; i++)
                    Buttons[i].IsActive = CurrentGame.ActiveButtons[i];
            }

            timer.Start();
        }
        public ICommand ShowStatisticsCommand { get; set; }

        public void ShowStatistics()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var user in Users)
                sb.Append($"{user.Name}: {user.NumberOfGamesPlayed} games played, {user.NumberOfGamesWon} won\n");
            MessageBox.Show(sb.ToString());
        }

        public ICommand AboutCommand { get; set; }
        private void About()
        {
            MessageBox.Show("Lupu Alexandru-Florin\nGrupa 2 (10LF342)\nInformatica Aplicata");
        }

        public void Dispose()
        {
            timer.Stop();
            timer.Tick -= UpdateValue;
            timer = null;
        }
    }
}
