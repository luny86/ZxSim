
using Builder;
using Pyjamarama.Inventory;

namespace Pyjamarama.House
{
   public class TestCarrying : ITest, IBuildable
    {
        const int ObjectIndex = 0;

        private IInventory  _inventory = null!;
        
        public TestCarrying()
        {
        }


        #region ITest implementation
        int ITest.TestDataSize
        {
            get
            {
                return 1;
            }
        }

        bool ITest.Test(System.Collections.Generic.IList<byte> data)
        {
            return _inventory.IsCarrying(data[ObjectIndex]);
        }
        #endregion

        #region IBuildable

        void IBuildable.AskForDependents(IRequests requests)
        {
            requests.AddRequest(ClassNames.Inventory, typeof(IInventory));
        }

        IList<IBuildable>? IBuildable.CreateBuildables()
        {
            return null;
        }

        void IBuildable.DependentsMet(IDependencies dependencies)
        {
            _inventory = dependencies.TryGetInstance<IInventory>(ClassNames.Inventory);
        }

        void IBuildable.EndBuild()
        {

        }

        void IBuildable.RegisterObjects(IDependencyPool dependencies)
        {

        }

        #endregion
    }
}