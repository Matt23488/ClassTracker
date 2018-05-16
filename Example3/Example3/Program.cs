using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Example3
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = new List<int>
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10
            };

            foreach (var num in Bar(numbers))
            {
                Console.WriteLine(num);
            }
        }

        // Name this method
        static IEnumerable<int> Foo(IEnumerable<int> source)
        {
            var results = new List<int>();
            foreach (int num in source)
            {
                if (num > 4)
                {
                    results.Add(num * num + 3);
                }
            }

            return results;
        }

        static IEnumerable<int> Bar(IEnumerable<int> source)
            => source.Where(num => num > 4).Select(num => num * num + 3);
    }

    static class Extensions
    {
        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item)) yield return item;
            }
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> conversion)
        {
            foreach (var item in source)
            {
                yield return conversion(item);
            }
        }
    }
}
