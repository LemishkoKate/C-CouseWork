using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryApp
{
    class WordEntry
    {
        public string Word { get; set; }
        public List<string> Translations { get; set; } = new List<string>();
    }
}
