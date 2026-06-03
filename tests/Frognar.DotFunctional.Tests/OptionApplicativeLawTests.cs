using FsCheck.Xunit;

namespace Frognar.DotFunctional.Tests;

public sealed class OptionApplicativeLawTests
{
        [Property(Arbitrary = [typeof(ArbitraryOption)])]
        public void IdentityLaw(Option<int> optV)
        {
                Assert.Equal(Basis.Some<Func<int, int>>(Basis.Id<int>).Apply(optV), optV);
        }

        [Property]
        public void HomomorphismLaw(int x)
        {
                Func<int, bool> f = i => i > 0;
                Assert.Equal(Basis.Some(f).Apply(Basis.Some(x)), Basis.Some(f(x)));
        }

        [Property]
        public void InterchangeLaw(bool uIsSome, int y)
        {
                Option<Func<int, bool>> u = uIsSome ? Basis.Some((int x) => x > 0) : Basis.None;
                Func<Func<int, bool>, bool> applyY = f => f(y);
                Assert.Equal(u.Apply(Basis.Some(y)), Basis.Some(applyY).Apply(u));
        }

        [Property(Arbitrary = [typeof(ArbitraryOption)])]
        public void CompositionLaw(bool uIsSome, bool vIsSome, Option<int> w)
        {
                Func<Func<bool, string>, Func<int, bool>, Func<int, string>> compose = (f, g) => v => f(g(v));
                Option<Func<bool, string>> u = uIsSome ? Basis.Some((bool x) => x.ToString()) : Basis.None;
                Option<Func<int, bool>> v = vIsSome ? Basis.Some((int x) => x > 0) : Basis.None;

                Assert.Equal(Basis.Some(compose)
                                .Apply(u)
                                .Apply(v)
                                .Apply(w),
                        u.Apply(v.Apply(w)));
        }
}