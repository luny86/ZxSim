
using ZX;

namespace ZX.Tests.ZX
{
    public class MathsTests
    {
        [Test]
        public void Bit8_SignedIntTest()
        {
            Assert.That(Maths.Bit8_Signed(0), Is.EqualTo(0));
            Assert.That(Maths.Bit8_Signed(255), Is.EqualTo(-1));
            Assert.That(Maths.Bit8_Signed(127), Is.EqualTo(127));
            Assert.That(Maths.Bit8_Signed(128), Is.EqualTo(-128));
            Assert.That(Maths.Bit8_Signed(129), Is.EqualTo(-127));            
        }

        [Test]
        public void Bit8_SignedByteTest()
        {
            Assert.That(Maths.Bit8_Signed((byte)0), Is.EqualTo(0));
            Assert.That(Maths.Bit8_Signed((byte)255), Is.EqualTo(-1));
            Assert.That(Maths.Bit8_Signed((byte)127), Is.EqualTo(127));
            Assert.That(Maths.Bit8_Signed((byte)128), Is.EqualTo(-128));
            Assert.That(Maths.Bit8_Signed((byte)129), Is.EqualTo(-127));            
        }        
    }
}