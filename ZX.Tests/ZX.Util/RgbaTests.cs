
using ZX.Util;

namespace ZX.Tests.ZX.Util
{
    public class RgbaTests
    {
        [Test]
        public void EqualsWhereRightIsNull()
        {
            Rgba lval = new Rgba(1.0f, 1.0f, 1.0f, 1.0f);

            Assert.That(lval, Is.Not.EqualTo(null));
        }

        [Test]
        public void EqualsWhereRightIsNotRgba()
        {
            Rgba lval = new Rgba(1.0f, 1.0f, 1.0f, 1.0f);

            Assert.That(lval, Is.Not.EqualTo(new object()));
        }

        [TestCase(1.0f,1.0f,1.0f,1.0f, ExpectedResult = true, TestName="RGBA is equals" )]
        [TestCase(0.0f,1.0f,1.0f,1.0f, ExpectedResult = false, TestName="RGBA where R is different." )]
        [TestCase(1.0f,0.0f,1.0f,1.0f, ExpectedResult = false, TestName="RGBA where G is different." )]
        [TestCase(1.0f,1.0f,0.0f,1.0f, ExpectedResult = false, TestName="RGBA where B is different." )]
        [TestCase(1.0f,1.0f,1.0f,0.0f, ExpectedResult = false, TestName="RGBA where A is different." )]
        public bool EqualsTest(float r, float g, float b, float a)
        {
            Rgba lval = new Rgba(1.0f, 1.0f, 1.0f, 1.0f);
            Rgba rval = new Rgba(r,g,b,a);

            return lval.Equals(rval);
        }

        [TestCase(1.0f,1.0f,1.0f,1.0f, ExpectedResult = true, TestName="RGBA ==" )]
        [TestCase(0.0f,1.0f,1.0f,1.0f, ExpectedResult = false, TestName="RGBA ==, where R is different." )]
        [TestCase(1.0f,0.0f,1.0f,1.0f, ExpectedResult = false, TestName="RGBA ==, where G is different." )]
        [TestCase(1.0f,1.0f,0.0f,1.0f, ExpectedResult = false, TestName="RGBA ==, where B is different." )]
        [TestCase(1.0f,1.0f,1.0f,0.0f, ExpectedResult = false, TestName="RGBA ==, where A is different." )]
        public bool OperatorEqualsTest(float r, float g, float b, float a)
        {
            Rgba lval = new Rgba(1.0f, 1.0f, 1.0f, 1.0f);
            Rgba rval = new Rgba(r,g,b,a);

            return lval == rval;
        }    

        [TestCase(1.0f,1.0f,1.0f,1.0f, ExpectedResult = false, TestName="RGBA != is equals" )]
        [TestCase(0.0f,1.0f,1.0f,1.0f, ExpectedResult = true, TestName="RGBA != where R is different." )]
        [TestCase(1.0f,0.0f,1.0f,1.0f, ExpectedResult = true, TestName="RGBA != where G is different." )]
        [TestCase(1.0f,1.0f,0.0f,1.0f, ExpectedResult = true, TestName="RGBA != where B is different." )]
        [TestCase(1.0f,1.0f,1.0f,0.0f, ExpectedResult = true, TestName="RGBA != where A is different." )]
        public bool OperatorNotEqualsTest(float r, float g, float b, float a)
        {
            Rgba lval = new Rgba(1.0f, 1.0f, 1.0f, 1.0f);
            Rgba rval = new Rgba(r,g,b,a);

            return lval != rval;
        }    
    }
}