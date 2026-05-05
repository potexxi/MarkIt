using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MarkIt
{
    public class GeneralSettings
    {
        private double width {  get; set; }
        private double height { get; set; }
        private Brush color {  get; set; }
        private List<Brush> colors {  get; set; }

        public GeneralSettings() { }

        public GeneralSettings(double width, double height, Brush color, List<Brush> colors)
        {
            this.width = width;
            this.height = height;
            this.color = color;
            this.colors = colors;
        }
    }
}
