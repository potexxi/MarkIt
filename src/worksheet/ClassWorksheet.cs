using MarkIt.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MarkIt.worksheet
{
    public class ClassWorksheet
    // this class manages everything about the worksheets (may use sub-classes later on)
    // ws = worksheet
    {
        private TextBox textBoxContent { get; set; }
        private string wsName { get; set; } = "markdown";
        private DateTime wsCreationDate { get; set; }
        private List<string> wsStringPages { get; set; } = new List<string>();
        private double wsWidth { get; set; } = 1050;
        public double wsHeight { get; set; } = 1440;

        private Grid gridWorkSheet;
        private List<Canvas> canvisWsPages;
        private StackPanel stackpanelWorksheet;

        // might be moved to a different setting / config file
        private double Zoom = 0.8;
        private double pageMargin = 50;

        public ClassWorksheet() {}
        public ClassWorksheet(Grid _gridWorkSheet)
        {
            this.gridWorkSheet = _gridWorkSheet;
            stackpanelWorksheet = new StackPanel();
        }

        public void Init()
        // may also be move to a render-class (otherwise just use it once, while it gets created)
        {
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");
            wsStringPages.Add("testpage");

            ScrollViewer ScrollViewerWorksheet = new ScrollViewer();
            ScrollViewerWorksheet.HorizontalAlignment = HorizontalAlignment.Stretch;
            ScrollViewerWorksheet.VerticalAlignment = VerticalAlignment.Stretch;
            ScrollViewerWorksheet.Height = 1000; // must be chanhe in windowsizechange event later on
            stackpanelWorksheet.Margin = new System.Windows.Thickness(MainWindow.GeneralSettings.width);
            bool first = true;
            for (int pageNumber = 0; pageNumber < this.wsStringPages.Count(); pageNumber++)
            {
                CustomLine customLine = new CustomLine();
                customLine.Height = (int)MainWindow.GeneralSettings.height;
                customLine.fontsize = (int)MainWindow.GeneralSettings.height - 2;
                stackpanelWorksheet.Children.Add(customLine);
            }
            ScrollViewerWorksheet.Content = stackpanelWorksheet;
            this.gridWorkSheet.Children.Add(ScrollViewerWorksheet);
        }

        public void addToPostion(string symbols)
        {
            int line = checkCurrentLine();
            if (line != -1)
            {
                int cursorpos = getCursorPosition(line);
                CustomLine l = (CustomLine)stackpanelWorksheet.Children[line];
                string content = l.CT_TextBox.Text;
                string bevor = "";
                string after = "";
                int currentpos = 0;
                foreach (char c in content)
                {
                    if (currentpos >= cursorpos)
                        after += c;
                    else
                        bevor += c;
                    currentpos++;
                }
                l.CT_TextBox.Text = bevor + symbols + symbols + after;
                l.CT_TextBox.CaretIndex = cursorpos + symbols.Length;
            }
        }
        public void addToPostion(string symbols,string symbolsEND)
        {
            int line = checkCurrentLine();
            if (line != -1)
            {
                int cursorpos = getCursorPosition(line);
                CustomLine l = (CustomLine)stackpanelWorksheet.Children[line];
                string content = l.CT_TextBox.Text;
                string bevor = "";
                string after = "";
                int currentpos = 0;
                foreach (char c in content)
                {
                    if (currentpos >= cursorpos)
                        after += c;
                    else
                        bevor += c;
                    currentpos++;
                }
                l.CT_TextBox.Text = bevor + symbols + symbolsEND + after;
                l.CT_TextBox.CaretIndex = cursorpos + symbols.Length;
            }
        }
        private int checkCurrentLine()
        {
            for(int i = 0; i < stackpanelWorksheet.Children.Count; i ++)
            {
                if (stackpanelWorksheet.Children[i].IsKeyboardFocusWithin)
                {
                    return i;
                }
            }
            return -1; // default return
        }
        private int getCursorPosition(int line)
        {
            CustomLine l = (CustomLine)stackpanelWorksheet.Children[line]; // converts the child into the line
            return l.CT_TextBox.CaretIndex;  // CaretIndex is form ChatGPT what it does is that it shows the current cursor position
        }

        private void TextboxPage_TextChanged(object sender, TextChangedEventArgs e)
        {
            ClassPages page = new ClassPages(gridWorkSheet);
            TextBox textbox = (TextBox)sender;
            page.content = textbox.Text;
            page.Render();
            //MessageBox.Show(Convert.ToString(sender));
            CheckIfNextPage(0);
        }


        private void CheckIfNextPage(int pagenumber)
        // this methode sees if the user should be creating a new page if he presses the enter-key
        {
            //MessageBox.Show(wsStringPages[pagenumber]);
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

        public void LoadFromString(string content)
        {
            textBoxContent.Text = content;
        }
    }
}
