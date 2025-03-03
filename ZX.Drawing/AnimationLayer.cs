
using ZX.Game;
using ZX.Platform;

namespace ZX.Drawing
{
    /// <summary>
    /// The animation layer is used to display an array of
    /// animated sprites.
    /// </summary>
    /// <remarks>
    /// Restrictions include that all animations have to be the same size.
    /// Animations are cleared if a new level or new game is called. This is
    /// invoked if the layer is added to the <see cref="Game.IGameProvider"/> 
    /// </remarks>
    internal class AnimationLayer : Layer, IGameItem, IAnimationLayer
    {
        /// <summary>
        /// Draw object used by all animations.
        /// This means all animations have to be the same size.
        /// </summary>
        private readonly IDrawer _spriteDrawer;

        private List<IAnimation> _animations = new List<IAnimation>();

        /// <summary>
        /// Create a layer.
        /// </summary>
        /// <param name="name">Unique name, required is placing on the <see cref="IScreen   ">.</param>
        /// <param name="spriteDrawer"><see cref="IDrawer"/> for rendering the animations.</param>
        /// <param name="surface">Platform surface to holds the resulting pixels.</param>
        /// <param name="z">Z order of layer, required if placed on the <see cref="IScreen"/> </param>
        public AnimationLayer(string name, IDrawer spriteDrawer, ISurface surface, int z)
        : base(name, surface, z)
        {
            _spriteDrawer = spriteDrawer;
        }

        /// <summary>
        /// Give an animation to the layer.
        /// </summary>
        /// <param name="animation"><see cref="IAnimation"/> to be displayed on the layer.</param>
        public void RegisterAnimation(IAnimation animation)
        {
            _animations.Add(animation);
        }

        /// <summary>
        /// Remove an animation from the layer.
        /// </summary>
        /// <param name="name">Name of animation to remove.</param>
        public void DeregisterAnimation(string name)
        {
            IAnimation? animation = _animations.First((a) => a.Name == name );

            if(animation is not null)
            {
                _animations.Remove(animation);
            }
        }

        /// <summary>
        /// Updats the rendering of all enabled animations.
        /// </summary>
        public override void Update()
        {
            List<IAnimation> completed = new List<IAnimation>();

            // Clear all previous animations.
            Surface.Fill(Palette.Transparent);

            foreach(IAnimation anim in _animations)
            {
                if(anim.Enabled)
                {
                    anim.Update();
                }

                if(anim.Completed)
                {
                    completed.Add(anim);
                }

                if(anim.Enabled)
                {
                    _spriteDrawer.Draw(Surface, anim.Frame, anim.Position.X, anim.Position.Y);
                }
            }

            foreach(IAnimation anim in completed)
            {
                _animations.Remove(anim);
            }
        }

        void IGameStatic.NewGame()
        {
            _animations.Clear();
        }

        void IGameStatic.NewLevel()
        {
            _animations.Clear();
        }
    }
}