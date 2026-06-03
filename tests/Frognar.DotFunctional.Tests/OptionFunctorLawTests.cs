using FsCheck.Xunit;

namespace Frognar.DotFunctional.Tests;

public sealed class OptionFunctorLawTests
{
        [Property(Arbitrary = [typeof(ArbitraryOption)])]
        public void IdentityLaw(Option<string> option)
        {
                Assert.Equal(option, option.Map(Basis.Id));
        }

        [Property(Arbitrary = [typeof(ArbitraryOption)])]
        public void CompositionLaw(Option<int> option)
        {
                Func<int, bool> f = i => i > 0;
                Func<bool, string> g = i => i.ToString();
                Assert.Equal(option.Map(f).Map(g), option.Map(v => g(f(v))));
        }
}