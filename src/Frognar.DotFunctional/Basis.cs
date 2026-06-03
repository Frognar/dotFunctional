using Unit = System.ValueTuple;

namespace Frognar.DotFunctional;

public static partial class Basis
{
        public static T Id<T>(T value) => value;

        public static Func<T, Unit> ToFunc<T>(this Action<T> action)
                => x => { action(x); return Unit.Create(); };

        public static Func<T1, Func<T2, R>> Curry<T1, T2, R>(Func<T1, T2, R> func)
                => a => b => func(a, b);

        public static Func<T1, Func<T2, T3, R>> CurryFirst<T1, T2, T3, R>(Func<T1, T2, T3, R> func)
                => a => (b, c) => func(a, b, c);

        public static Func<T1, Func<T2, T3, T4, R>> CurryFirst<T1, T2, T3, T4, R>(Func<T1, T2, T3, T4, R> func)
                => a => (b, c, d) => func(a, b, c, d);
}