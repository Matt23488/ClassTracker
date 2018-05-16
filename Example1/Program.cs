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
            //AddExample();
            TryCatchExample();
        }

        #region AddExample

        // Named for Haskell Curry
        static Func<T2, TResult> Curry<T1, T2, TResult>(Func<T1, T2, TResult> func, T1 arg1)
            => arg2 => func(arg1, arg2);

        static Func<T2, T3, TResult> Curry<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, T1 arg1)
            => (arg2, arg3) => func(arg1, arg2, arg3);

        static Func<T2, Func<T3, TResult>> FullCurry<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, T1 arg1)
            => arg2 => arg3 => func(arg1, arg2, arg3);

        #region Rediculous
        static Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, Func<T16, TResult>>>>>>>>>>>>>>> FullCurry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, T1 arg1)
            => arg2 => arg3 => arg4 => arg5 => arg6 => arg7 => arg8 => arg9 => arg10 => arg11 => arg12 => arg13 => arg14 => arg15 => arg16 => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
        #endregion

        public static Func<int, int> AddFour => Curry<int, int, int>(AddTwoNumbers, 4);

        static void AddExample()
        {
            // Using a Curry method
            {
                //var addFour = Curry<int, int, int>(AddTwoNumbers, 4);
                //Console.WriteLine($"addFour(5) = {addFour(5)}");

                //var addFive = Curry<int, int, int, int>(AddThreeNumbers, 5);
                //var addNine = Curry(addFive, 4); // Notice how Type Inferrence kicks in here
                //Console.WriteLine($"addNine(10) = {addNine(10)}");
            }

            // Using a Curry method a bit more cleverly
            {
                //var addFive = FullCurry<int, int, int, int>(AddThreeNumbers, 5);
                //var addNine = addFive(4);
                //Console.WriteLine($"addNine(10) = {addNine(10)}");
            }

            // With functions written with currying in mind
            {
                var addFour = AddTwoNumbers(4);
                Console.WriteLine($"addFour(5) = {addFour(5)}");

                var addFive = AddThreeNumbers(5);
                var addNine = addFive(4);
                Console.WriteLine($"addNine(10) = {addNine(10)}");
            }

            //////////////////////////////////////////////
            // Why would you want to do this?
            // Functional languages get this for free
        }

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
            // Abstracting away the implementation details
            //var logErrorsFrom = Catch(LogException);
            var logErrorsFrom = CatchAndRethrow(LogException);

            var result1 = 0;
            logErrorsFrom(() =>
            {
                checked
                {
                    result1 = 14 + 1;
                }
            });
            //Console.WriteLine(result1);

            //var result2 = 0;
            //logErrorsFrom(() =>
            //{
            //    checked
            //    {
            //        result2 = GetMaxInt() + 1;
            //    }
            //});
            //Console.WriteLine(result2);

            var user1 = default(User);
            logErrorsFrom(() =>
            {
                user1 = Using(() => new ExampleEntities(),
                        context => context.Users.Single(u => u.Id == 2));
            });
            Console.WriteLine(user1);

            var user2 = default(User);
            logErrorsFrom(() =>
            {
                user2 = Using(() => new ExampleEntities(),
                        context => context.Users.Single(u => u.Id == 4));
            });
            Console.WriteLine(user2);
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

        static TResult Using<TDisposable, TResult>(Func<TDisposable> getDisposable, Func<TDisposable, TResult> operate)
            where TDisposable : IDisposable
        {
            using (var disposable = getDisposable())
            {
                return operate(disposable);
            }
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
