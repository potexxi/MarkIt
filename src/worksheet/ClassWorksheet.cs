using MarkIt.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reactive;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;

namespace MarkIt.worksheet
{
    public class ClassWorksheet
    // this class manages everything about the worksheets (may use sub-classes later on)
    // ws = worksheet
    {
        private string wsName { get; set; } = "markdown";
        private DateTime wsCreationDate { get; set; }
        private List<string> wsStringPages { get; set; } = [""];
        private double wsWidth { get; set; } = 1050;
        public double wsHeight { get; set; } = 1440;

        public Grid gridWorkSheet;
        private List<Canvas> canvisWsPages;

        private List<int> selectionSize = [0, 0];
        private List<CustomLine> sellectedLines = new List<CustomLine>();
        private StackPanel stackpanelWorksheet;

        private DispatcherTimer dt;

        // might be moved to a different setting / config file
        private double Zoom = 0.8;
        private double pageMargin = 50;
        private bool AddingInProzess = false;

        private bool MouseIsDown = false;
        public ScrollViewer ScrollViewerWorksheet { get; private set; }
        public ClassWorksheet(){}
        public ClassWorksheet(Grid _gridWorkSheet) : this()
        {
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(10);
            dt.Tick += Dt_Tick;

            this.gridWorkSheet = _gridWorkSheet;
            stackpanelWorksheet = new StackPanel();
        }

        public void RenderLines()
        // may also be move to a render-class (otherwise just use it once, while it gets created)
        {
            this.gridWorkSheet.Children.Clear();
            this.stackpanelWorksheet.Children.Clear();
            ScrollViewerWorksheet = new ScrollViewer();
            ScrollViewerWorksheet.HorizontalAlignment = HorizontalAlignment.Stretch;
            ScrollViewerWorksheet.VerticalAlignment = VerticalAlignment.Stretch;
            ScrollViewerWorksheet.Height = 830; // must be chanhe in windowsizechange event later on
            stackpanelWorksheet.Margin = new System.Windows.Thickness(MainWindow.GeneralSettings.width);
            bool first = true;
            for (int pageNumber = 0; pageNumber < this.wsStringPages.Count(); pageNumber++)
            {
                CustomLine customLine = new CustomLine();
                customLine.CT_TextBox.Text = wsStringPages[pageNumber]; // reupdate the text
                customLine.Height = (int)MainWindow.GeneralSettings.height;
                customLine.fontsize = (int)MainWindow.GeneralSettings.height - 16;
                customLine.CT_TextBox.TextChanged += CT_TextBox_TextChanged;
                customLine.CT_TextBox.PreviewKeyDown += CT_TextBox_PreviewKeyDown;
                stackpanelWorksheet.Children.Add(customLine);
            }
            gridWorkSheet.PreviewMouseDown += GridWorkSheet_MouseDown;
            gridWorkSheet.PreviewMouseUp += GridWorkSheet_MouseUp;
            ScrollViewerWorksheet.Content = stackpanelWorksheet;
            this.gridWorkSheet.Children.Add(ScrollViewerWorksheet);
        }
        private void ClearSelction()
        {
            selectionSize[0] = 0;
            selectionSize[1] = 0;
            foreach (CustomLine cl in stackpanelWorksheet.Children)
            {
                cl.CT_TextBox.Background = Brushes.White;
                cl.CT_TextBox.Foreground = Brushes.Black;
            }
            sellectedLines.Clear();

        }
        private void GridWorkSheet_MouseUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = false;
            MouseIsDown = false;
            dt.Stop();
        }

        public bool IsMarked { get; private set; }

        private void GridWorkSheet_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            ClearSelction();
            MouseIsDown = true;

