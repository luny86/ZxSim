using Godot;
using KUtil;

namespace Platform
{
    public static class Palette
    {

        public static Color Colour(Rgba rgba) 
        { 
            return  new Color(rgba.R, rgba.G, rgba.B, rgba.A);
        }

        public static Color Black => Colour(zx.Palette.Black);
        public static Color Red => Colour(zx.Palette.Red);
        public static Color Blue => Colour(zx.Palette.Blue);
        public static Color Green => Colour(zx.Palette.Green);
        public static Color Magenta => Colour(zx.Palette.Magenta);
        public static Color Cyan => Colour(zx.Palette.Cyan);
        public static Color Yellow => Colour(zx.Palette.Yellow);
        public static Color White => Colour(zx.Palette.White);


        public static Color BrightBlack => Colour(zx.Palette.BrightBlack);
        public static Color BrightRed => Colour(zx.Palette.BrightRed);
        public static Color BrightBlue => Colour(zx.Palette.BrightBlue);
        public static Color BrightGreen => Colour(zx.Palette.BrightGreen);
        public static Color BrightMagenta => Colour(zx.Palette.BrightMagenta);
        public static Color BrightCyan => Colour(zx.Palette.BrightCyan);
        public static Color BrightYellow => Colour(zx.Palette.BrightYellow);
        public static Color BrightWhite => Colour(zx.Palette.BrightWhite);
    }
}