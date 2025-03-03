
using Pyjamarama.House;

namespace PyjamaramaTests.House
{
    public class ActionControllerTests
    {
        /// <summary>
        /// Basic test that can give either result and
        /// have a given data expected data size.
        /// </summary>
        private class SimpleTest : ITest
        { 
            private bool _result;

            public SimpleTest(bool result = false)
            {
                _result = result;
            }

            public int TestDataSize 
            {
                get;
                set;
            } = 0;

            public bool Tested { get; private set; }

            public int DataSize 
            { 
                get;
                set; 
            } = -1;

            bool ITest.Test(IList<byte> data)
            {
                DataSize = data.Count;
                Tested = true;
                return _result;
            }
        }

        /// <summary>
        /// Update test that will fail and get
        /// put onto the update queue.
        /// </summary>
        private class UpdateTest : ITest, IUpdate
        {
            int ITest.TestDataSize => 0;

            public bool Updated { get; private set; }

            public bool Update()
            {
                Updated = true;
                return true;
            }

            bool ITest.Test(IList<byte> data)
            {
                return false;
            }
        }

        private class TestAction : IAction
        {
            public int DataSize => 2;

            public bool RanOk
            {
                get;
                private set;
            }

            public int DataSizeFound
            {
                get;
                set;
            } = -1;

            public bool Invoke(IList<byte> data)
            {
                DataSizeFound = data.Count;
                RanOk = true;
                return true;
            }
        }

        public class UpdateAction : IAction, IUpdate
        {
            private bool _requiresUpdate;

            public UpdateAction(bool requiresUpdate)
            {
                _requiresUpdate = requiresUpdate;
            }
        
            int IAction.DataSize => 0;

            bool IAction.Invoke(IList<byte> data)
            {
                return _requiresUpdate;
            }


            public bool Updated { get; private set; }
            bool IUpdate.Update()
            {
                Updated = true;
                return true;
            }
        }

        public class ActionTestProvider : IActionProvider
        {
            ITest _test = null!;
            IAction _action = null!;

            public ActionTestProvider(ITest test)
            {
                _test = test;
            }

            public ActionTestProvider(ITest test, IAction action)
            {
                _test =test;
                _action = action;
            }

            IReadOnlyList<ITest> IActionProvider.Tests => 
                new List<ITest>()
                {
                    _test
                };

            IReadOnlyList<IAction> IActionProvider.Actions => 
                new List<IAction>()
                {
                    _action
                };

            IReadOnlyList<byte> IActionProvider.RoomActionData(int room)
            {
                return _data[room];
            }

            private static List<List<byte>> _data = new List<List<byte>> ()
                {
                    // Basic test with no actions
                    new List<byte> { 0x00, ActionController.CmdEndOfStatement, ActionController.CmdEndOfString },
                    // Malformed with no 'Then' commands
                    new List<byte> { 0x00, 0x01 },
                    // True test, no data that runs an action
                    // False test should skip the Then and Endif
                    new List<byte> { 0x00, ActionController.CmdThen, 
                                                0x00, 0x20,0x30,
                                            ActionController.CmdEndIf, 
                                            ActionController.CmdEndOfString},
                    new List<byte> { 0x00, ActionController.CmdThen, 0x00, ActionController.CmdEndIf, ActionController.CmdEndOfString}
                };
        }

        [Test]
        public void RunFalseTestNoActionTest()
        {
            SimpleTest test = new SimpleTest();

            ActionController controller = new ActionController(
                new ActionTestProvider(
                    test));

            bool updatesRequired = controller.CheckActions(0);

            Assert.That(updatesRequired, Is.False);
            Assert.That(test.Tested, Is.True);
            Assert.That(test.DataSize, Is.EqualTo(0));
        }

        [Test]
        public void RunTrueTestWithMalformedData()
        {
            SimpleTest test = new SimpleTest(true);

            ActionController controller = new ActionController(
                new ActionTestProvider(
                    test));

            Assert.Throws<InvalidOperationException>(() =>
                controller.CheckActions(1)
            );
        }

        [Test]
        public void RunTrueTestWithActionTest()
        {
            SimpleTest test = new SimpleTest(true);
            TestAction action = new TestAction();

            ActionController controller = new ActionController(
                new ActionTestProvider(
                    test, action));

            controller.CheckActions(2);
            Assert.That(action.RanOk, Is.True);
            Assert.That(action.DataSizeFound, Is.EqualTo(2));
        }

        [Test]
        public void RunFalseTestWithActionTest()
        {
            SimpleTest test = new SimpleTest(false);
            TestAction action = new TestAction();

            ActionController controller = new ActionController(
                new ActionTestProvider(
                    test, action));

            controller.CheckActions(2);
            Assert.That(action.RanOk, Is.False);
        }

        [Test]
        public void RunFalseTestWithUpdate()
        {
            UpdateTest test = new UpdateTest();

            ActionController controller = new ActionController(
                new ActionTestProvider(
                    test));

            controller.CheckActions(0);   

            Assert.That(test.Updated, Is.False);

            bool updatesLeft = controller.CheckActions(0);

            Assert.That(test.Updated, Is.True);
            Assert.That(updatesLeft, Is.False);
        }

        [Test]
        public void RunTrueTestWithActionUpdateFalse()
        {
            SimpleTest test = new SimpleTest(true);
            // Action will not ask to be put onto update queue.
            UpdateAction action = new UpdateAction(false);

            ActionController controller = new ActionController(
                new ActionTestProvider(
                    test, action));

            bool requiresUpdate = controller.CheckActions(3);

            Assert.That(requiresUpdate, Is.False);
            Assert.That(action.Updated, Is.False);

            controller.CheckActions(3);

            Assert.That(action.Updated, Is.False);   
        }

        [Test]
        public void RunTrueTestWithActionUpdateTrue()
        {
            SimpleTest test = new SimpleTest(true);
            // Action will ask to be put onto update queue.
            UpdateAction action = new UpdateAction(true);

            ActionController controller = new ActionController(
                new ActionTestProvider(
                    test, action));

            bool requiresUpdate = controller.CheckActions(3);

            Assert.That(requiresUpdate, Is.True);
            Assert.That(action.Updated, Is.False);

            controller.CheckActions(3);

            Assert.That(action.Updated, Is.True);   
        }
    }
}