            for (int i = 0; i < stackpanelWorksheet.Children.Count; i++)
            {
                CustomLine cl = (CustomLine)stackpanelWorksheet.Children[i];

                if (cl.IsMouseWithin)
                {
                    selectionSize[0] = i;
                    selectionSize[1] = i;
                    break;
                }
            }
            if (!dt.IsEnabled)
                dt.Start();
        }
        private void SelectLine(CustomLine cl)
        {
            if (!sellectedLines.Contains(cl))
                sellectedLines.Add(cl);

            cl.CT_TextBox.Focus();
            Keyboard.Focus(cl.CT_TextBox);
            cl.CT_TextBox.CaretIndex = cl.CT_TextBox.Text.Length;
        }
        private void Dt_Tick(object? sender, EventArgs e)
        {
            int i = 0;
            if (MouseIsDown)
            {
                foreach (CustomLine cl in stackpanelWorksheet.Children)
                {
                    cl.CT_TextBox.Background = Brushes.White;
                    cl.CT_TextBox.Foreground = Brushes.Black;
                }
                foreach (CustomLine cl in stackpanelWorksheet.Children)
                {
                    if (cl.IsMouseWithin)
                    {
                        SelectLine(cl);
                        selectionSize[1] = i;
                    }
                    i++;
                }
                int start = Math.Min(selectionSize[0], selectionSize[1]); // chatgpt anfang
                int end = Math.Max(selectionSize[0], selectionSize[1]); // chatgpt ende

                for (int i2 = 0; i2 < stackpanelWorksheet.Children.Count; i2++)
                {
                    if (i2 >= start && i2 <= end)
                    {

                        CustomLine cl = (CustomLine)stackpanelWorksheet.Children[i2];
                        if (!sellectedLines.Contains(cl)) // chatgpt
                            sellectedLines.Add(cl);
                        cl.CT_TextBox.Background = new SolidColorBrush(Color.FromRgb(228, 228, 228));
                        cl.CT_TextBox.Foreground = Brushes.Gray;
                    }
                }
            }
        }

        private void CT_TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && Keyboard.Modifiers.HasFlag(ModifierKeys.Control)) // chatgpt diese line weil e kann nur eine Taste gleichzeitig sein
            {
                e.Handled = true;
                //TODO STR C
            }
            if (e.Key == Key.V && Keyboard.Modifiers.HasFlag(ModifierKeys.Control)) // chatgpt diese line weil e kann nur eine Taste gleichzeitig sein
            {
                e.Handled = true;
                //TODO STR V
            }
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
                if (sellectedLines.Count > 1)
                    deletSelection();
                TextBox cl = (TextBox)sender;
                if (cl.CaretIndex == 0)
                    deletLine();
            }
            else
            {
                ClearSelction();
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
        public void addTabel(int height, int width)
        {
            List<string> lines = [];

            string header = "";
            for (int x = 0; x < width; x++)
            {
                header += "|    ";
            }
            header += "|";
            lines.Add(header);

            string Spliter = "";
            for (int x = 0; x < width; x++)
            {
                Spliter += "|---";
            }
            Spliter += "|";
            lines.Add(Spliter);

            for (int y = 0; y < height; y++)
            {
                string lineContent = "";
                for (int x = 0; x < width; x++)
                {
                    lineContent += "|    ";
                }
                lineContent += "|";
                lines.Add(lineContent);
            }
            for (int i = lines.Count - 1; i >= 0; i--) // otherwise backwarts
            {
                makeLineNoCarterIDX(lines[i]);
            }
        }
        
        private void makeLineNoCarterIDX(string lineContent)
        {
            int line = -1;
            string oldline = "";
            string newline = "";

            line = checkCurrentLine();
            if (line == -1)
            {
                AddingInProzess = false;
                return;
            }
            int cursorPOS = getCursorPosition(line);

            CustomLine customLine = new CustomLine();
            customLine.CT_TextBox.Text = lineContent;
            customLine.Height = (int)MainWindow.GeneralSettings.height;
            customLine.fontsize = (int)MainWindow.GeneralSettings.height - 16;
            int indx = wsStringPages.Count - 1;
            customLine.CT_TextBox.TextChanged += CT_TextBox_TextChanged;
            customLine.CT_TextBox.PreviewKeyDown += CT_TextBox_PreviewKeyDown;
            stackpanelWorksheet.Children.Insert(line + 1, customLine);
            wsStringPages.Insert(line + 1, lineContent);
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
            customLine.fontsize = (int)MainWindow.GeneralSettings.height - 16;
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

        private void deletSelection()
        {
            //chatgpt
            int start = Math.Min(selectionSize[0], selectionSize[1]);
            int end = Math.Max(selectionSize[0], selectionSize[1]);

            for (int i = end; i >= start; i--)
            {
                if (i <= 0)
                    continue;

                wsStringPages.RemoveAt(i);
                stackpanelWorksheet.Children.RemoveAt(i);
            }
            //chatgpt ende
            ClearSelction();
        }
        private void deletLine(bool bypass = false)
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
            if(wsStringPages[line] == "" || cursorPOS == 0 || bypass)
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

        public void LoadFromString(string content)
        { // loads the
            wsStringPages.Clear();
            string[] splitted = content.Split("\n");
            for (int i = 0; i < splitted.Length; i++)
            {
                wsStringPages.Add(splitted[i]);
            }
            if (wsStringPages.Count == 0)
                wsStringPages.Add("");
            RenderLines();
            Logger.logger.Debug("Loaded content succsesfully");
        }
        public string GetContent()
        { // gives back pages as a string
            string content = "";
            for (int i = 0; i < wsStringPages.Count; i++)
            {
                content += wsStringPages[i];
                if (i != wsStringPages.Count - 1)
                    content += "\n";
            }
            return content;
        }
    }
}
