
using System.Drawing;

namespace ZX
{
    public interface IAttributeTable
    {
        void Clear(byte attribute);
        void SetAt(Point position, byte colours);
        void SetAt(int x, int y, byte colours);
        byte GetAt(Point position);
    }
}