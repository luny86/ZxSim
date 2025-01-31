using Godot;
using ZX.Util;
using System;
using GameEditorLib.Platform;

namespace Platform
{
	/// <summary>
	/// Wraps the image class of the platform.
	/// </summary>
	internal class Surface : ISurface
	{
		public event EventHandler Updated;

		public Surface()
		{
		}

		public Image Image { get; private set; }

		/// <summary>
		/// Create the underlying image at a given width and height.
		/// </summary>
		/// <param name="w"></param>
		/// <param name="h"></param>
		public void Create(int w, int h)
		{
			Image = Image.CreateEmpty(w, h, false, Image.Format.Rgba8);
		}

		public virtual void BeginDraw()
		{
		}

		public virtual void EndDraw()
		{
			OnImageUpdated();
		}
		
		public void Fill(Rgba colour)
		{
			Image.Fill(Palette.Colour(colour));
		}

		public void FillRect(Rectangle rect, Rgba colour)
		{
			Image.FillRect(
				new Rect2I(rect.X, rect.Y, rect.W, rect.H),
				Palette.Colour(colour));
		}

		public void SetPixel(int x, int y, Rgba colour)
		{
			Image.SetPixel(x,y, Palette.Colour(colour));
		}

		/// <summary>
		/// Invokes the Updated event. Use when the image has changed.
		/// </summary>
		protected void OnImageUpdated()
		{
			Updated?.Invoke(this, new EventArgs());
		}

		public bool IsInBounds(int x, int y)
		{
			return 
				x >= 0 && x <= Image.GetWidth()
				&&
				y >= 0 && y <= Image.GetHeight();
		}
	}
}
