using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example2
{
    public interface IMaybe<T>
    {
        IMaybe<U> Map<U>(Func<T, U> func);
        IMaybe<T> Filter(Func<T, bool> predicate);
        IMaybe<T> Do(Action<T> just, Action nothing);
    }

    class Something<T> : IMaybe<T>
    {
        readonly T _obj;
        public Something(T obj)
        {
            _obj = obj;
        }

        public IMaybe<U> Map<U>(Func<T, U> func)
            => new Something<U>(func(_obj));

        public IMaybe<T> Filter(Func<T, bool> predicate)
            => predicate(_obj)
                ? (IMaybe<T>)this
                : new Nothing<T>();

        public IMaybe<T> Do(Action<T> action, Action nothing)
        {
            action(_obj);
            return this;
        }
    }

    class Nothing<T> : IMaybe<T>
    {

        public IMaybe<U> Map<U>(Func<T, U> func)
            => new Nothing<U>();

        public IMaybe<T> Filter(Func<T, bool> predicate)
            => this;

        public IMaybe<T> Do(Action<T> action, Action nothing)
        {
            nothing();
            return this;
        }
    }

    public static class Extensions
    {
        public static IMaybe<T> Head<T>(this IEnumerable<T> list)
        {
            var result = list.FirstOrDefault();
            if (result == null) return new Nothing<T>();
            return new Something<T>(result);
        }

        public static bool IsEven(this int num)
            => num % 2 == 0;
    }
}
