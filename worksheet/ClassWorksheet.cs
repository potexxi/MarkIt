using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MarkIt.worksheet
{
    public class ClassWorksheet
    // Diese Klasse soll das WorkSheet managen dazu soll gehoert = Bearbeitung, Speichern, Laden, Displayen(Live Rendering) 
    // WS = Worksheet
    {
        private string wsName { get; set; } = "markdown";
        private DateTime wsCreationDate { get; set; }
        private List<string> wsStringPages { get; set; } = new List<string>();
        private double wsWidth { get; set; } = 1050;
        public double wsHeight { get; set; } = 1440;

        private Grid gridWorkSheet;
        private List<Canvas> canvisWsPages;

        // muss vielleicht später in ein Settings-file / Class geändert werden
        private double Zoom = 0.5;
        private double pageMargin = 50;

        public ClassWorksheet()
        {

        }
        public ClassWorksheet(Grid _gridWorkSheet)
        {
            this.gridWorkSheet = _gridWorkSheet;
        }

        public void Init()
        // Nur einmal ausfuehren Laded alle Informationen aus der Klasse und erstellt die Pages (welche dann mit Render() gerendert werden können)!
        {
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");

            ScrollViewer ScrollViewerWorksheet = new ScrollViewer();
            ScrollViewerWorksheet.HorizontalAlignment = HorizontalAlignment.Stretch;
            ScrollViewerWorksheet.VerticalAlignment = VerticalAlignment.Stretch;
            ScrollViewerWorksheet.Height = 1000; // Muss in window Size mit der current window Size geändert werden

            StackPanel stackpanelWorksheet = new StackPanel();

            for (int pageNumber = 0; pageNumber < this.wsStringPages.Count(); pageNumber++)
            {
                Grid GridPage = new Grid();
                GridPage.Height = this.wsHeight * this.Zoom;
                GridPage.Width = this.wsWidth * this.Zoom;
                GridPage.Margin = new Thickness(0, pageMargin, 0, 0);

                GridPage.Background = Brushes.Red;

                TextBox TextboxPage = new TextBox();

                TextboxPage.AcceptsReturn = true; // Zeilen Umbrüche mit Enter erlauben

                GridPage.Children.Add(TextboxPage);
                stackpanelWorksheet.Children.Add(GridPage);
            }
            ScrollViewerWorksheet.Content = stackpanelWorksheet;
            this.gridWorkSheet.Children.Add(ScrollViewerWorksheet);

        }

        private void DisplayedPages()
        // Diese Methode soll Anzeigen, auf welcher Seite sich der Nutzer Befindet (Um potentziellen Lag bei vielen Pages zu vermeiden)
        {
            
        }

        public void Render()
        // Zeichnet die Blaetter & Scrawlbar. Irgendwann dann auch noch die Schrift Decorations (*italic*, **Bold**)
        {
            foreach (Canvas page in this.canvisWsPages)
            {

            }
        }
        public void Load(string filepath)
        // laded ein Worksheat von einem File in das 
        {

        }
        public void Save(string filepath)
        // laded ein Worksheat von einem File in das 
        {

        }
    }
}
