
namespace ZX.Platform
{
    /// <summary>
    /// Each flag represents an action that can be taken by the user.
    /// </summary>
    /// <remarks>
    /// These should mapped to the platforms input states. The platform should
    /// create a concrete version of <see cref="IUserInput"/> which understands the 
    /// platforms inputs and map these to <see cref="UserInputFlags"/>. 
    /// </remarks>
    /// <seealso cref="IUserInput"/> 
    [Flags]
    public enum UserInputFlags : int
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8,
        FireA = 16,
        FireB = 32,
        MiscA = 64,
        MiscB = 128
    };
}