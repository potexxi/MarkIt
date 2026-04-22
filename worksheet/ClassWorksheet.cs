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
            wsStringPages.Add("test");
            ScrollViewer ScrollViewerWorksheet = new ScrollViewer();
            ScrollViewerWorksheet.HorizontalAlignment = HorizontalAlignment.Stretch;
            ScrollViewerWorksheet.VerticalAlignment = VerticalAlignment.Stretch;


            StackPanel stackpanelWorksheet = new StackPanel();
            //stackpanelWorksheet.Width = gridWorkSheet.Width;
            //stackpanelWorksheet.Height = gridWorkSheet.Height;

            for (int pageNumber = 0; pageNumber < this.wsStringPages.Count(); pageNumber ++)
            {
                Grid CanvasPage = new Grid();
                CanvasPage.Height = this.wsHeight;
                CanvasPage.Width = this.wsWidth;
                CanvasPage.Margin = new Thickness(0, 100, 0, 0);

                CanvasPage.Background = Brushes.Black;
                PageTextBoxes(CanvasPage);
                stackpanelWorksheet.Children.Add(CanvasPage);
            }
            ScrollViewerWorksheet.Content = stackpanelWorksheet;
            this.gridWorkSheet.Children.Add(ScrollViewerWorksheet);

        }

        private void PageTextBoxes(Grid gridPage)
        // Unterteilt eine Seite in TextBoxen (Für # | " | ' | * |)
        {
            gridPage.Children.Clear();
            double TextBoxHeight = 35;
            StackPanel StackPanelLines = new StackPanel();
            StackPanelLines.Orientation = Orientation.Vertical;
            StackPanelLines.VerticalAlignment = VerticalAlignment.Center;
            StackPanelLines.HorizontalAlignment = HorizontalAlignment.Center;
            for (int line = 0; line < (gridPage.Height / TextBoxHeight); line++)
            {
                TextBox TextboxLine = new TextBox();
                TextboxLine.Height = TextBoxHeight;
                TextboxLine.Width = gridPage.Width;
                StackPanelLines.Children.Add(TextboxLine);
            }
            gridPage.Children.Add(StackPanelLines);
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
