using static Frognar.DotFunctional.F;

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

public static class F
{
        public static readonly NoneType None = default!;
        public static Option<T> Some<T>(T value) => Option<T>.Some(value);
}