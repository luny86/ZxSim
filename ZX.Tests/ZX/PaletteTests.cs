
using System.Collections;
using NUnit.Framework;
using ZX.Util;

namespace ZX.Tests.ZX
{
    public class PaletteTests
    {
        public class TestAttribute(Rgba paper, Rgba ink) : IAttribute
        {
            public Rgba Paper { get; set; } = paper;
            public Rgba Ink { get; set; } = ink;
        }


        public class TestData 
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(
                        new TestAttribute(new Rgba(0.2f,0.2f,0.2f), new Rgba(0.3f,0.4f,0.5f)))
                        .SetName("Test attribute for unknown RGBA values returning 0.")
                        .Returns(0x00);
                    yield return new TestCaseData(
                        new TestAttribute(Palette.Black, Palette.Black))
                        .SetName("Test attribute for black and black.")
                        .Returns(0x00);
                    yield return new TestCaseData(
                        new TestAttribute(Palette.Black, Palette.White))
                        .SetName("Test attribute for black and white.")
                        .Returns(0x07);
                    yield return new TestCaseData(
                        new TestAttribute(Palette.White, Palette.White))
                        .SetName("Test attribute for white and white.")
                        .Returns(0x3f);
                    yield return new TestCaseData(
                        new TestAttribute(Palette.BrightBlack, Palette.BrightBlack))
                        .SetName("Test attribute for bright black and bright black.")
                        .Returns(0x40);
                    yield return new TestCaseData(
                        new TestAttribute(Palette.BrightGreen, Palette.BrightCyan))
                        .SetName("Test attribute for bright green and bright cyan.")
                        .Returns(0x65);
                }
            }
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.TestCases))]
        public byte GetAttributeTest(TestAttribute value)
        {
            return Palette.GetAttribute(value);
        }
    }
}