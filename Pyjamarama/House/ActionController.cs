
using Builder;

namespace Pyjamarama.House
{
    /// <summary>
    /// Runs <see cref="ITest"/>s and invokes <see cref="IAction"/>s
    /// basedon test results.
    /// </summary>
    /// <remarks>
    /// If a test failes and it implements <see cref="IUpdate"/> then
    /// the test is put onto the update queue and is called through the <see cref="IUpdate"/>
    /// interface on the next frame.
    /// The same goes for an <see cref="IAction"/>, if that also implements the <see cref="IUpdate"/>
    /// interface and returns a true after its invocation.
    /// </remarks>
    internal class ActionController
    {
        #region Commands

        public const int CmdEndOfString = 0xFF;
        public const int CmdEndOfStatement = 0xFE;
        public const int CmdThen = 0xFD;
        public const int CmdEndIf = 0xFE;

        #endregion

        #region Members

        /// <summary>
        /// Queue holding any tests or actions that implement the
        /// <see cref="IUpdate"/> interface and has asked to be 
        /// updated.
        /// </summary>
        private Queue<IUpdate> actionQueue = new Queue<IUpdate>();

        /// <summary>
        /// Provider that supplies the tests and actions.
        /// </summary>
        private IActionProvider _actionProvider;
        #endregion

        #region Construction

        /// <summary>
        /// Create an instance of.
        /// </summary>
        /// <param name="actionProvider">Provider to suply the tests and actions.</param>
        public ActionController(IActionProvider actionProvider)
        {
            _actionProvider = actionProvider;
        }

        #endregion

        #region Controlling methods

        /// <summary>
        /// Main method for testing for an action,
        /// called for each game tick.
        /// </summary>
        public bool CheckActions(int roomIndex)
        {
            bool updatesRequired = false;

            if (this.actionQueue.Count > 0)
            {
                // If action updates are queued,
                // these need dealing, instead of checks
                updatesRequired = RunUpdates();
            }
            else
            {
                updatesRequired = RunChecks(roomIndex);
            }

            return updatesRequired;
        }

        // Run through the next update.
        private bool RunUpdates()
        {
            bool updatesStill = false;
            IUpdate update = this.actionQueue.Peek();

            if (update != null)
            {
                if (update.Update())
                {
                    // Update has finished.
                    // So remove it.
                    this.actionQueue.Dequeue();

                    // See if there are any more.
                    updatesStill = (this.actionQueue.Count > 0);
                }
                else
                {
                    // Not finished yet.
                    updatesStill = true;
                }
            }

            return updatesStill;
        }

        // Run tests and actions.
        private bool RunChecks(int roomIndex)
        {
            bool updatesRequired = false;
            IList<byte> data = null!;

            int i = 0;
            bool done = false;

            if (roomIndex >= 0)
            {
                IReadOnlyList<byte> actions = _actionProvider.RoomActionData(roomIndex);

                // Check for actions.
                do
                {
                    do
                    {
                        if(i > actions.Count)
                        {
                            throw new IndexOutOfRangeException($"Index out of range for Actions. Index = {i}");
                        }

                        ITest test = _actionProvider.Tests[actions[i++]];

                        data = CopyDataFromRoomActionData(actions, i, test.TestDataSize);
                        i += test.TestDataSize;

                        // Call test
                        if(test.Test(data))
                        {
                            updatesRequired = ScanActions(actions, ref i);
                        }
                        else
                        {
                            // See if the test is an update.
                            if(test is IUpdate update)
                            {
                                this.actionQueue.Enqueue(update);
                                updatesRequired = true;
                            }

                            // Skip onto next test / end of data.
                            while(i < actions.Count &&
                                  actions[i] != CmdEndOfString &&
                                  actions[i] != CmdEndOfStatement)
                            {
                                i++;
                            }
                        }
                    }
                    while(!done && actions[i] != CmdEndOfStatement);

                    if(actions[i] == CmdEndOfStatement)
                    {
                        i++;
                    }
                } 
                while(!done && actions[i] != CmdEndOfString);
            }

            return updatesRequired;
        }

        private List<byte> CopyDataFromRoomActionData(IReadOnlyList<byte> roomActionData, int fromIndex, int amountToCopy)
        {
            List<byte> data = new List<byte>();

            // Fill in with a copy of the data.
            for(int k=0;k<amountToCopy;k++)
            {
                data.Add(roomActionData[fromIndex++]);
            }

            return data;
        }

        private bool ScanActions(IReadOnlyList<byte> roomActionData, ref int dataIndex)
        {
            bool updatesRequired = false;

            if(roomActionData[dataIndex] == CmdThen)
            {
                dataIndex++;
                while(roomActionData[dataIndex] != CmdEndIf)
                {
                    IAction action = _actionProvider.Actions[roomActionData[dataIndex++]];
                    IList<byte> data = CopyDataFromRoomActionData(roomActionData, dataIndex, action.DataSize);
                    dataIndex+= action.DataSize;

                    if(action.Invoke(data))
                    {
                        // TODO - Can only have one action update
                        // at a time on a successful test.
                        // Might need to stack them
                        if(action is IUpdate update)
                        {
                            this.actionQueue.Enqueue(update);
                            updatesRequired = true;
                        }
                    }
                }
            }
            else
            { 
                throw new InvalidOperationException($"Invalid room action data for room.");
            }

            return updatesRequired;
        }
        #endregion
    }
}