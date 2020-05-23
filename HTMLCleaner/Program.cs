using System;

namespace HTMLCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser p = new Parser(@"C:\My Web Sites\Notes", @"Source", @"Output");
            p.Parse();

        }
    }
}
