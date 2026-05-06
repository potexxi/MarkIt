using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MarkIt
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
    }
}
