using FsCheck;
using FsCheck.Xunit;
using static Frognar.DotFunctional.Basis;

namespace Frognar.DotFunctional.Tests;

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