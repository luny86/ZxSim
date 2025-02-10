
using System;
using ZX.Util;
using GameEditorLib.Builder;

namespace ZX.Drawing
{
    /// <summary>
    /// Main factory class for ZX.Drawing
    /// </summary>
    internal class Factory : IFactory , IBuildable
    {
        private IMemoryMap _memoryMap = null!;

        #region IFactory
        
        IDrawer IFactory.CreateTileDrawer(string scope)
        {
            IChunk chunk = _memoryMap[scope];
            return new TileDrawer(chunk);
        }   

        
        #endregion IFactory     
        
        #region IBuildable

        void IBuildable.AskForDependents(IRequests requests)
        {
            requests.AddRequest("Platform.Main.IMemoryMap", typeof(ZX.Util.IMemoryMap));
        }

        IList<IBuildable>? IBuildable.CreateBuildables()
        {
            return null;
        }


        void IBuildable.DependentsMet(IDependencies dependencies)
        {
            _memoryMap = (dependencies.TryGetInstance("Platform.Main.IMemoryMap",
			typeof(ZX.Util.IMemoryMap)) 
                as ZX.Util.IMemoryMap) ?? 
                throw new InvalidOperationException("Unable to get dependency IMemoryMap");
        }

        void IBuildable.RegisterObjects(IDependencyPool dependencies)
        {

        }

        #endregion IBuildable
    }
}