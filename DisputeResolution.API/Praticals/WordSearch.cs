using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praticals
{
    public class WordSearch
    {
       public static string[] FindWord(string word, string[] strings)
        {
                       
            List<string> foundWords = new List<string>();

            foreach (string s in strings)
            {
                if (s.ToLower().Contains(word.ToLower()))
                {
                    foundWords.Add(s);
                }

            }

            if (foundWords.Count == 0)
            {
                return null;
            }
            return foundWords.ToArray();

        }


    }
}
