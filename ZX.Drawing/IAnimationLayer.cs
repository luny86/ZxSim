
namespace ZX.Drawing
{
    public interface IAnimationLayer
    {

        /// <summary>
        /// Give an animation to the layer.
        /// </summary>
        /// <param name="animation"><see cref="IAnimation"/> to be displayed on the layer.</param>
        void RegisterAnimation(IAnimation animation);

        /// <summary>
        /// Remove an animation from the layer.
        /// </summary>
        /// <param name="name">Name of animation to remove.</param>
        void DeregisterAnimation(string name);
    }
}