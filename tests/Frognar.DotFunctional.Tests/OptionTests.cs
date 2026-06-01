using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;
using static Frognar.DotFunctional.Basis;

namespace Frognar.DotFunctional.Tests;

file static class ArbitraryOption
{
        public static Arbitrary<Option<T>> Option<T>() =>
                (from isSome in Gen.OneOf(Gen.Constant(true), Gen.Constant(false))
                        from val in ArbMap.Default.GeneratorFor<T>()
                        select isSome && val != null ? Some(val) : None)
                .ToArbitrary();
}

public sealed class OptionTests
{
        [Fact]
        public void Some_Null_ThrowsArgumentNullException()
        {
                Assert.Throws<ArgumentNullException>(() => Some<string>(null!));
        }

        [Fact]
        public void ImplicitConversion_Default_ReturnsNone()
        {
                Option<int> option = default;
                Assert.True(option.Equals(None));
        }

        [Fact]
        public void ImplicitConversion_Null_ReturnsNone()
        {
                Option<string> option = null;
                Assert.True(option.Equals(None));
        }

        [Property]
        public bool ImplicitConversion_NonNull_ReturnsSome(NonEmptyString value)
        {
                Option<string> option = value.Get;
                return option.Equals(Some(value.Get));
        }

        [Fact]
        public void Match_ReturnsCorrectBranch()
        {
                Assert.Equal("None", Print(None));
                Assert.Equal("1", Print(Some(1)));

                static string Print(Option<int> option)
                        => option.Match(
                                () => "None",
                                s => s.ToString());
        }

        [Fact]
        public void ToString_None_ReturnsNone()
        {
                Option<int> none = None;
                Assert.Equal("None", none.ToString());
        }

        [Property]
        public bool ToString_Some_ReturnsFormattedValue(NonEmptyString value)
        {
                Option<string> some = Some(value.Get);
                return some.ToString() == $"Some({value.Get})";
        }
}

public sealed class OptionEqualityTests
{
        [Fact]
        public void GetHashCode_None_ReturnsZero()
        {
                Option<int> none = None;
                Assert.Equal(0, none.GetHashCode());
        }

        [Property]
        public bool GetHashCode_Some_IsDeterministicAndConsistent(NonEmptyString value)
        {
                string v = value.Get;
                Option<string> some1 = Some(v);
                Option<string> some2 = Some(v);
                return some1.GetHashCode() == some2.GetHashCode();
        }

        [Property]
        public bool Equality_SameValues_AreEqual(NonEmptyString value)
        {
                Option<string> some1 = value.Get;
                Option<string> some2 = value.Get;
                return some1 == some2 && some1.Equals(some2);
        }

        [Property]
        public bool Equality_DifferentValues_AreNotEqual(NonEmptyString value)
        {
                Option<string> some1 = value.Get;
                Option<string> some2 = value.Get + "_different";
                return some1 != some2 && !some1.Equals(some2);
        }

        [Property]
        public bool Equality_SomeAndNone_AreNotEqual(NonEmptyString value)
        {
                Option<string> some = value.Get;
                Option<string> none = None;
                return some != none && !some.Equals(none);
        }

        [Fact]
        public void Equality_NoneAndNone_AreEqual()
        {
                Option<int> none1 = None;
                Option<int> none2 = None;
                Assert.Equal(none1, none2);
        }
}