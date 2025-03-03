
namespace Pyjamarama.House
{
    /// <summary>
    /// Describes a test.
    /// </summary>
    public interface ITest
    {
        /// <summary>
        /// Gets the number of bytes expected in
        /// the data passed to the method <see cref="Test"/> .
        /// </summary>
        /// <value>The size of the test data.</value>
        int TestDataSize
        {
            get;
        }

        /// <summary>
        /// Run test.
        /// </summary>
        /// <param name="data">Data to base test on.</param>
        bool Test(IList<byte> data);
    }
}

