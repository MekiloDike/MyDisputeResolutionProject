using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praticals
{
    public class vowelConsonantFinder
    {

        public static Counter GetCount(string count)
        {
            if (string.IsNullOrEmpty(count))
            {
            return new Counter { vowel = 0,consonant = 0 };
                
            }
            var consonants = 0;
            var vowels = 0;
            List<char> list = new List<char>() { 'a', 'e', 'i', 'o', 'u', };
            foreach (char c in count)
            {
                if (char.IsLetter(c))
                {
                    if (list.Contains(c))
                    {
                        vowels++;

                    }
                    else consonants++;
                }
                
            }
            return new Counter { vowel = vowels,consonant = consonants };
            
        }
        
        public class Counter
        {
            public int consonant { get; set; }
            public int vowel { get; set; }
        }
    }
}
