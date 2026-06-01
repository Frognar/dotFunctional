using static Frognar.DotFunctional.Basis;

namespace Frognar.DotFunctional;

public readonly record struct Option<T> : IEquatable<NoneType>
{
        private readonly T? _value;
        private readonly bool _isSome;
        private bool IsNone => !_isSome;

        private Option(T value) => (_value, _isSome) = (value ?? throw new ArgumentNullException(nameof(value)), true);

        internal static Option<T> Some(T value) => new(value);
        public static implicit operator Option<T>(NoneType _) => default;
        public static implicit operator Option<T>(T? value) => value is null ? None : Some(value);

        public R Match<R>(Func<R> none, Func<T, R> some)
        {
                ArgumentNullException.ThrowIfNull(none);
                ArgumentNullException.ThrowIfNull(some);
                return _isSome ? some(_value!) : none();
        }

        public bool Equals(NoneType other) => IsNone;
        public override int GetHashCode() => IsNone ? 0 : EqualityComparer<T>.Default.GetHashCode(_value!) * -1521134295;
        public override string ToString() => Match(() => "None", v => $"Some({v})");
}

public readonly record struct NoneType;

public static class Option
{
        extension<T, R>(Option<T> opt)
        {
                public Option<R> Map(Func<T, R> map)
                {
                        ArgumentNullException.ThrowIfNull(map);
                        return opt.Match(() => None, v => Some(map(v)));
                }

                public Option<R> Bind(Func<T, Option<R>> bind)
                {
                        ArgumentNullException.ThrowIfNull(bind);
                        return opt.Match(() => None, bind);
                }
        }

        extension<T, R>(Option<Func<T, R>> optF)
        {
                public Option<R> Apply(Option<T> optV)
                {
                        return optF.Match(
                                () => None,
                                f => optV.Match(
                                        () => None,
                                        v => Some(f(v))));
                }
        }

        extension<T1, T2, R>(Option<Func<T1, T2, R>> optF)
        {
                public Option<Func<T2, R>> Apply(Option<T1> optV)
                {
                        return optF.Match(
                                () => None,
                                f => optV.Match(
                                        () => None,
                                        t1 => Some<Func<T2, R>>(t2 => f(t1, t2))));
                }
        }
}

public static partial class Basis
{
        public static readonly NoneType None = default!;
        public static Option<T> Some<T>(T value) => Option<T>.Some(value);
}