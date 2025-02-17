
namespace ZX.Drawing
{
    /// <summary>
    /// ZX.Drawing deals with any onscreen handling.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="Screen"/> is the screen handling for displaying
    /// the complete output for each frame. <see cref="Screen"/> is
    /// made up of <see cref="Layer"/>s which have their own Z-order
    /// and are drawn in that order. Each <see cref="Layer"/> should be
    /// created to handle drawing via <see cref="IDrawer"/>s and have
    /// its own <see cref="ZX.Platform.ISurface"/>s to create an image. This image
    /// is then automatically drawn to the screen when <see cref="Screen.Update"/>
    /// is called. The update should be called for every frame, but each
    /// layer update can be handled individually depending on what is required.
    /// </para>
    /// <para>
    /// <see cref="Screen" /> is exposed as "ZX.Drawing.Screen" by the composition builder.
    /// THere is also a <see cref="IFactory"/> exposed as "ZX.Drawing.IFactory". This cacreate any 
    /// drawers available.
    /// </para>
    /// <para>
    /// There are also helper classes such as <see cref="TileDrawer"/> which
    /// can draw 8 bit tiles (8 bytes) into a surface using colours that match
    /// the spectrum attributes emulating the basic background drawing used in
    /// a lot of games.
    /// </para>
    /// </remarks>
    [System.Runtime.CompilerServices.CompilerGenerated]
     class NamespaceDoc
    {

    }
}