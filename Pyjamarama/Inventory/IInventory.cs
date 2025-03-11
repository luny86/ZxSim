
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
    }
}