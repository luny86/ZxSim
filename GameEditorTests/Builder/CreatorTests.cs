using NUnit.Framework;

using GameEditorLib.Builder;

namespace Tests
{
    public class CreatorTests
    {
        private class Composition : IComposition
        {
            IList<IBuildable>? IComposition.CreateBuildables()
            {
                return null;
            }
        }

        [Test]
        public void BuildWithCompositionOnly()
        {
            // Simple test to make sure no buildables does not have an impact.
            //Creator creator = new GameEditorLib.Builder.Creator();
        }
    }
}