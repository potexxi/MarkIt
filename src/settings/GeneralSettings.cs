using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace MarkIt.settings
{
    public class GeneralSettings
    {
        public double width {  get; set; }
        public double height { get; set; }
        public bool iconAnimations { get; set; }
        public bool liveRendering { get; set; }
        public string animationFPS { get; set; }
        public ColorTheme? currentColorTheme {  get; set; }
        private List<ColorTheme>? colorThemes {  get; set; }

        public GeneralSettings() { }

        public GeneralSettings(double width, double height, bool iconAnimaition, bool liveRendering, string animationFPS)
        {
            setColorsFromFile();
            this.width = width;
            this.height = height;
            this.iconAnimations = iconAnimaition;
            this.liveRendering = liveRendering;
            this.animationFPS = animationFPS;

            currentColorTheme = colorThemes[0];
        }

        public static GeneralSettings? LoadFromFile(string filename)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    return JsonSerializer.Deserialize<GeneralSettings>(sr.ReadToEnd());
                }
            }
            catch
            {
                return null;
            }
        }

        public void SaveToFile(string filename)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            using(StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(JsonSerializer.Serialize(this, options));
            }
        }

        public List<ColorTheme>? GetAllColors()
        {
            return colorThemes;
        }

        public void ChangeColor(ColorTheme color)
        {
            currentColorTheme = color;
        }

        public void ChangeSize(double width, double height)
        {
            this.width = width;
            this.height = height;
        }

        private void setColorsFromFile()
        {
            try
            {
                using (StreamReader sr = new StreamReader("sources/options/color-themes.json"))
                {
                    colorThemes = JsonSerializer.Deserialize<List<ColorTheme>>(sr.ReadToEnd());
                    Logger.logger.Debug("Loaded all color themes.");
                }
            }
            catch
            {
                Logger.logger.Warning("No file color-themes.json found!");
                ColorTheme colortheme = new ColorTheme("default", "#FFEA00", "#FF1F4572", "#FFFFFF");
                colorThemes.Add(colortheme);
                currentColorTheme = colortheme;
            }
        }

        public void SaveColorsToFile()
        {
            var path = Directory.GetDirectoryRoot("sources/options/color-themes.json");
            Directory.CreateDirectory(path);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            File.WriteAllText("sources/options/color-themes.json", JsonSerializer.Serialize(colorThemes, options));
        }
    }
}
