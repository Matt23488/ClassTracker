using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example2
{
    class Program
    {
        static void Main(string[] args)
        {
            //NonFunctional();
            Functional();
        }

        static void NonFunctional()
        {
            var strings = new List<string>
            {
                "cat", "chair", "floor", "something", "computer"
            };

            var firstEvenString = strings.Where(s => s.Length.IsEven()).FirstOrDefault();

            // Have to check for null
            if (firstEvenString != null)
            {
                Console.WriteLine($"Head is {firstEvenString}");
            }
            // Are we sure we don't need an else here?
        }

        static void Functional()
        {
            var strings = new List<string>
            {
                "cat", "chair", "floor", "something"//, "computer"
            };

            strings
                .Where(s => s.Length.IsEven())
                .Head()
                .Do(
                    head => Console.WriteLine($"Head is {head}"),
                    () => Console.WriteLine($"No head of an empty list")); // Forced "else" case for null check
        }
    }
}
