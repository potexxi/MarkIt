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
using System.Windows.Shapes;

namespace MarkIt
{
    /// <summary>
    /// Interaktionslogik für WindowMessageBox.xaml
    /// </summary>
    public partial class WindowMessageBox : Window
    {
        public ReturnType returnType { get; private set; }
        public enum ButtonType
        {
            Okay,
            YesNo
        }

        public enum ReturnType
        {
            Okay,
            Yes,
            No
        }

        public WindowMessageBox(string heading, string content, ButtonType buttonType)
        {
            InitializeComponent();
            LabelHeading.Content = heading;
            LabelContent.Content = content;
            DrawButtons(buttonType);
        }

        private void DrawButtons(ButtonType buttonType)
        {
            if(buttonType == ButtonType.Okay)
            {
                Button button = new Button();
                button.Content = "Okay";
                button.Width = 200;
                button.Height = 40;
                button.Cursor = Cursors.Hand;
                button.FontSize = 18;
                button.FontFamily = new FontFamily("Leelawadee UI");
                button.Click += Okay_Click;
                StackPanelButtons.Children.Add(button);
            }
            else if(buttonType == ButtonType.YesNo)
            {
                Button button1 = new Button();
                button1.Content = "Yes";
                button1.Width = 95;
                button1.Height = 40;
                button1.Cursor = Cursors.Hand;
                button1.FontSize = 18;
                button1.FontFamily = new FontFamily("Leelawadee UI");
                button1.Click += Yes_Click;
                StackPanelButtons.Children.Add(button1);

                Button button2 = new Button();
                button2.Content = "No";
                button2.Width = 95;
                button2.Height = 40;
                button2.Cursor = Cursors.Hand;
                button2.FontSize = 18;
                button2.FontFamily = new FontFamily("Leelawadee UI");
                button2.Margin = new Thickness(10, 0, 0, 0);
                button2.Click += No_Click;
                StackPanelButtons.Children.Add(button2);
            }
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            returnType = ReturnType.No;
            this.Close();
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            returnType = ReturnType.Yes;
            this.Close();
        }

        private void Okay_Click(object sender, RoutedEventArgs e)
        {
            returnType = ReturnType.Okay;
            this.Close();
        }
    }
}
