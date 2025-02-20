
namespace Pyjamarama.Inventory
{
    /// <summary>
    /// Struct holding values used by the inventory / scoreboard. 
    /// </summary>
    internal struct InventoryStats
    {
        /// <summary>
        /// First (top) pocket, holding an object.
        /// </summary>
        public int pocket1;

        /// <summary>
        /// Second (bottom) pocket, holding an object. 
        /// </summary>
        public int pocket2;

        /// <summary>
        /// Number of lives left.
        /// </summary>
        public int livesLeft;
    }
}