
using System.Runtime.CompilerServices;

using GameEditorLib.Builder;

[assembly:InternalsVisibleTo("GameEditorTests")]


namespace GameEditorLib;

///
// Main entry point and top level definitions 
// for Game Editor Library.
///
static public class GameEditor
{
    /// <summary>
    /// Start here for all Game Editor needs.
    /// </summary>
    static public void Initialise()
    {
        Creator creator = new Creator();
        creator.BuildAll();
    }
}