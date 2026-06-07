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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;

namespace MarkIt.worksheet
{
    public class ClassWorksheet
    // this class manages everything about the worksheets (may use sub-classes later on)
    // ws = worksheet
    {
        private TextBox textBoxContent { get; set; }
        private string wsName { get; set; } = "markdown";
        private DateTime wsCreationDate { get; set; }
        private List<string> wsStringPages { get; set; } = [""];
        private double wsWidth { get; set; } = 1050;
        public double wsHeight { get; set; } = 1440;

        private Grid gridWorkSheet;
        private List<Canvas> canvisWsPages;
        private StackPanel stackpanelWorksheet;

        // might be moved to a different setting / config file
        private double Zoom = 0.8;
        private double pageMargin = 50;
        private bool AddingInProzess = false;

        public ClassWorksheet()
        {
        }
        public ClassWorksheet(Grid _gridWorkSheet):this()
        {
            this.gridWorkSheet = _gridWorkSheet;
            stackpanelWorksheet = new StackPanel();
        }

        public void RenderLines()
        // may also be move to a render-class (otherwise just use it once, while it gets created)
        {
            this.gridWorkSheet.Children.Clear();
            this.stackpanelWorksheet.Children.Clear();
            ScrollViewer ScrollViewerWorksheet = new ScrollViewer();
            ScrollViewerWorksheet.HorizontalAlignment = HorizontalAlignment.Stretch;
            ScrollViewerWorksheet.VerticalAlignment = VerticalAlignment.Stretch;
            ScrollViewerWorksheet.Height = 1000; // must be chanhe in windowsizechange event later on
            stackpanelWorksheet.Margin = new System.Windows.Thickness(MainWindow.GeneralSettings.width);
            bool first = true;
            for (int pageNumber = 0; pageNumber < this.wsStringPages.Count(); pageNumber++)
            {
                CustomLine customLine = new CustomLine();
                customLine.CT_TextBox.Text = wsStringPages[pageNumber]; // reupdate the text
                customLine.Height = (int)MainWindow.GeneralSettings.height;
                customLine.fontsize = (int)MainWindow.GeneralSettings.height - 20;
                customLine.CT_TextBox.TextChanged += CT_TextBox_TextChanged;
                customLine.CT_TextBox.PreviewKeyDown += CT_TextBox_PreviewKeyDown;
                stackpanelWorksheet.Children.Add(customLine);
            }
            ScrollViewerWorksheet.Content = stackpanelWorksheet;
            this.gridWorkSheet.Children.Add(ScrollViewerWorksheet);
        }

        private void CT_TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true; // ChatGPT to stop the refocus on the old line
                if(AddingInProzess)
                    return;
                addLine();
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true; //tells the programm that e has been handelt
                moveLineUp();
            }
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                moveLineDown();
            }
            if (e.Key == Key.Back)
            {
                TextBox cl = (TextBox)sender;

                if (cl.CaretIndex == 0)
                    deletLine();
            }
        }

        private void CT_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox changedTextBox = (TextBox)sender;
            int l = checkCurrentLine();
            if (l != -1 && l < wsStringPages.Count)
                wsStringPages[l] = changedTextBox.Text;
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
        public void addToLineBeginning(string symbol)
        {
            int line = checkCurrentLine();
            if (line != -1)
            {
                int cursorpos = getCursorPosition(line);
                CustomLine l = (CustomLine)stackpanelWorksheet.Children[line];
                string content = l.CT_TextBox.Text;
                l.CT_TextBox.Text = symbol + content;
                l.CT_TextBox.CaretIndex = cursorpos + symbol.Length;
            }
        }
        private int checkCurrentLine()
        { // returns the current line
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
            if (line == -1)
                return -1;
            CustomLine l = (CustomLine)stackpanelWorksheet.Children[line]; // converts the child into the line
            return l.CT_TextBox.CaretIndex;  // CaretIndex is form ChatGPT what it does is that it shows the current cursor position
        }


        private void moveLineUp()
        {
            int line = -1;
            int linepos = -1;
            if (stackpanelWorksheet.Children != null)
            {
                line = checkCurrentLine();
                if (line == -1)
                    return;
                else
                {
                    linepos = getCursorPosition(line);
                }
            }

            if (line <= 0)
                return;
            if (stackpanelWorksheet.Children[line - 1] == null)
                return;
            CustomLine customLine = (CustomLine)stackpanelWorksheet.Children[line  - 1];
            //chatGPT beginning (comments from me tryint to explain the code to make it more sencefull)
            //promt: please help me fix this
            customLine.Dispatcher.BeginInvoke(new Action(() => // makes a new Dispatcher timer for the new line bc WPF is wierd like that and needs this...
            {
                customLine.CT_TextBox.Focus();// focuses the line
                Keyboard.Focus(customLine.CT_TextBox); // sets the keyboard focus to the line
                FocusManager.SetFocusedElement( // tells WPF more directly what element to focus on
                    FocusManager.GetFocusScope(customLine.CT_TextBox), // sets the focus to the right line
                    customLine.CT_TextBox // the textbox to focus
                );

                customLine.CT_TextBox.CaretIndex = linepos; // change the carter index to the beginning of the line
            }), System.Windows.Threading.DispatcherPriority.Input); // runs the focusing code later if WPF is still processing previews inputs
            //chatGPT end
        }
        private void moveLineDown()
        {
            int line = -1;
            int linepos = -1;
            if (stackpanelWorksheet.Children != null)
            {
                line = checkCurrentLine();
                if (line == -1)
                    return;
                else
                {
                    linepos = getCursorPosition(line);
                }
            }

            if (line >= stackpanelWorksheet.Children.Count - 1)
                return;
            CustomLine customLine = (CustomLine)stackpanelWorksheet.Children[line + 1];
            //chatGPT beginning (comments from me tryint to explain the code to make it more sencefull)
            //promt: please help me fix this
            customLine.Dispatcher.BeginInvoke(new Action(() => // makes a new Dispatcher timer for the new line bc WPF is wierd like that and needs this...
            {
                customLine.CT_TextBox.Focus();// focuses the line
                Keyboard.Focus(customLine.CT_TextBox); // sets the keyboard focus to the line
                FocusManager.SetFocusedElement( // tells WPF more directly what element to focus on
                    FocusManager.GetFocusScope(customLine.CT_TextBox), // sets the focus to the right line
                    customLine.CT_TextBox // the textbox to focus
                );

                customLine.CT_TextBox.CaretIndex = linepos; // change the carter index to the beginning of the line
            }), System.Windows.Threading.DispatcherPriority.Input); // runs the focusing code later if WPF is still processing previews inputs
            //chatGPT end
        }
        private void addLine()
        {
            AddingInProzess = true;
            int line = -1;
            string oldline = ""; // I searched this up I could use substring instead of a for loop but I am still going to use a for bc it looks like chatgpt if I use a substring
            string newline = "";
            string addon = ""; // (in the case of a list it gets continued)
            if (stackpanelWorksheet.Children != null)
            {
                line = checkCurrentLine();
                if (line == -1)
                {
                    AddingInProzess = false;
                    return;
                }
                int cursorPOS = getCursorPosition(line);
                for (int i = 0; i < wsStringPages[line].Length; i++)
                {
                    if (i < cursorPOS)
                        oldline += wsStringPages[line][i];
                    else
                        newline += wsStringPages[line][i];
                }

                if (oldline.Length >= 2)
                {
                    if (oldline[0] == '-' && oldline[1] == ' ')
                        addon = "- ";
                }
                if (oldline.Length >= 3)
                {
                    int i = 0;
                    try
                    {
                        i = Convert.ToInt32(oldline[0].ToString()); // laut chatgpt nimmt es sonst die asci Zahl was 50 ergibt bei 1 ...
                        if (oldline[1] == '.' && oldline[2] == ' ')
                            addon = (i + 1) + ". ";
                    }
                    catch
                    {
                        Logger.logger.Verbose("Not able to continue List, due to ERROR while trying to make a new orderd list");
                    }
                }
                if (oldline.Length >= 4)
                {
                    try
                    {
                        string ZahlenBisHundert = oldline[0].ToString() + oldline[1].ToString();
                        int i = Convert.ToInt32(ZahlenBisHundert); // laut chatgpt nimmt es sonst die asci Zahl was 50 ergibt bei 1 ...
                        if (oldline[2] == '.' && oldline[3] == ' ')
                            addon = (i + 1) + ". ";
                    }
                    catch
                    {
                        Logger.logger.Verbose("Not able to continue List, due to ERROR while trying to make a new orderd list");
                    }
                }
                if (oldline.Length >= 5)
                {
                    if (oldline[0] == '-' && oldline[1] == ' ' && oldline[2] == '[' && oldline[3] == ' ' && oldline[4] == ']')
                        addon = "- [ ] ";
                }

                wsStringPages[line] = oldline; // makes it so if you press enter in the middle of the line splits the line and puts it into the next
                wsStringPages.Insert(line + 1, addon + newline);
                CustomLine CL_oldline = (CustomLine)stackpanelWorksheet.Children[line];
                CL_oldline.CT_TextBox.Text = oldline;
            }
            CustomLine customLine = new CustomLine();
            customLine.CT_TextBox.Text = addon + newline;
            customLine.Height = (int)MainWindow.GeneralSettings.height;
            customLine.fontsize = (int)MainWindow.GeneralSettings.height - 20;
            int indx = wsStringPages.Count - 1;
            customLine.CT_TextBox.TextChanged += CT_TextBox_TextChanged;
            customLine.CT_TextBox.PreviewKeyDown += CT_TextBox_PreviewKeyDown;
            stackpanelWorksheet.Children.Insert(line+1, customLine);

            //chatGPT beginning (comments from me tryint to explain the code to make it more sencefull)
            //promt: please help me fix this
            customLine.Dispatcher.BeginInvoke(new Action(() => // makes a new Dispatcher timer for the new line bc WPF is wierd like that and needs this...
            {
                customLine.CT_TextBox.Focus();// focuses the line
                Keyboard.Focus(customLine.CT_TextBox); // sets the keyboard focus to the line
                FocusManager.SetFocusedElement( // tells WPF more directly what element to focus on
                    FocusManager.GetFocusScope(customLine.CT_TextBox), // sets the focus to the right line
                    customLine.CT_TextBox // the textbox to focus
                ); 

                customLine.CT_TextBox.CaretIndex = addon.Length; // change the carter index to the beginning of the line
                AddingInProzess = false;
            }), System.Windows.Threading.DispatcherPriority.Input); // runs the focusing code later if WPF is still processing previews inputs
            //chatGPT end
        }
        private void deletLine()
        { // deletes a line (if the current lines content is 0)
            int line = -1;
            int cursorPOS = -1;
            if (stackpanelWorksheet.Children != null)
            {
                line = checkCurrentLine();
                if (line == -1)
                    return;
                cursorPOS = getCursorPosition(line);
            }
            if(wsStringPages[line] == "" || cursorPOS == 0)
            {
                if(line <= 0)
                    return; // to make sure you dont remove line 1
                CustomLine oldline = (CustomLine)stackpanelWorksheet.Children[line];
                string oldstrcontent = oldline.CT_TextBox.Text;
                wsStringPages.RemoveAt(line);
                stackpanelWorksheet.Children.RemoveAt(line);
                CustomLine customLine = (CustomLine)stackpanelWorksheet.Children[line - 1];
                int OLDcarterINDX = customLine.CT_TextBox.Text.Length;
                customLine.CT_TextBox.Text += oldstrcontent;
                //chatGPT beginning (comments from me tryint to explain the code to make it more sencefull)
                //promt: please help me fix this
                customLine.Dispatcher.BeginInvoke(new Action(() => // makes a new Dispatcher timer for the new line bc WPF is wierd like that and needs this...
                {
                    customLine.CT_TextBox.Focus();// focuses the line
                    Keyboard.Focus(customLine.CT_TextBox); // sets the keyboard focus to the line
                    FocusManager.SetFocusedElement( // tells WPF more directly what element to focus on
                        FocusManager.GetFocusScope(customLine.CT_TextBox), // sets the focus to the right line
                        customLine.CT_TextBox // the textbox to focus
                    );
                    customLine.CT_TextBox.CaretIndex = OLDcarterINDX; // change the carter index to the beginning of the line
                }), System.Windows.Threading.DispatcherPriority.Input); // runs the focusing code later if WPF is still processing previews inputs
            //chatGPT end
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
