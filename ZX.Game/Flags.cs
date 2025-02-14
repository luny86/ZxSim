
namespace ZX.Game
{
    /// Flags are indicators during game play
    /// and can represent different states of objects
    /// and rooms.
    /// </summary>
    /// <remarks>
    /// Flags can be registered as an <see cref="IGameItem"/> so 
    /// that the flags are reset for a new game. Does nothing
    /// for a new level.
    /// </remarks>
    internal class Flags : IFlags, IGameStatic
    {
        private class EmptyFlag : IFlag
        {
            int IFlag.Value { get; set; }

            int IFlag.Object => -1;
        }

        /// <summary>
        /// Create an empty instance 
        /// </summary>
        public Flags()
        {
            Dictionary = new Dictionary<string, IFlag>();
        }

        /// <summary>
        /// Flags are stored as a name/value pair.
        /// </summary>
        /// <value>The dictionary of flags.</value>
        private Dictionary<string, IFlag> Dictionary
        {
            get;
            set;
        }

        #region IGameStatic

        void IGameStatic.NewGame()
        {
            foreach (var pair in Dictionary)
            {
                pair.Value.Value = 0;
            }
        }

        void IGameStatic.NewLevel()
        {

        }
        #endregion

        /// <summary>
        /// Registers a flag with a given name and initial value.
        /// </summary>
        /// <param name="name">Name of flag to create.</param>
        /// <param name="initialValue">Initial value of flag.</param>
        /// <exception cref="ArgumentException">Thrown if name already exists.</exception>
        public void RegisterFlag(string name, IFlag initialValue)
        {
            if (Dictionary.ContainsKey(name))
            {
                throw new ArgumentException(string.Format(
                    "Flag '{0}' already exists.",
                    name));
            }

            Dictionary.Add(name, initialValue);
        }

        /// <summary>
        /// Gets or sets the value of a named flag.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <exception cref="ArgumentOutOfRange">Thrown if name of flag is not found.</exception>
        public IFlag this[string name]
        {
            get
            {
                if (!Dictionary.ContainsKey(name))
                {
                    throw new ArgumentOutOfRangeException(string.Format(
                        "Unable to find flag '{0}'.",
                        name));
                }

                return Dictionary[name];
            }

            set
            {
                if (!Dictionary.ContainsKey(name))
                {
                    throw new ArgumentOutOfRangeException(string.Format(
                        "Unable to find flag '{0}'.",
                        name));
                }

                Dictionary[name] = value;
            }
        }

        public IFlag GetByObjectIndex(int index)
        {
            IFlag flag = new EmptyFlag();

            foreach (var pair in Dictionary)
            {
                if (pair.Value.Object == index)
                {
                    flag = pair.Value;
                    break;
                }
            }

            return flag;
        }
    }
}