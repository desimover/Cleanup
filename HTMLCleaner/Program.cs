using System;

namespace HTMLCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser p = new Parser(@"C:\My Web Sites\PianoNotes", "www.pianomint.com", @"Output");
            p.Parse();

        }
    }
}
