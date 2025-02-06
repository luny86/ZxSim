
using ZX;

namespace Test.ZX
{
    public class MathsTests
    {
        [Test]
        public void Bit8_SignedIntTest()
        {
            Assert.AreEqual(0, Maths.Bit8_Signed(0));
            Assert.AreEqual(-1, Maths.Bit8_Signed(255));
            Assert.AreEqual(127, Maths.Bit8_Signed(127));
            Assert.AreEqual(-128, Maths.Bit8_Signed(128));
            Assert.AreEqual(-127, Maths.Bit8_Signed(129));            
        }

        [Test]
        public void Bit8_SignedByteTest()
        {
            Assert.AreEqual(0, Maths.Bit8_Signed((byte)0));
            Assert.AreEqual(-1, Maths.Bit8_Signed((byte)255));
            Assert.AreEqual(127, Maths.Bit8_Signed((byte)127));
            Assert.AreEqual(-128, Maths.Bit8_Signed((byte)128));
            Assert.AreEqual(-127, Maths.Bit8_Signed((byte)129));            
        }        
    }
}