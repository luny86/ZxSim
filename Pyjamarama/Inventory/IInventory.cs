
namespace Pyjamarama.Inventory
{
    /// <summary>
    /// Interface to the inventory object.
    /// </summary>
    public interface IInventory
    {
        /// <summary>
        /// Rotates the pockets with a given new object 
        /// that has been picked up
        /// </summary>
        /// <param name="newObjectIndex">Index to new object to place into pockets</param>
        /// <returns>Index to object taken out of pockets.</returns>
        int RotatePockets(int newObjectIndex);

        /// <summary>
        /// Determines if either pocket contains 
        /// the given object index.
        /// </summary>
        /// <param name="objectIndex">Index of object to check for.</param>
        /// <returns>True if being carried.</returns>
        bool IsCarrying(int objectIndex);

        /// <summary>
        /// Update energy by a given difference.
        /// </summary>
        /// <param name="by">Amount of energy to lose.</param>
        void LoseEnergy(int by);
    }
}