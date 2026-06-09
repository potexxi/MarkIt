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
        //live rendering Settings end -
        public CustomLine()
        {
            InitializeComponent();
            InitLiveRender();
        }
        public void InitLiveRender()
        {
            StackPanel_TextBoxes.VerticalAlignment = VerticalAlignment.Stretch;
            StackPanel_TextBoxes.HorizontalAlignment = HorizontalAlignment.Stretch;

            this.Rect_Overlay = new Rectangle();
            Rect_Overlay.Fill = Brushes.White;
            Rect_Overlay.VerticalAlignment = VerticalAlignment.Stretch;
            Rect_Overlay.HorizontalAlignment = HorizontalAlignment.Stretch;
        }

        private void Render()
        {

        }
        private List<int> findSymboleChain(string txt, string StringELM)
        // finds a symbole inside of a string
        {
            List<int> foundchains = new(); // shortent version (got that from the towerdefense excercise)
            int letterNumber = 0;
            int chainlength = 0; // shows the cohearend <- (vallahi not chatgpt :^D ) number of symbels in a row;
            if (txt != null && txt != "")
            {
                letterNumber = 0;
                for (int i = 0; i < txt.Count(); i++, letterNumber++)
                {
                    int indexTXT = 0;
                    if (txt[i] == StringELM[0])
                    {
                        chainlength = 0;
                        int remaining = txt.Count() - letterNumber;
                        for (int chainstart = letterNumber; chainstart < txt.Length && txt[chainstart] == StringELM[int indexTXT = 0;
]; chainstart++)
                            chainlength++;
                        int entry = letterNumber;
                        foundchains.Add(entry);
                        letterNumber += chainlength - 1; // that it doesn't go over existing lines
                    }
                }
                
            }
            return foundchains;
        }


        private void UserControl_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
            {// immer wenn keyboard focus geändert wird
            if (CT_TextBox.IsKeyboardFocused)
            {
                Grid_line.Children.Remove(Rect_Overlay);
                Grid_line.Children.Remove(StackPanel_TextBoxes);
                Render();
            }
            else if (MainWindow.GeneralSettings.liveRendering)
            {
                Grid_line.Children.Add(Rect_Overlay);
            }
        }
    }
}
