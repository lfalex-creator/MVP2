using System;
using System.Collections.Generic;
using System.Text;

namespace Tema2.Models
{
    public class WordModel
    {
        public string Text { get; set; }
        public WordType Type { get; set; }

        public enum WordType
        {
            Car,
            Movie,
            State,
            Mountain,
            River
        }
    }
}
