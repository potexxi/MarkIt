using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace MarkIt.settings
{
    public class ColorTheme
    {
        public string Name { get; init; }
        public string MenuBarColor {  get; init; }
        public string IconsColor {  get; init; }
        public string HoverColor {  get; init; }
        public string BackgroundColor {  get; init; }
        public string SliderColor {  get; init; }
        public string Foreground {  get; init; }

        public ColorTheme() { }
        public ColorTheme(string name, string menubarcolor, string iconscolor, string hovercolor, string backgroundcolor, string slidercolor, string foreground)
        {
            Name = name;
            MenuBarColor = menubarcolor;
            IconsColor = iconscolor;
            HoverColor = hovercolor;
            BackgroundColor = backgroundcolor;
            SliderColor = slidercolor;
            Foreground = foreground;
        }

        static private string tohex(int dez)
        {
            string[] HEX = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F"];
            string hex1 = "";
            string hex2 = "";
            int hexi1 = 0;
            int hexi2 = 0;

            for (int i = 0; i < dez; i++)
            {
                hexi1 += 1;
                if (hexi1 >= 16)
                {
                    hexi1 = 0;
                    hexi2 += 1;
                    hex2 = HEX[hexi2];
                }
            }
            hex1 = HEX[hexi1];
            hex2 = HEX[hexi2];
            return hex2 + hex1;
        }
        static public string RGBToHEX(int red, int green, int blue)
        // # RR GG BB
        {
            string hex = "#FF";
            hex += ColorTheme.tohex(red);
            hex += ColorTheme.tohex(green);
            hex += ColorTheme.tohex(blue);
            return hex;
        }
    }
}
