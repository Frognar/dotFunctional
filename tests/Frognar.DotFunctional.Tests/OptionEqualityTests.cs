using FsCheck;
using FsCheck.Xunit;

namespace Frognar.DotFunctional.Tests;

public sealed class OptionEqualityTests
{
        [Fact]
        public void GetHashCode_None_ReturnsZero()
        {
                Option<int> none = Basis.None;
                Assert.Equal(0, none.GetHashCode());
        }

        [Property]
        public void GetHashCode_Some_IsDeterministicAndConsistent(NonEmptyString value)
        {
                string v = value.Get;
                Option<string> some1 = Basis.Some(v);
                Option<string> some2 = Basis.Some(v);
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
                Assert.NotEqual(some, Basis.None);
        }

        [Fact]
        public void Equality_NoneAndNone_AreEqual()
        {
                Option<int> none1 = Basis.None;
                Option<int> none2 = Basis.None;
                Assert.Equal(none1, none2);
        }

        [Fact]
        public void EqualityNoneType_Some_ReturnsFalse()
        {
                Option<string> some = Basis.Some("value");
                Assert.False(some.Equals(Basis.None));
        }

        [Fact]
        public void EqualityNoneType_None_ReturnsTrue()
        {
                Option<int> none = Basis.None;
                Assert.True(none.Equals(Basis.None));
        }
}