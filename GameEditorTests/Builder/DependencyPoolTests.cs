using NUnit.Framework;

using GameEditorLib.Builder;

namespace Tests
{
    public class DependencyPoolTests
    {
        private class D 
        {
        }

        private class A : IComposition, IBuildable
        {
            void IBuildable.AskForDependents(IRequests requests)
            {
            }

            IList<IBuildable>? IComposition.CreateBuildables()
            {
                return null;
            }

            void IBuildable.DependentsMet(IDependencies dependencies)
            {
            }

            void IBuildable.RegisterObjects(IDependencyPool dependencies)
            {
                dependencies.Add("T.A", typeof(D), new D());
            }
        }

        private class B : IComposition, IBuildable
        {
            void IBuildable.AskForDependents(IRequests requests)
            {
                requests.AddRequest("T.A", typeof(D));
            }

            IList<IBuildable>? IComposition.CreateBuildables()
            {
                return null;
            }

            void IBuildable.DependentsMet(IDependencies dependencies)
            {
                if (dependencies.TryGetInstance("T.A", typeof(D)) is not D d)
                {
                    Assert.Fail("B composition was not able to find dependency 'T.A2'.");
                }
            }

            void IBuildable.RegisterObjects(IDependencyPool dependencies)
            {
            }
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void DependenciesBetweenTwoCompositions()
        {
            // Creates the compositions.
            GameEditorLib.GameEditor.Initialise();

            Assert.Pass();
        }
    }
}