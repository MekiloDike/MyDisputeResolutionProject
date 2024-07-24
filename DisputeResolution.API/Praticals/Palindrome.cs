using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praticals
{
    public static class Palindrome
    {
        public static bool Palindrom(string str)
        {
            var first = 0;
            var second = str.Length-1;

            while (first < second)
            {

            if(str[first] != str[second])
            {
                return false;
            }
            first++;
            second--;
            }
            return true;
        }
        
    }
}
