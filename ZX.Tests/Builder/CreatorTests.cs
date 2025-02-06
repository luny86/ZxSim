using NUnit.Framework;

using GameEditorLib.Builder;

namespace Tests.Builder
{
    public class CreatorTests
    {
        private class Buildable : IBuildable
        {
            bool askForDependents = false;
            bool register = false;
            bool dependentsMet = false;

            void IBuildable.RegisterObjects(IDependencyPool dependencies)
            {
                Assert.False(askForDependents, "RegisterObjects: AskForDependents call out of order");
                Assert.False(dependentsMet, "RegisterObjects: DependentsMet called out of order.");
                register = true;
            }

            void IBuildable.AskForDependents(IRequests requests)
            {
                Assert.True(register, "AskForDependents: RegisterObjects not called yet");
                Assert.False(dependentsMet, "AskForDependents: DependentsMet called out of order.");
                askForDependents = true;
            }

            void IBuildable.DependentsMet(IDependencies dependencies)
            {
                Assert.True(register, "DependentsMet: RegisterObjects not called yet");
                Assert.True(askForDependents, "DependentsMet: AskForDependents called out of order.");
                dependentsMet = true;
            }

            public void FinalTest()
            {
                Assert.True(register && askForDependents && dependentsMet);
            }
        }

        private class Composition : IComposition
        {
            private Buildable buildable = new Buildable();

            public string Name => "Composition"; 

            public Buildable Child => buildable;

            IList<IBuildable>? IComposition.CreateBuildables()
            {
                return new List<IBuildable>()
                {
                    buildable
                }; 
            }
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

            // Check the inner flags for the correct order
            // of call on a buildable.
            if(c is Composition comp)
            {
                comp.Child.FinalTest();
            }
            else
            {
                Assert.Fail("Composition found is not the test one");
            }
        }
    }
}