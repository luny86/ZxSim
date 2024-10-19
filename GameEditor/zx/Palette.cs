using KUtil;
using System.Collections.Generic;
using System.Linq;

namespace zx
{
    public static class Palette
    {
        const float DarkValue = 0.6f;

        private enum ZxAttr : int
        {
            // Order is important as we do some int maths with the enum.
            Black, Blue, Red, Magenta, Green, Cyan, Yellow, White,
            BrBlack, BrBlue, BrRed, BrMagenta, BrGreen, BrCyan, BrYellow, BrWhite
        };

        private static readonly Dictionary<ZxAttr, Rgba> rgb = new Dictionary<ZxAttr, Rgba>
        {
            { ZxAttr.Black,   new Rgba(0.0f, 0.0f, 0.0f) },
            { ZxAttr.Blue,    new Rgba(0.0f, 0.0f, DarkValue) },
            { ZxAttr.Red,     new Rgba(DarkValue, 0.0f, 0.0f) },
            { ZxAttr.Magenta, new Rgba(DarkValue, 0.0f, DarkValue) },
            { ZxAttr.Green,   new Rgba(0.0f, DarkValue, 0.0f) },
            { ZxAttr.Cyan,    new Rgba(0.0f, DarkValue, DarkValue) },
            { ZxAttr.Yellow,  new Rgba(DarkValue, DarkValue, 0.0f) },
            { ZxAttr.White,   new Rgba(DarkValue, DarkValue, 0.4f) },
            { ZxAttr.BrBlack,   new Rgba(0.0f, 0.0f, 0.0f) },
            { ZxAttr.BrBlue,    new Rgba(0.0f, 0.0f, 1.0f) },
            { ZxAttr.BrRed,     new Rgba(1.0f, 0.0f, 0.0f) },
            { ZxAttr.BrMagenta, new Rgba(1.0f, 0.0f, 1.0f) },
            { ZxAttr.BrGreen,   new Rgba(0.0f, 1.0f, 0.0f) },
            { ZxAttr.BrCyan,    new Rgba(0.0f, 1.0f, 1.0f) },
            { ZxAttr.BrYellow,  new Rgba(1.0f, 1.0f, 0.0f) },
            { ZxAttr.BrWhite,   new Rgba(1.0f, 1.0f, 1.0f) }
        };

        public static void SetAttribute(byte attribute, IAttribute objectToSet)
        {
            int bright = (attribute >> 6) & 0x01;
            ZxAttr ink = (ZxAttr)((attribute & 0x07) + (8*bright));
            ZxAttr paper = (ZxAttr)(((attribute >> 3) & 0x07) + (8*bright));

            Godot.GD.Print($"attr({attribute:X}) = P{paper} I{ink}");
            objectToSet.Ink = rgb[ink];
            objectToSet.Paper = rgb[paper];
        }

        public static void ToggleBright(IAttribute objectToSet)
        {
            ZxAttr ink = rgb.Where(i => i.Value == objectToSet.Ink).First().Key;
            ZxAttr paper = rgb.Where(i => i.Value == objectToSet.Paper).First().Key;

            objectToSet.Ink = rgb[ToggleBrightInner(ink)];
            objectToSet.Paper = rgb[ToggleBrightInner(paper)];            
        }

        private static ZxAttr ToggleBrightInner(ZxAttr key)
        {
            if((int)key < (int)ZxAttr.BrBlack)
            {
                key += (int)ZxAttr.BrBlack; 
            }
            else
            {
                key -= (int)ZxAttr.BrBlack;
            }

            return key;
        }

        public static Rgba Black => rgb[ZxAttr.Black];
        public static Rgba Blue => rgb[ZxAttr.Blue];
        public static Rgba Red => rgb[ZxAttr.Red];
        public static Rgba Magenta => rgb[ZxAttr.Magenta];
        public static Rgba Green => rgb[ZxAttr.Green];
        public static Rgba Cyan => rgb[ZxAttr.Cyan];
        public static Rgba Yellow => rgb[ZxAttr.Yellow];
        public static Rgba White => rgb[ZxAttr.White];


        public static Rgba BrightBlack => rgb[ZxAttr.BrBlack];
        public static Rgba BrightBlue => rgb[ZxAttr.BrBlue];
        public static Rgba BrightRed => rgb[ZxAttr.BrRed];
        public static Rgba BrightMagenta => rgb[ZxAttr.BrMagenta];
        public static Rgba BrightGreen => rgb[ZxAttr.BrGreen];
        public static Rgba BrightCyan => rgb[ZxAttr.BrCyan];
        public static Rgba BrightYellow => rgb[ZxAttr.BrYellow];
        public static Rgba BrightWhite => rgb[ZxAttr.BrWhite];
    }
}