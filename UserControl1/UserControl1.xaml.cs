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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserControl1
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        public Line? Needle { get; set; } = null;
        public double Radius { get; set; }
        

        private void sldSpeedometer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            progressBarBrush.BeginAnimation(SolidColorBrush.ColorProperty, null);
            RotateNeedle(sldSpeedometer.Value);
            prgbar.Value = sldSpeedometer.Value;

            ColorAnimation colorAnimation = new ColorAnimation
            {
                To = Colors.Red,
                Duration = TimeSpan.FromMilliseconds(50), // Dauer der Animation in Millisekunden
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                FillBehavior = FillBehavior.Stop // Hält die Animation am Ende an
            };

            // Animation auf die Farbe der ProgressBar anwenden

            prgbar.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
            prgbar.Value = sldSpeedometer.Value;

        }

        private void canSpeedometer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Löschen und neu erzeugen und zeichnen
            canSpeedometer.Children.Clear();

            Radius = canSpeedometer.ActualWidth / 2 > canSpeedometer.ActualHeight * 0.9 ?
                           canSpeedometer.ActualHeight * 0.9 : canSpeedometer.ActualWidth / 2;

            RedrawNeedle();
            RedrawScale();
            RotateNeedle(sldSpeedometer.Value);

        }

        private void RedrawScale()
        {


            for (int angle = 0; angle <= 180; angle = angle + 10)
            {
                int i = 1;
                //Skalenstrich zeichnen
                Line scaleLine = new Line
                {
                    Stroke = new SolidColorBrush(Colors.White),
                    StrokeThickness = 1,
                    X1 = canSpeedometer.ActualWidth / 2 - Radius * 0.8,
                    Y1 = canSpeedometer.ActualHeight * 0.9,
                    X2 = canSpeedometer.ActualWidth / 2 - Radius * 0.85,
                    Y2 = canSpeedometer.ActualHeight * 0.9

                };
                RotateTransform rotateScale = new RotateTransform(angle, Needle.X1, Needle.Y1);
                scaleLine.RenderTransform = rotateScale;



                canSpeedometer.Children.Add(scaleLine);
                //Skalenbeschriftung
                TextBlock tbSpeedScale = new TextBlock
                {
                    Text = angle.ToString(),
                    Foreground = new SolidColorBrush(Colors.White),
                    Width = 30,
                    Height = 20,
                    TextAlignment = TextAlignment.Center,

                };
                //Canvas.SetTop(tbSpeedScale,  angle);
                //Canvas.SetLeft(tbSpeedScale,  angle);


                //Achtung die Transformationen in umgekehrter Reihenfolge im Code
                TransformGroup transformGroupTB = new TransformGroup();
                transformGroupTB.Children.Add(new TranslateTransform(-tbSpeedScale.Width / 2, -tbSpeedScale.Height / 2));
                transformGroupTB.Children.Add(new RotateTransform(+180 - angle));
                transformGroupTB.Children.Add(new TranslateTransform(Radius * 0.9, 0));
                transformGroupTB.Children.Add(new RotateTransform(-180 + angle));
                transformGroupTB.Children.Add(new TranslateTransform(Needle.X1, Needle.Y1));


                tbSpeedScale.RenderTransform = transformGroupTB;

                canSpeedometer.Children.Add(tbSpeedScale);
            }
        }

        private void RedrawNeedle()
        {


            Needle = new Line
            {
                Stroke = new SolidColorBrush(Colors.Red),
                StrokeThickness = 2,
                X1 = canSpeedometer.ActualWidth / 2,
                Y1 = canSpeedometer.ActualHeight * 0.9,
                X2 = canSpeedometer.ActualWidth / 2 - Radius * 0.75,
                Y2 = canSpeedometer.ActualHeight * 0.9

            };
            canSpeedometer.Children.Add(Needle);

        }
        private void RotateNeedle(double angle)
        {
            if (Needle != null)
            {
                RotateTransform rotateNeedle = new RotateTransform(angle, Needle.X1, Needle.Y1);
                Needle.RenderTransform = rotateNeedle;
            }



        }
    }
}
