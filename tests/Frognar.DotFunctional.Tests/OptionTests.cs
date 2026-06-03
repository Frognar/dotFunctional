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
                Assert.Equal(None, option);
        }

        [Fact]
        public void ImplicitConversion_Null_ReturnsNone()
        {
                Option<string> option = null;
                Assert.Equal(None, option);
        }

        [Property]
        public void ImplicitConversion_NonNull_ReturnsSome(NonEmptyString value)
        {
                Option<string> option = value.Get;
                Assert.Equal(Some(value.Get), option);
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

        [Property]
        public void Where_Some_ReturnsSomeForTruePredicate(NonEmptyString value)
        {
                Option<string> option = Some(value.Get);
                Assert.Equal(Some(value.Get), option.Where(s => s.Length > 0));
        }

        [Property]
        public void Where_Some_ReturnsNoneForFalsePredicate(NonEmptyString value)
        {
                Option<string> option = Some(value.Get);
                Assert.Equal(None, option.Where(s => s.Length == 0));
        }

        [Fact]
        public void Where_None_ReturnsNone()
        {
                Option<string> option = None;
                Assert.Equal(None, option.Where(s => s.Length > 0));
        }

        [Property]
        public void Iter_Some_ExecutesAction(NonEmptyString value)
        {
                Option<string> some = value.Get;
                bool executed = false;
                some.Iter(_ => executed = true);
                Assert.True(executed);
        }

        [Fact]
        public void Iter_None_DoesNotExecuteAction()
        {
                Option<string> none = None;
                bool executed = false;
                none.Iter(_ => executed = true);
                Assert.False(executed);
        }

        [Fact]
        public void ToString_None_ReturnsNone()
        {
                Option<int> none = None;
                Assert.Equal("None", none.ToString());
        }

        [Property]
        public void ToString_Some_ReturnsFormattedValue(NonEmptyString value)
        {
                Option<string> some = Some(value.Get);
                Assert.Equal($"Some({value.Get})", some.ToString());
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
        public void GetHashCode_Some_IsDeterministicAndConsistent(NonEmptyString value)
        {
                string v = value.Get;
                Option<string> some1 = Some(v);
                Option<string> some2 = Some(v);
                Assert.Equal(some1.GetHashCode(), some2.GetHashCode());
        }

        [Property]
        public void Equality_SameValues_AreEqual(NonEmptyString value)
        {
                Option<string> some1 = value.Get;
                Option<string> some2 = value.Get;
                Assert.Equal(some1, some2);
        }

        [Property]
        public void Equality_DifferentValues_AreNotEqual(NonEmptyString value)
        {
                Option<string> some1 = value.Get;
                Option<string> some2 = value.Get + "_different";
                Assert.NotEqual(some1, some2);
        }

        [Property]
        public void Equality_SomeAndNone_AreNotEqual(NonEmptyString value)
        {
                Option<string> some = value.Get;
                Assert.NotEqual(some, None);
        }

        [Fact]
        public void Equality_NoneAndNone_AreEqual()
        {
                Option<int> none1 = None;
                Option<int> none2 = None;
                Assert.Equal(none1, none2);
        }

        [Fact]
        public void EqualityNoneType_Some_ReturnsFalse()
        {
                Option<string> some = Some("value");
                Assert.False(some.Equals(None));
        }

        [Fact]
        public void EqualityNoneType_None_ReturnsTrue()
        {
                Option<int> none = None;
                Assert.True(none.Equals(None));
        }
}

public sealed class OptionFunctorLawTests
{
        [Property(Arbitrary = [typeof(ArbitraryOption)])]
        public void IdentityLaw(Option<string> option)
        {
                Assert.Equal(option, option.Map(Id));
        }

        [Property(Arbitrary = [typeof(ArbitraryOption)])]
        public void CompositionLaw(Option<int> option)
        {
                Func<int, bool> f = i => i > 0;
                Func<bool, string> g = i => i.ToString();
                Assert.Equal(option.Map(f).Map(g), option.Map(v => g(f(v))));
        }
}

public sealed class OptionApplicativeLawTests
{
        [Property(Arbitrary = [typeof(ArbitraryOption)])]
        public void IdentityLaw(Option<int> optV)
        {
                Assert.Equal(Some<Func<int, int>>(Id).Apply(optV), optV);
        }

        [Property]
        public void HomomorphismLaw(int x)
        {
                Func<int, bool> f = i => i > 0;
                Assert.Equal(Some(f).Apply(Some(x)), Some(f(x)));
        }

        [Property]
        public void InterchangeLaw(bool uIsSome, int y)
        {
                Option<Func<int, bool>> u = uIsSome ? Some((int x) => x > 0) : None;
                Func<Func<int, bool>, bool> applyY = f => f(y);
                Assert.Equal(u.Apply(Some(y)), Some(applyY).Apply(u));
        }

        [Property(Arbitrary = [typeof(ArbitraryOption)])]
        public void CompositionLaw(bool uIsSome, bool vIsSome, Option<int> w)
        {
                Func<Func<bool, string>, Func<int, bool>, Func<int, string>> compose = (f, g) => v => f(g(v));
                Option<Func<bool, string>> u = uIsSome ? Some((bool x) => x.ToString()) : None;
                Option<Func<int, bool>> v = vIsSome ? Some((int x) => x > 0) : None;

                Assert.Equal(Some(compose)
                               .Apply(u)
                               .Apply(v)
                               .Apply(w),
                       u.Apply(v.Apply(w)));
        }
}

public sealed class OptionMonadLawTests
{
        [Property]
        public void LeftIdentityLaw(int value)
        {
                Func<int, Option<int>> f = x => x > 0 ? Some(x) : None;
                Assert.Equal(Some(value).Bind(f), f(value));
        }


        [Property(Arbitrary = [typeof(ArbitraryOption)])]
        public void RightIdentityLaw(Option<int> optV)
        {
                Assert.Equal(optV.Bind(Some), optV);
        }

        [Property(Arbitrary = [typeof(ArbitraryOption)])]
        public void AssociativityLaw(Option<int> optV)
        {
                Func<int, Option<string>> f = x => x > 0 ? Some(x.ToString()) : None;
                Func<string, Option<int>> g = x => x.Length > 10 ? Some(x.Length) : None;
                Assert.Equal(optV.Bind(f).Bind(g), optV.Bind(v => f(v).Bind(g)));
        }
}