using FsCheck;
using FsCheck.Fluent;

namespace Frognar.DotFunctional.Tests;

internal static class ArbitraryOption
{
        public static Arbitrary<Option<T>> Option<T>() =>
                (from isSome in Gen.OneOf(Gen.Constant(true), Gen.Constant(false))
                        from val in ArbMap.Default.GeneratorFor<T>()
                        select isSome && val != null ? Basis.Some(val) : Basis.None)
                .ToArbitrary();
}