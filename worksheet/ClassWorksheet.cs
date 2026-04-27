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
    // this class manages everything about the worksheets (may use sub-classes later on)
    // ws = worksheet
    {
        private string wsName { get; set; } = "markdown";
        private DateTime wsCreationDate { get; set; }
        private List<string> wsStringPages { get; set; } = new List<string>();
        private double wsWidth { get; set; } = 1050;
        public double wsHeight { get; set; } = 1440;

        private Grid gridWorkSheet;
        private List<Canvas> canvisWsPages;

        // might be moved to a different setting / config file
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
        // may also be move to a render-class (otherwise just use it once, while it gets created)
        {
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");

            ScrollViewer ScrollViewerWorksheet = new ScrollViewer();
            ScrollViewerWorksheet.HorizontalAlignment = HorizontalAlignment.Stretch;
            ScrollViewerWorksheet.VerticalAlignment = VerticalAlignment.Stretch;
            ScrollViewerWorksheet.Height = 1000; // must be chanhe in windowsizechange event later on

            StackPanel stackpanelWorksheet = new StackPanel();

            for (int pageNumber = 0; pageNumber < this.wsStringPages.Count(); pageNumber++)
            {
                Grid GridPage = new Grid();
                GridPage.Height = this.wsHeight * this.Zoom;
                GridPage.Width = this.wsWidth * this.Zoom;
                GridPage.Margin = new Thickness(0, pageMargin, 0, 0);

                GridPage.Background = Brushes.Red;

                TextBox TextboxPage = new TextBox();
                TextboxPage.TextChanged += TextboxPage_TextChanged; ;

                TextboxPage.AcceptsReturn = true; // allows user to press enter to create a new line inside of the textbox

                GridPage.Children.Add(TextboxPage);
                stackpanelWorksheet.Children.Add(GridPage);
            }
            ScrollViewerWorksheet.Content = stackpanelWorksheet;
            this.gridWorkSheet.Children.Add(ScrollViewerWorksheet);
        }

        private void TextboxPage_TextChanged(object sender, TextChangedEventArgs e)
        {
            //MessageBox.Show(Convert.ToString(sender));
            CheckIfNextPage(0);
        }

        private void CheckIfNextPage(int pagenumber)
        // this methode sees if the user should be creating a new page if he presses the enter-key
        {
            MessageBox.Show(wsStringPages[pagenumber]);
            if (wsStringPages[pagenumber].Split("\n").Count() >= 10)
            {
                MessageBox.Show("10 Zeilen");
            }
        }

        public void Render()
        // renders the pages that are in view (*italic*, **Bold**)
        {
            foreach (Canvas page in this.canvisWsPages)
            {

            }
        }


        public void Load(string filepath)
        // loads a worksheet from filepath
        {

        }
        public void Save(string filepath)
        // saves current worksheet to filepath
        {

        }
    }
}
