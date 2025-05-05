using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tectonic
{
    public static class ColorGenerator
    {
        public static List<Color> GenerateColors(int count)
        {
            var colors = new List<Color>();
            double goldenRatioConjugate = 0.61803398875; // Helps spread hues

            double hue = 0;
            for (int i = 0; i < count; i++)
            {
                hue += goldenRatioConjugate;
                hue %= 1;
                colors.Add(FromHSV(hue * 360, 0.5, 0.95));
            }

            return colors;
        }

        // Converts HSV to RGB Color
        private static Color FromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            byte v = Convert.ToByte(value);
            byte p = Convert.ToByte(value * (1 - saturation));
            byte q = Convert.ToByte(value * (1 - f * saturation));
            byte t = Convert.ToByte(value * (1 - (1 - f) * saturation));

            return hi switch
            {
                0 => Color.FromRgb(v, t, p),
                1 => Color.FromRgb(q, v, p),
                2 => Color.FromRgb(p, v, t),
                3 => Color.FromRgb(p, q, v),
                4 => Color.FromRgb(t, p, v),
                _ => Color.FromRgb(v, p, q),
            };
        }
    }
}
