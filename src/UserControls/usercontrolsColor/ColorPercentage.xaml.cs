using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Threading;

namespace MarkIt.UserControls.usercontrolsColor
{
    /// <summary>
    /// Interaktionslogik für ColorPercentage.xaml
    /// </summary>
    public partial class ColorPercentage : UserControl
    {
        private DispatcherTimer timer = new();
        private bool MouseDown = false;
        public ColorPercentage()
        {
            InitializeComponent();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e){ MouseDown = true; }
        private void RectSelection_MouseUp(object sender, MouseButtonEventArgs e){ MouseDown = false; }

        private void RectSelection_MouseEnter(object sender, MouseEventArgs e)
        {
            MouseDown = false;
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void RectSelection_MouseLeave(object sender, MouseEventArgs e)
        {
            timer.Tick -= Timer_Tick;
            timer.Stop();
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (MouseDown)
            {
                Point MousePosition = Mouse.GetPosition(RectSelection);
                double x = MousePosition.X;
                Canvas.SetLeft(Selection, x + 125);
                Percentage.CustomContent = Convert.ToString(CalcValue(x));
            }
        }

        private int CalcValue(double pos)
        {
            //chatgpt anfang
            //prompt: [File] give me the math for the CalcValue
            pos = Math.Max(0, Math.Min(pos, 125));
            return (int)Math.Round(pos / 125 * 255);
            //chatGPT ende
        }

        private double CalcPos(double value)
        {
            value = Math.Max(0, Math.Min(value, 255));
            return value / 255 * 125;
        }

        private void Percentage_TextChanged(object sender, TextChangedEventArgs e)
        {
            double x = 0;
            if (Percentage.CustomContent != null && Percentage.CustomContent != "")
               x = Convert.ToDouble(Percentage.CustomContent);
            Canvas.SetLeft(Selection, CalcPos(x) + 125);
        }
    }
}
