namespace Frognar.DotFunctional;

public static partial class Basis
{
        public static T Id<T>(T value) => value;

        public static Func<T1, Func<T2, R>> Curry<T1, T2, R>(Func<T1, T2, R> func) => x => y => func(x, y);
}