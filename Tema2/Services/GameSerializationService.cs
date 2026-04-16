using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml.Serialization;
using Tema2.Models;

namespace Tema2.Services
{
    internal class GameSerializationService
    {
        private GameModel aGame;
        public GameSerializationService(GameModel theGame)
        {
            aGame = theGame;
        }
        public void Serialize()
        {
            FileStream fileStr = new FileStream($"games\\{aGame.SaveName}.xml", FileMode.Create);
            new XmlSerializer(typeof(GameModel)).Serialize(fileStr, aGame);
            fileStr.Dispose();
        }
        public void Deserialize(string target)
        {
            FileStream fileStr;
            try
            {
               fileStr = new FileStream($"games\\{target}.xml", FileMode.Open);
            }
            catch 
            {
                MessageBox.Show("Save does not exist.");
                return;
            }
            GameModel dummyGame = new XmlSerializer(typeof(GameModel)).Deserialize(fileStr) as GameModel;

            aGame.Level = dummyGame.Level;
            aGame.Word = dummyGame.Word;
            aGame.DisplayWord = dummyGame.DisplayWord;
            aGame.NameOfUser = dummyGame.NameOfUser;
            aGame.SaveName=dummyGame.SaveName;
            aGame.Lives=dummyGame.Lives;
            aGame.ActiveButtons=dummyGame.ActiveButtons;
            aGame.ClockValue=dummyGame.ClockValue;
            aGame.WordType=dummyGame.WordType;
            fileStr.Dispose();
        }

    }
}
