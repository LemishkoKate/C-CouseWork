using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryApp
{
    class Dictionary
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<WordEntry> Entries { get; set; } = new List<WordEntry>();
    }
}
