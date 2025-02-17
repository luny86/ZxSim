using NUnit.Framework;

using Builder;

namespace ZX.Tests.Builder
{
    public class DependencyPoolTests
    {
        #region Test Types
        private class D 
        {
        }

        private class A : IComposition, IBuildable
        {
            string IComposition.Name => "A";

            void IBuildable.AskForDependents(IRequests requests)
            {
            }

            IList<IBuildable>? IBuildable.CreateBuildables()
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

            void IBuildable.EndBuild()
            {

            }
        }

        private class B : IComposition, IBuildable
        {
            string IComposition.Name => "B";

            void IBuildable.AskForDependents(IRequests requests)
            {
                requests.AddRequest("T.A", typeof(D));
            }

            IList<IBuildable>? IBuildable.CreateBuildables()
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

            void IBuildable.EndBuild()
            {

            }
        }
        #endregion

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void DependenciesBetweenTwoCompositions()
        {
            // Creates the compositions.
            Creator creator = new Creator();
            creator.BuildAll();

            Assert.Pass();
        }
    }
}