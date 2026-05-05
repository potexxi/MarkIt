using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MarkIt
{
    public class GeneralSettings
    {
        public double width {  get; private set; }
        private double height { get; set; }
        private ColorTheme? currentColorTheme {  get; set; }
        private List<ColorTheme>? colorThemes {  get; set; }

        public GeneralSettings() { }

        public GeneralSettings(double width, double height, ColorTheme color)
        {
            setColorsFromFile();
            this.width = width;
            this.height = height;
            this.currentColorTheme = color;
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
            this.currentColorTheme = color;
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
                var box = new WindowMessageBox("Load error!", "A unexpected error forced the application to stop.");
                box.ShowDialog();
                Application.Current.Shutdown();
            }
        }
    }
}
