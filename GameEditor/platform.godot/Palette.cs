using Godot;
using KUtil;
using ZX;

namespace Platform
{
	public static class Palette
	{

		public static Color Colour(Rgba rgba) 
		{ 
			return  new Color(rgba.R, rgba.G, rgba.B, rgba.A);
		}

		public static Color Black => Colour(ZX.Palette.Black);
		public static Color Red => Colour(ZX.Palette.Red);
		public static Color Blue => Colour(ZX.Palette.Blue);
		public static Color Green => Colour(ZX.Palette.Green);
		public static Color Magenta => Colour(ZX.Palette.Magenta);
		public static Color Cyan => Colour(ZX.Palette.Cyan);
		public static Color Yellow => Colour(ZX.Palette.Yellow);
		public static Color White => Colour(ZX.Palette.White);


		public static Color BrightBlack => Colour(ZX.Palette.BrightBlack);
		public static Color BrightRed => Colour(ZX.Palette.BrightRed);
		public static Color BrightBlue => Colour(ZX.Palette.BrightBlue);
		public static Color BrightGreen => Colour(ZX.Palette.BrightGreen);
		public static Color BrightMagenta => Colour(ZX.Palette.BrightMagenta);
		public static Color BrightCyan => Colour(ZX.Palette.BrightCyan);
		public static Color BrightYellow => Colour(ZX.Palette.BrightYellow);
		public static Color BrightWhite => Colour(ZX.Palette.BrightWhite);
	}
}
