
using System.Linq;
using System.Collections.Generic;
using ZX.Platform;

namespace ZX.Drawing
{
    /// <summary>
    /// Defines a virtual screen from which layers can be added, which hold drawers and
    /// surfaces. Each layer is updated and copied to the main surface, which is created by
    /// the project. The screen's main blit method should then be called during
    /// the games loop process.
    /// </summary>
    internal class Screen : IScreen
    {
        private readonly List<ILayer> layers;

        /// <summary>
        /// Create a screen with a main surface.
        /// </summary>
        public Screen()
        {
            layers = new List<ILayer>();
        }
    
        /// <summary>
        ///  Main screen surface to which all layers will be blended to.
        /// </summary>
        public ISurface Main { get; set; } = null!;

        /// <summary>
        /// Gets a containing layer based on its name.
        /// </summary>
        ILayer IScreen.this[string name]
        {
            get 
            {
                ILayer found = layers.First(l => l.Name == name);
                return found;
            }
        }

        /// <summary>
        /// Creates a new layer based on the given drawer and surface.
        /// This is then added to the main screen in order of it's
        /// Z ordering. Any layers with sharing ordering are blitted
        /// in order of creation.
        /// </summary>
        public void AddLayer(ILayer layer)
        {
            layers.Add(layer);
            layers.Sort();
        }

        /// <summary>
        /// Blend all layers to the main surface.
        /// </summary>
        public void Update()
        {
            foreach(ILayer layer in layers)
            {
                if(layer.Visible)
                {
                    layer.Blend(Main);
                }
            }
        }
    }
}