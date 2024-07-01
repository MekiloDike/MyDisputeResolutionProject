// See https://aka.ms/new-console-template for more information

int[] a = new int[5];
for (int x = 0; x < 5; x++)
{
    a[x] = x + 10;
}
foreach (int y in a)
{
    int x = y - 10;
    Console.WriteLine("Element[{0}] = {1}", x, y);
    x++;  // This increment has no effect on the program.
}

//Console.WriteLine("Hello, World!");
