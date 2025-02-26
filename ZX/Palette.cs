using ZX.Util;

namespace ZX
{
    public static class Palette
    {
        const float DarkValue = 0.6f;
        const float DarkYellow = 0.7f;
        const float DarkWhite = 0.7f;

        private enum ZxAttrIndex : int
        {
            // Order is important as we do some int maths with the enum.
            Black, Blue, Red, Magenta, Green, Cyan, Yellow, White,
            BrBlack, BrBlue, BrRed, BrMagenta, BrGreen, BrCyan, BrYellow, BrWhite
        };

        private static readonly Dictionary<ZxAttrIndex, Rgba> rgb = new Dictionary<ZxAttrIndex, Rgba>
        {
            { ZxAttrIndex.Black,   new Rgba(0.0f, 0.0f, 0.0f) },
            { ZxAttrIndex.Blue,    new Rgba(0.0f, 0.0f, DarkValue) },
            { ZxAttrIndex.Red,     new Rgba(DarkValue, 0.0f, 0.0f) },
            { ZxAttrIndex.Magenta, new Rgba(DarkValue, 0.0f, DarkValue) },
            { ZxAttrIndex.Green,   new Rgba(0.0f, DarkValue, 0.0f) },
            { ZxAttrIndex.Cyan,    new Rgba(0.0f, DarkValue, DarkValue) },
            { ZxAttrIndex.Yellow,  new Rgba(DarkYellow, DarkYellow, 0.0f) },
            { ZxAttrIndex.White,   new Rgba(DarkWhite, DarkWhite, DarkWhite) },
            { ZxAttrIndex.BrBlack,   new Rgba(0.001f, 0.001f, 0.001f) },
            { ZxAttrIndex.BrBlue,    new Rgba(0.0f, 0.0f, 1.0f) },
            { ZxAttrIndex.BrRed,     new Rgba(1.0f, 0.0f, 0.0f) },
            { ZxAttrIndex.BrMagenta, new Rgba(1.0f, 0.0f, 1.0f) },
            { ZxAttrIndex.BrGreen,   new Rgba(0.0f, 1.0f, 0.0f) },
            { ZxAttrIndex.BrCyan,    new Rgba(0.0f, 1.0f, 1.0f) },
            { ZxAttrIndex.BrYellow,  new Rgba(1.0f, 1.0f, 0.0f) },
            { ZxAttrIndex.BrWhite,   new Rgba(1.0f, 1.0f, 1.0f) }
        };

        static public byte GetAttribute(IAttribute colours)
        {
            int bright = 0;
            int paper = MatchColour(colours.Paper);
            int ink = MatchColour(colours.Ink);

            if(paper > 7)
            {
                paper -= 8;
                bright = 64;
            }

            if(ink > 7)
            {
                ink -= 8;
                bright = 64;
            }
            
            return (byte)(bright + ink + (paper << 3));
        }

        private static int MatchColour(Rgba colour)
        {
            var kvp = rgb.FirstOrDefault((pair) => colour == pair.Value );

            return (int)kvp.Key;
        }

        public static void SetAttribute(byte attribute, IAttribute objectToSet)
        {
            int bright = (attribute >> 6) & 0x01;
            ZxAttrIndex ink = (ZxAttrIndex)((attribute & 0x07) + (8*bright));
            ZxAttrIndex paper = (ZxAttrIndex)(((attribute >> 3) & 0x07) + (8*bright));

            objectToSet.Ink = rgb[ink];
            objectToSet.Paper = rgb[paper];
        }

        public static void SetAttribute(Rgba ink, Rgba paper, IAttribute objectToSet)
        {
            objectToSet.Ink = ink;
            objectToSet.Paper = paper;
        }

        public static void ToggleBright(IAttribute objectToSet)
        {
            ZxAttrIndex ink = rgb.Where(i => i.Value == objectToSet.Ink).First().Key;
            ZxAttrIndex paper = rgb.Where(i => i.Value == objectToSet.Paper).First().Key;

            objectToSet.Ink = rgb[ToggleBrightInner(ink)];
            objectToSet.Paper = rgb[ToggleBrightInner(paper)];            
        }

        private static ZxAttrIndex ToggleBrightInner(ZxAttrIndex key)
        {
            if((int)key < (int)ZxAttrIndex.BrBlack)
            {
                key += (int)ZxAttrIndex.BrBlack; 
            }
            else
            {
                key -= (int)ZxAttrIndex.BrBlack;
            }

            return key;
        }

        public static Rgba Black => rgb[ZxAttrIndex.Black];
        public static Rgba Blue => rgb[ZxAttrIndex.Blue];
        public static Rgba Red => rgb[ZxAttrIndex.Red];
        public static Rgba Magenta => rgb[ZxAttrIndex.Magenta];
        public static Rgba Green => rgb[ZxAttrIndex.Green];
        public static Rgba Cyan => rgb[ZxAttrIndex.Cyan];
        public static Rgba Yellow => rgb[ZxAttrIndex.Yellow];
        public static Rgba White => rgb[ZxAttrIndex.White];


        public static Rgba BrightBlack => rgb[ZxAttrIndex.BrBlack];
        public static Rgba BrightBlue => rgb[ZxAttrIndex.BrBlue];
        public static Rgba BrightRed => rgb[ZxAttrIndex.BrRed];
        public static Rgba BrightMagenta => rgb[ZxAttrIndex.BrMagenta];
        public static Rgba BrightGreen => rgb[ZxAttrIndex.BrGreen];
        public static Rgba BrightCyan => rgb[ZxAttrIndex.BrCyan];
        public static Rgba BrightYellow => rgb[ZxAttrIndex.BrYellow];
        public static Rgba BrightWhite => rgb[ZxAttrIndex.BrWhite];

        public static Rgba Transparent => new Rgba(0,0,0,0);
    }
}