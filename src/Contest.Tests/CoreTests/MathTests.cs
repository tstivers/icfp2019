using Contest.Core.Extensions;
using NUnit.Framework;

namespace Contest.Tests.CoreTests
{
    [TestFixture]
    public class MathTests
    {
        [Test]
        public void TestClamp()
        {
            var a = MathExtensions.Clamp(-5, -1, 1);
            var b = MathExtensions.Clamp(5, -1, 1);
            var c = MathExtensions.Clamp(0, -1, 1);

            Assert.That(a, Is.EqualTo(-1));
            Assert.That(b, Is.EqualTo(1));
            Assert.That(c, Is.EqualTo(0));
        }
    }
}