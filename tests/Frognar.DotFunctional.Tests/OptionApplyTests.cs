using FsCheck.Xunit;
using static Frognar.DotFunctional.Basis;

namespace Frognar.DotFunctional.Tests;

public sealed class OptionApplyTests
{
        Func<int, int, int> add =
                (a, b) => a + b;

        Func<int, int, int, int> add3 =
                (a, b, c) => a + b + c;

        Func<int, int, int, int, int> add4 =
                (a, b, c, d) => a + b + c + d;

        [Fact]
        public void Apply_Some_2Args()
        {
                Option<int> result = Some(add)
                        .Apply(Some(3))
                        .Apply(Some(4));

                Assert.Equal(Some(7), result);
        }

        [Fact]
        public void Apply_Some_3Args()
        {
                Option<int> result = Some(add3)
                        .Apply(Some(3))
                        .Apply(Some(4))
                        .Apply(Some(-1));

                Assert.Equal(Some(6), result);
        }

        [Fact]
        public void Apply_Some_4Args()
        {
                Option<int> result = Some(add4)
                        .Apply(Some(3))
                        .Apply(Some(4))
                        .Apply(Some(-1))
                        .Apply(Some(6));

                Assert.Equal(Some(12), result);
        }
}