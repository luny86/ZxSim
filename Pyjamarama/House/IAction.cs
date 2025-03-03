
namespace Pyjamarama.House
{
    /// <summary>
    /// Interface describing an action.
    /// Actions handle events that ocurr when
    /// Wally hits a point on the screen and
    /// creates a trigger.
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// number of bytes expected in the
        /// data passed to the Invoke method.
        /// </summary>
        int DataSize
        {
            get;
        }

        // Gets called immediately after a test returns true.
        // Returns true to run the update method.
        bool Invoke(IList<byte> data);
    }
}

