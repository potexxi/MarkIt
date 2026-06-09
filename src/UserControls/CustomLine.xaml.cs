using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MarkIt.UserControls
{
    /// <summary>
    /// Interaktionslogik für CustomLine.xaml
    /// </summary>
    public partial class CustomLine : UserControl
    {
        private int _fontsize = 30;
        public int fontsize
        {
            get
            {
                return _fontsize;
            }
            set
            {
                _fontsize = value;
                TextboxLineText.FontSize = _fontsize;
            }
        }
        public TextBox CT_TextBox
        {
            get
            {
                return TextboxLineText;
            }
            set
            {
                TextboxLineText = value;
            }
        }

        //live rendering Settings
        Rectangle Rect_Overlay;
        StackPanel StackPanel_TextBoxes;
        TextBlock RenderedText;
        //live rendering Settings end -
        public CustomLine()
        {
            InitializeComponent();
            InitLiveRender();
        }
        public void InitLiveRender()
        {
            StackPanel_TextBoxes = new StackPanel();
            StackPanel_TextBoxes.VerticalAlignment = VerticalAlignment.Stretch;
            StackPanel_TextBoxes.HorizontalAlignment = HorizontalAlignment.Stretch;

            Rect_Overlay = new Rectangle();
            Rect_Overlay.Fill = Brushes.White;
            Rect_Overlay.VerticalAlignment = VerticalAlignment.Stretch;
            Rect_Overlay.HorizontalAlignment = HorizontalAlignment.Stretch;

            RenderedText = new TextBlock();
            RenderedText.VerticalAlignment = VerticalAlignment.Center;
            RenderedText.FontSize = _fontsize;
            RenderedText.Padding = new Thickness(4, 0, 0, 0);

            StackPanel_TextBoxes.Children.Add(RenderedText);
        }

        private void Render()
        {
            //chatGPT anfang
            //promt: Füge meine findSymbol funktion zusammen damit ich das Liverending Add runn nutzen kann
            RenderedText.Inlines.Clear();

            string txt = CT_TextBox.Text;
            int i = 0;

            List<List<int>> boldSymbols = findSymbole(txt, "**");
            List<List<int>> italicSymbols = findSymbole(txt, "*");
            List<List<int>> codeSymbols = findSymbole(txt, "`");
            List<List<int>> underlineStartSymbols = findSymbole(txt, "<u>");
            List<List<int>> underlineEndSymbols = findSymbole(txt, "</u>");
            List<List<int>> supStartSymbols = findSymbole(txt, "<sup>");
            List<List<int>> supEndSymbols = findSymbole(txt, "</sup>");
            List<List<int>> subStartSymbols = findSymbole(txt, "<sub>");
            List<List<int>> subEndSymbols = findSymbole(txt, "</sub>");
            List<List<int>> strikeSymbols = findSymbole(txt, "~~");

            while (i < txt.Length)
            {
                if (FindNextSymbol(boldSymbols, i, 2) == i)
                {
                    int end = FindNextSymbol(boldSymbols, i + 2, 2);

                    if (end != -1)
                    {
                        AddRun(txt.Substring(i + 2, end - i - 2), bold: true);
                        i = end + 2;
                        continue;
                    }
                }

                if (FindNextSymbol(codeSymbols, i, 1) == i)
                {
                    int end = FindNextSymbol(codeSymbols, i + 1, 1);

                    if (end != -1)
                    {
                        AddRun(txt.Substring(i + 1, end - i - 1), code: true);
                        i = end + 1;
                        continue;
                    }
                }

                if (FindNextSymbol(underlineStartSymbols, i, 3) == i)
                {
                    int end = FindNextSymbol(underlineEndSymbols, i + 3, 4);

                    if (end != -1)
                    {
                        AddRun(txt.Substring(i + 3, end - i - 3), underline: true);
                        i = end + 4;
                        continue;
                    }
                }

                if (FindNextSymbol(supStartSymbols, i, 5) == i)
                {
                    int end = FindNextSymbol(supEndSymbols, i + 5, 6);

                    if (end != -1)
                    {
                        AddRun(txt.Substring(i + 5, end - i - 5), sup: true);
                        i = end + 6;
                        continue;
                    }
                }

                if (FindNextSymbol(subStartSymbols, i, 5) == i)
                {
                    int end = FindNextSymbol(subEndSymbols, i + 5, 6);

                    if (end != -1)
                    {
                        AddRun(txt.Substring(i + 5, end - i - 5), sub: true);
                        i = end + 6;
                        continue;
                    }
                }

                if (FindNextSymbol(strikeSymbols, i, 2) == i)
                {
                    int end = FindNextSymbol(strikeSymbols, i + 2, 2);

                    if (end != -1)
                    {
                        AddRun(txt.Substring(i + 2, end - i - 2), strike: true);
                        i = end + 2;
                        continue;
                    }
                }

                if (FindNextSymbol(italicSymbols, i, 1) == i)
                {
                    int end = FindNextSymbol(italicSymbols, i + 1, 1);

                    if (end != -1)
                    {
                        AddRun(txt.Substring(i + 1, end - i - 1), italic: true);
                        i = end + 1;
                        continue;
                    }
                }

                AddRun(txt[i].ToString());
                i++;
                //chat GPT ende
            }
        }

        private void AddRun(string text, bool bold = false, bool italic = false, bool underline = false, bool strike = false, bool code = false, bool sup = false, bool sub = false)
        {
            // chatgpt anfang (erklärt von mir)
            // promt: what can I use instead of findSymbol to not have to make TextBoxes for all of this
            Run run = new Run(text); // das Run Element ist ein Teil von Einer Textbox, z.B: wenn ich einen kleinen Text teil (in diesem fall ein char) kann ich damit die Decoration ändern für diesen Text teil

            if (bold) // geht durch ob eines von den tags activ ist und ändert die Textdecoration so damit es passt
                run.FontWeight = FontWeights.Bold;
            if (italic)
                run.FontStyle = FontStyles.Italic;
            if (underline)
                run.TextDecorations = TextDecorations.Underline;
            if (strike)
                run.TextDecorations = TextDecorations.Strikethrough;
            if (code)
                run.FontFamily = new FontFamily("Consolas");
            if (sup) // extra prompt damit hochzahlen / subscripts nicht das ganze Textfeld ruinieren (font size wird verkleinert)
            {
                run.BaselineAlignment = BaselineAlignment.Superscript;
                run.FontSize = _fontsize * 0.65;
            }
            if (sub) // extra prompt damit hochzahlen / subscripts nicht das ganze Textfeld ruinieren (font size wird verkleinert)
            {
                run.BaselineAlignment = BaselineAlignment.Subscript;
                run.FontSize = _fontsize * 0.65;
            }
            RenderedText.Inlines.Add(run); // adds the chat run zum Gerenderten Text
            //chatgpt ende
        }


        private void UserControl_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
            {// checks if should be rendering
            if(CT_TextBox.IsKeyboardFocused)
            {
                Grid_line.Children.Remove(Rect_Overlay);
                Grid_line.Children.Remove(StackPanel_TextBoxes);
            }
            else if (MainWindow.GeneralSettings.liveRendering)
            {
                Render();

                if (!Grid_line.Children.Contains(Rect_Overlay))
                    Grid_line.Children.Add(Rect_Overlay);

                if (!Grid_line.Children.Contains(StackPanel_TextBoxes))
                    Grid_line.Children.Add(StackPanel_TextBoxes);
            }
        }
        private bool IsAt(string txt, int index, string symbol)
        {// checks position if  it contains the symbol list
            if (index + symbol.Length > txt.Length)
                return false;

            for (int j = 0; j < symbol.Length; j++)
            {
                if (txt[index + j] != symbol[j])
                    return false;
            }

            return true;
        }
        public List<List<int>> findSymbole(string txt, string symbol)
        { // finds the simbol
            List<List<int>> foundchains = new();
            int letterNumber = 0;
            int lineNumber = 0;
            int chainlength = 0;

            if (txt != null && symbol != "")
            {
                string[] splited = txt.Split("\n");

                foreach (string line in splited)
                {
                    lineNumber++;
                    letterNumber = 0;

                    for (int i = 0; i <= line.Length - symbol.Length; i++, letterNumber++)
                    {
                        if (IsAt(line, i, symbol))
                        {
                            chainlength = 0;

                            for (
                                int chainstart = letterNumber;
                                chainstart <= line.Length - symbol.Length &&
                                IsAt(line, chainstart, symbol);
                                chainstart += symbol.Length
                            )
                            {
                                chainlength += symbol.Length;
                            }

                            List<int> entry = [letterNumber, lineNumber, chainlength];
                            foundchains.Add(entry);

                            i += chainlength - 1;
                            letterNumber += chainlength - 1;
                        }
                    }
                }
            }

            return foundchains;
        }
        private int FindNextSymbol(List<List<int>> symbols, int afterIndex, int minLength)
        { // goes through the List of symbols that if found
            foreach (List<int> symbol in symbols)
            {
                if (symbol[0] >= afterIndex && symbol[2] >= minLength)
                    return symbol[0];
            }

            return -1;
        }
    }
}
