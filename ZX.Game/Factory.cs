
namespace ZX.Game
{
    internal class Factory : IFactory
    {
        IFlag IFactory.CreateFlag(int value, int relatedObjectIndex)
        {
            return new Flag(value, relatedObjectIndex);
        }
    }
}