
using Praticals;

//string word = "Test";
//string[] strings = {"This is a test", "Another string", "tkesting string", "No match here" };
//var result = WordSearch.FindWord(word, strings);


//foreach (string str in result)
//{
//    Console.WriteLine(str);
//}

var result = vowelConsonantFinder.GetCount(",,skodiaioama");
Console.WriteLine($"consonant = {result.consonant}, vowel = { result.vowel}");


 string Reversed(string str)
{
    var charr = str.ToCharArray();
     Array.Reverse(charr);
    return new string(charr);
}


int Max(int[] ints)
{
        int max = ints[0];

    foreach (int item in ints)
    {
        if(item > max)
        {
            max = item;
        }
    }
    return max;
}

int[] arr = [4, 3, 6, 4, 9, 7, 2];
var res = Max(arr);
    Console.WriteLine(res);

var pal = Palindrome.Palindrom("redeoo");
Console.WriteLine(pal);

var resultt = Algorithm.Arithmetic(123, 3);
Console.WriteLine(resultt);


