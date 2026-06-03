using Unit = System.ValueTuple;

namespace Frognar.DotFunctional;

public static partial class Basis
{
        public static T Id<T>(T value) => value;

        public static Func<T, Unit> ToFunc<T>(this Action<T> action)
                => x => { action(x); return Unit.Create(); };

        public static Func<T1, Func<T2, R>> Curry<T1, T2, R>(Func<T1, T2, R> func)
                => a => b
                        => func(a, b);

        public static Func<T1, Func<T2, T3, R>>
                CurryFirst<T1, T2, T3, R>(
                        Func<T1, T2, T3, R> func)
                => a => (b, c)
                        => func(a, b, c);

        public static Func<T1, Func<T2, T3, T4, R>>
                CurryFirst<T1, T2, T3, T4, R>
                (Func<T1, T2, T3, T4, R> func)
                => a => (b, c, d)
                        => func(a, b, c, d);

        public static Func<T1, Func<T2, T3, T4, T5, R>>
                CurryFirst<T1, T2, T3, T4, T5, R>
                (Func<T1, T2, T3, T4, T5, R> func)
                => a => (b, c, d, e)
                        => func(a, b, c, d, e);

        public static Func<T1, Func<T2, T3, T4, T5, T6, R>>
                CurryFirst<T1, T2, T3, T4, T5, T6, R>
                (Func<T1, T2, T3, T4, T5, T6, R> func)
                => a => (b, c, d, e, f)
                        => func(a, b, c, d, e, f);

        public static Func<T1, Func<T2, T3, T4, T5, T6, T7, R>>
                CurryFirst<T1, T2, T3, T4, T5, T6, T7, R>
                (Func<T1, T2, T3, T4, T5, T6, T7, R> func)
                => a => (b, c, d, e, f, g)
                        => func(a, b, c, d, e, f, g);

        public static Func<T1, Func<T2, T3, T4, T5, T6, T7, T8, R>>
                CurryFirst<T1, T2, T3, T4, T5, T6, T7, T8, R>
                (Func<T1, T2, T3, T4, T5, T6, T7, T8, R> func)
                => a => (b, c, d, e, f, g, h)
                        => func(a, b, c, d, e, f, g, h);

        public static Func<T1, Func<T2, T3, T4, T5, T6, T7, T8, T9, R>>
                CurryFirst<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
                (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> func)
                => a => (b, c, d, e, f, g, h, i)
                        => func(a, b, c, d, e, f, g, h, i);

        public static Func<T1, Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, R>>
                CurryFirst<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>
                (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> func)
                => a => (b, c, d, e, f, g, h, i, j)
                        => func(a, b, c, d, e, f, g, h, i, j);
}