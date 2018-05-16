using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Example1
{
    static class Program
    {

        static void Main(string[] pArgs)
        {
            AddExample();
            //TryCatchExample();
        }

        #region AddExample

        static void AddExample()
        {
            // Using a Curry method
            {
                var addFour = Curry<int, int, int>(AddTwoNumbers, 4);
                Console.WriteLine($"addFour(5) = {addFour(5)}");

                var addFive = Curry<int, int, int, int>(AddThreeNumbers, 5);
                var addNine = Curry(addFive, 4); // Notice how Type Inferrence kicks in here
                Console.WriteLine($"addNine(10) = {addNine(10)}");
            }

            // Using a Curry method a bit more cleverly
            {
                //var addFive = FullCurry<int, int, int, int>(AddThreeNumbers, 5);
                //var addNine = addFive(4);
                //Console.WriteLine($"addNine(10) = {addNine(10)}");
            }

            // With functions written with currying in mind
            {
                //var addFour = AddTwoNumbers(4);
                //Console.WriteLine($"addFour(5) = {addFour(5)}");

                //var addFive = AddThreeNumbers(5);
                //var addNine = addFive(4);
                //Console.WriteLine($"addNine(10) = {addNine(10)}");
            }

            //////////////////////////////////////////////
            // Why would you want to do this?
            // Functional languages get this for free
        }

        static Func<T2, TResult> Curry<T1, T2, TResult>(Func<T1, T2, TResult> func, T1 arg1)
            => arg2 => func(arg1, arg2);
        static Func<T2, T3, TResult> Curry<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, T1 arg1)
            => (arg2, arg3) => func(arg1, arg2, arg3);
        static Func<T2, Func<T3, TResult>> FullCurry<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, T1 arg1)
            => arg2 => arg3 => func(arg1, arg2, arg3);

        static int AddTwoNumbers(int a, int b)
            => a + b;
        static int AddThreeNumbers(int a, int b, int c)
            => a + b + c;
        static Func<int, int> AddTwoNumbers(int a)
            => b => a + b;
        static Func<int, Func<int, int>> AddThreeNumbers(int a)
            => b => c => a + b + c;


        #endregion

        #region TryCatchExample

        private static ILogger _logger = new ConsoleLogger();
        static void TryCatchExample()
        {
            var logErrorsFrom = Catch(LogException);
            //var logErrorsFrom = CatchAndRethrow(LogException);

            var result = 0;
            logErrorsFrom(() =>
            {
                checked
                {
                    result = 14 + 1;
                }
            });
            Console.WriteLine(result);

            var numbers = new int[3];
            logErrorsFrom(() =>
            {
                checked
                {
                    result = GetMaxInt() + 1;
                }
            });
            Console.WriteLine(result);
        }

        static Action<Action> Catch(Action<Exception> catchClause)
        {
            return tryClause =>
            {
                try
                {
                    tryClause();
                }
                catch (Exception ex)
                {
                    catchClause(ex);
                }
            };
        }

        static Action<Action> CatchAndRethrow(Action<Exception> catchClause)
        {
            return tryClause =>
            {
                try { tryClause(); }
                catch (Exception ex)
                {
                    catchClause(ex);
                    throw;
                }
            };
        }

        static void LogException(Exception ex)
        {
            _logger.LogMessage(ex.Message);
        }

        public static int GetMaxInt()
            => int.MaxValue;

        #endregion
    }
}
