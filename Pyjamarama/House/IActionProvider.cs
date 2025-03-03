
namespace Pyjamarama.House
{
    /// <summary>
    /// Describes a room action data provider.
    /// </summary>
    public interface IActionProvider
    {
        /// <summary>
        /// Data string, of tests and actions, for each room.
        /// </summary>
        /// <param name="room">Index of data to get.</param>
        /// <returns>A list of data for given room.</returns>
        IReadOnlyList<byte> RoomActionData(int room);

        /// <summary>
        /// Available tests, used by the room data.
        /// </summary>
        IReadOnlyList<ITest> Tests { get; }

        /// <summary>
        /// Available actions, used by the room data.
        /// </summary>
        IReadOnlyList<IAction> Actions { get; }
    }
}