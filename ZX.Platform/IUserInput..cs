
namespace ZX.Platform
{
    /// <summary>
    /// Defines the user input object, which creates a bridge 
    /// between the platforms user input and the sims handler.
    /// </summary>
    /// <remarks>
    /// The platform should create a concrete object which is capable
    /// of taking the physical input, mapping it to the
    /// <see cref="UserInputFlags"/> and triggering the
    /// event handlers.
    /// </remarks>
    public interface IUserInput
    {
        event EventHandler<UserInputFlags> InputPressed;
        event EventHandler<UserInputFlags> InputReleased;
    }
}
