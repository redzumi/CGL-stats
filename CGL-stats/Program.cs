using System;

namespace CGL_stats
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = Settings.progTitle;

            Console.WindowHeight = 10;
            Console.WindowWidth = 20;

            new Worker().init();

            //Console.ReadLine();
        }
    }
}
