using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MercuryBuilder builder = new MercuryBuilder(@"C:\dev\Mercury\Mercury\Mercury\Core\Setup.json");
            builder.Build(@"C:\dev\Mercury\Mercury\Mercury.ConsoleApp\Sample\SampleProject.json", @"C:\dev\Mercury\Mercury\Mercury.ConsoleApp\Sample\output\");

            Console.WriteLine("Done.");
        }
    }
}
