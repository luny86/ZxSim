using NUnit.Framework;

using Builder;

namespace Tests.Builder
{
    public class CreatorTests
    {
        private class Composition : IComposition
        {
            public string Name => "Composition"; 
        }

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void CompositionsFoundTest()
        {      
            bool found = false;

            // Simple test to make sure no buildables does not have an impact.
            foreach(IComposition buildable in Creator.Compositions)
            {
                if(buildable is Composition)
                {
                    found = true;
                    break;
                }
            }

            Assert.True(found, "Test composition class not found");
        }

        [Test]
        public void BuildAllTest()
        {
            Creator creator = new Creator();
            creator.BuildAll();

            // Make sure we get a null for an unkown.
            IComposition? c = creator.TryGetComposition("Unknown");
            Assert.IsNull(c);

            // Make sure our composition was found and created.
            c = creator.TryGetComposition("Composition");
            Assert.NotNull(c);
        }
    }
}