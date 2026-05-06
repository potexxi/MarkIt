using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkIt
{
    public class ColorTheme
    {
        public string Name {  get; private set; }
        public string MenuBarColor {  get; private set; }
        public string IconsColor {  get; private set; }
        public string HoverColor {  get; private set; }
        public string BackgroundColor {  get; private set; }
        public string SliderColor {  get; private set; }
        public string Foreground {  get; private set; }

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
