using System;
using System.Collections.Generic;
using System.Text;

namespace Tema2.Models
{
    public class GameModel
    {
        public string NameOfUser { get; set; }
        public string SaveName { get; set; }
        public int Level { get; set; }
        public string Word  { get; set; }
        public string DisplayWord { get; set; }
        public int ClockValue { get; set; }

        public bool[] Lives { get; set; }

        public bool[] ActiveButtons { get; set; }
    }
}
