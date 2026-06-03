using FsCheck.Xunit;

namespace Frognar.DotFunctional.Tests;

public sealed class OptionMonadLawTests
{
        [Property]
        public void LeftIdentityLaw(int value)
        {
                Func<int, Option<int>> f = x => x > 0 ? Basis.Some(x) : Basis.None;
                Assert.Equal(Basis.Some(value).Bind(f), f(value));
        }


        [Property(Arbitrary = [typeof(ArbitraryOption)])]
        public void RightIdentityLaw(Option<int> optV)
        {
                Assert.Equal(optV.Bind(Basis.Some), optV);
        }

        [Property(Arbitrary = [typeof(ArbitraryOption)])]
        public void AssociativityLaw(Option<int> optV)
        {
                Func<int, Option<string>> f = x => x > 0 ? Basis.Some(x.ToString()) : Basis.None;
                Func<string, Option<int>> g = x => x.Length > 10 ? Basis.Some(x.Length) : Basis.None;
                Assert.Equal(optV.Bind(f).Bind(g), optV.Bind(v => f(v).Bind(g)));
        }
}