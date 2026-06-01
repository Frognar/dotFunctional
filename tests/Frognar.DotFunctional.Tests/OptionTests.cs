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

public sealed class OptionFunctorLawTests
{
        [Property(Arbitrary = [typeof(ArbitraryOption)])]
        public bool IdentityLaw(Option<string> option)
        {
                return option.Map(Id) == option;
        }

        [Property(Arbitrary = [typeof(ArbitraryOption)])]
        public bool CompositionLaw(Option<int> option)
        {
                Func<int, bool> f = i => i > 0;
                Func<bool, string> g = i => i.ToString();
                return option.Map(f).Map(g) == option.Map(v => g(f(v)));
        }
}

public sealed class OptionApplicativeLawTests
{
        [Property(Arbitrary = [typeof(ArbitraryOption)])]
        public bool IdentityLaw(Option<int> optV)
        {
                return Some<Func<int, int>>(Id).Apply(optV) == optV;
        }

        [Property]
        public bool HomomorphismLaw(int x)
        {
                Func<int, bool> f = i => i > 0;
                return Some(f).Apply(Some(x)) == Some(f(x));
        }

        [Property]
        public bool InterchangeLaw(bool uIsSome, int y)
        {
                Option<Func<int, bool>> u = uIsSome ? Some((int x) => x > 0) : None;
                Func<Func<int, bool>, bool> applyY = f => f(y);
                return u.Apply(Some(y)) == Some(applyY).Apply(u);
        }

        [Property(Arbitrary = [typeof(ArbitraryOption)])]
        public bool CompositionLaw(bool uIsSome, bool vIsSome, Option<int> w)
        {
                Func<Func<bool, string>, Func<int, bool>, Func<int, string>> compose = (f, g) => v => f(g(v));
                Option<Func<bool, string>> u = uIsSome ? Some((bool x) => x.ToString()) : None;
                Option<Func<int, bool>> v = vIsSome ? Some((int x) => x > 0) : None;

                return Some(compose)
                               .Apply(u)
                               .Apply(v)
                               .Apply(w)
                       == u.Apply(v.Apply(w));
        }
}