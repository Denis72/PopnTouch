using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Data;

namespace SurfaceApplicationTest
{
    public class Bubble
    {
        public int name { get; set; }
        public int duration { get; set; }
        private int angle;
        public int id { get; set; }
        private ScatterViewItem svitem { get; set; }
        private static ScatterView sv;
        private DispatcherTimer t;

        public Bubble(int _id, ScatterViewItem scatterItem)
        {
            Random r = new Random();
            if (sv == null) throw new Exception("No ScatterView defined (use Bubble.setScatterView())");
            angle = r.Next(0, 360);
            id = _id;


            //ellipse.TouchDown += new EventHandler<TouchEventArgs>(endAnimation);
            //<Ellipse Fill="Red" Opacity="0.5" Margin="10" TouchDown="endAnimation" />

            Image im = new Image();
            String path;
            
            switch (id)
            {
                default:
                case 0: path = "bullenote.png"; duration = 2;  break;
                case 1: path = "bullecroche.png"; duration = 1;  break;
                case 2: path = "bulleblanche.png"; duration = 4; break;
            }

            Grid grid = new Grid();
            name = SurfaceWindow1.nbBubble;
            im.Source = new BitmapImage(new Uri("Images/" + path, UriKind.Relative));
            im.IsHitTestVisible = false;
            im.Height = 60;
            im.Width = 60;

            grid.Children.Add(im);

            Ellipse ellipse = new Ellipse();       
            ellipse.Fill = System.Windows.Media.Brushes.Red;
            ellipse.Margin = new Thickness(20);
            ellipse.Opacity = 0.5;

            grid.Children.Add(ellipse);

            svitem = scatterItem;
            svitem.Background = Brushes.Transparent;
            svitem.ShowsActivationEffects = false;
            svitem.Height = 40;
            svitem.Width = 40;

            svitem.Content = grid;
           
            svitem.Name = "b"+name.ToString();
            SurfaceWindow1.nbBubble++;
            sv.Items.Add(scatterItem);

        }

        public static void setScatterView(ScatterView _sv)
        {
            sv = _sv;
        }

        public static bool ScatterViewIsNotSetted()
        {
            return (sv == null);
        }

        public void beginAnimation()
        {
            t = new DispatcherTimer();
            t.Interval = TimeSpan.FromMilliseconds(400);
            t.IsEnabled = true;
            t.Tick += new EventHandler(MovePortion);
        }
        public void endAnimation()
        {
            t.IsEnabled = false;
        }


        private void MovePortion(object source, EventArgs e)
        {
            Storyboard stb = new Storyboard();
            PointAnimation moveCenter = new PointAnimation();
            Random rand = new Random();
            Point p = svitem.ActualCenter;

            moveCenter.From = p;
            int vect = rand.Next(5, 10);

            /* Correction of the angle when the bubble is out limits */
            if (p.Y < 0 || p.Y > sv.Height)
            {
                angle = -angle;
                vect = 10;
            }
            else if (p.X > sv.Width || p.X < 0)
            {
                if (angle > 0)
                    angle = 180 - angle;
                else
                    angle = -180 - angle;
                vect = 10;
            }
            else
            {
                angle += rand.Next(-30, 30);
            }

            /* Correction of the angle overflow */
            if (angle > 180) { angle = -360 + angle; }
            if (angle < -180) { angle = 360 + angle; }

            /* New direction setted */

            p.X += vect * Math.Cos(angle * Math.PI / 180);
            p.Y += vect * Math.Sin(angle * Math.PI / 180);

            /* Animation */
            moveCenter.To = p;
            moveCenter.Duration = new Duration(TimeSpan.FromMilliseconds(400));
            svitem.Center = p;
            moveCenter.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(moveCenter);
            Storyboard.SetTarget(moveCenter, svitem);
            Storyboard.SetTargetProperty(moveCenter, new PropertyPath(ScatterViewItem.CenterProperty));
            stb.Begin();
        }


    }
}