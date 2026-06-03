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

        Func<int, int, int, int, int, int> add5 =
                (a, b, c, d, e) => a + b + c + d + e;

        Func<int, int, int, int, int, int, int> add6 =
                (a, b, c, d, e, f) => a + b + c + d + e + f;

        Func<int, int, int, int, int, int, int, int> add7 =
                (a, b, c, d, e, f, g) => a + b + c + d + e + f + g;

        Func<int, int, int, int, int, int, int, int, int> add8 =
                (a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h;

        Func<int, int, int, int, int, int, int, int, int, int> add9 =
                (a, b, c, d, e, f, g, h, i) => a + b + c + d + e + f + g + h + i;

        Func<int, int, int, int, int, int, int, int, int, int, int> add10 =
                (a, b, c, d, e, f, g, h, i, j) => a + b + c + d + e + f + g + h + i + j;

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

        [Fact]
        public void Apply_Some_5Args()
        {
                Option<int> result = Some(add5)
                        .Apply(Some(3))
                        .Apply(Some(4))
                        .Apply(Some(-1))
                        .Apply(Some(6))
                        .Apply(Some(2));

                Assert.Equal(Some(14), result);
        }

        [Fact]
        public void Apply_Some_6Args()
        {
                Option<int> result = Some(add6)
                        .Apply(Some(3))
                        .Apply(Some(4))
                        .Apply(Some(-1))
                        .Apply(Some(6))
                        .Apply(Some(2))
                        .Apply(Some(-8));

                Assert.Equal(Some(6), result);
        }

        [Fact]
        public void Apply_Some_7Args()
        {
                Option<int> result = Some(add7)
                        .Apply(Some(3))
                        .Apply(Some(4))
                        .Apply(Some(-1))
                        .Apply(Some(6))
                        .Apply(Some(2))
                        .Apply(Some(-8))
                        .Apply(Some(3));

                Assert.Equal(Some(9), result);
        }

        [Fact]
        public void Apply_Some_8Args()
        {
                Option<int> result = Some(add8)
                        .Apply(Some(3))
                        .Apply(Some(4))
                        .Apply(Some(-1))
                        .Apply(Some(6))
                        .Apply(Some(2))
                        .Apply(Some(-8))
                        .Apply(Some(3))
                        .Apply(Some(3));

                Assert.Equal(Some(12), result);
        }

        [Fact]
        public void Apply_Some_9Args()
        {
                Option<int> result = Some(add9)
                        .Apply(Some(3))
                        .Apply(Some(4))
                        .Apply(Some(-1))
                        .Apply(Some(6))
                        .Apply(Some(2))
                        .Apply(Some(-8))
                        .Apply(Some(3))
                        .Apply(Some(3))
                        .Apply(Some(1));

                Assert.Equal(Some(13), result);
        }

        [Fact]
        public void Apply_Some_10Args()
        {
                Option<int> result = Some(add10)
                        .Apply(Some(3))
                        .Apply(Some(4))
                        .Apply(Some(-1))
                        .Apply(Some(6))
                        .Apply(Some(2))
                        .Apply(Some(-8))
                        .Apply(Some(3))
                        .Apply(Some(3))
                        .Apply(Some(1))
                        .Apply(Some(-10));

                Assert.Equal(Some(3), result);
        }
}