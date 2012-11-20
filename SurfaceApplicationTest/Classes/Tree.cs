using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Surface.Presentation.Input;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using System.Timers;

namespace SurfaceApplicationTest
{

    public class Tree
    {
        private Image instrument1 { get; set; }
        private Image instrument2 { get; set; }
        private Image root { get; set; }
        private Image upper_branch { get; set; }
        private Image lower_branch { get; set; }
        private Image upper_root { get; set; }
        private Image lower_root { get; set; }
        private Image upper_choice { get; set; }
        private Image lower_choice { get; set; }

        private Canvas container { get; set; }

        private int default_inst { get; set; }

        public Tree(String path, String i1, String i2, Canvas _container)
        {
            container = _container;

            /*The instrument which is displayed in normal case (one of the two ones)*/
            instrument1 = new Image();
            instrument1.Source = new BitmapImage(new Uri(path + i1, UriKind.Relative));
            instrument1.Height = 72;
            instrument1.Width = 83;
            instrument1.Visibility = System.Windows.Visibility.Visible;
            container.Children.Add(instrument1);
            Canvas.SetLeft(instrument1, 10);
            Canvas.SetTop(instrument1, 80);

            instrument1.TouchDown += this.display_tree;

            /*The instrument which isn't displayed in normal case (one of the two ones)*/
            instrument2 = new Image();
            instrument2.Source = new BitmapImage(new Uri(path + i2, UriKind.Relative));
            instrument2.Height = 72;
            instrument2.Width = 83;
            instrument2.Visibility = System.Windows.Visibility.Hidden;
            container.Children.Add(instrument2);
            Canvas.SetLeft(instrument2, 10);
            Canvas.SetTop(instrument2, 80);

            instrument2.TouchDown += this.display_tree;

            /* The root which is displayed when the tree is displayed */
            root = new Image();
            root.Source = new BitmapImage(new Uri(path + "root.png", UriKind.Relative));
            root.Height = 50;
            root.Width = 56;
            root.Visibility = System.Windows.Visibility.Hidden;
            /* We add the root after the branches so that the branches are "under" the root ( z-axis problem solved) */
            Canvas.SetLeft(root, 23);
            Canvas.SetTop(root, 91);

            root.TouchDown += this.hide_tree;

            /* Tree's branches */
            upper_branch = new Image();
            upper_branch.Source = new BitmapImage(new Uri(path + "upper_branch.png", UriKind.Relative));
            upper_branch.Height = 99;
            upper_branch.Width = 214;
            upper_branch.Visibility = System.Windows.Visibility.Hidden;
            container.Children.Add(upper_branch);
            Canvas.SetLeft(upper_branch, 8);
            Canvas.SetTop(upper_branch, 21);

            lower_branch = new Image();
            lower_branch.Source = new BitmapImage(new Uri(path + "lower_branch.png", UriKind.Relative));
            lower_branch.Height = 99;
            lower_branch.Width = 214;
            lower_branch.Visibility = System.Windows.Visibility.Hidden;
            container.Children.Add(lower_branch);
            Canvas.SetLeft(lower_branch, 8);
            Canvas.SetTop(lower_branch, 99);

            Binding binding = new Binding();
            binding.Source = root;
            binding.Path = new PropertyPath("Visibility");
            upper_branch.SetBinding(Image.VisibilityProperty, binding);
            lower_branch.SetBinding(Image.VisibilityProperty, binding);

            /* We add the root after the branches so that the branches are "under" the root ( z axis problem solved) */
            container.Children.Add(root);

            /* Instruments which can be chosen and their respectives nodes */
            upper_root = new Image();
            upper_root.Source = new BitmapImage(new Uri(path + "root.png", UriKind.Relative));
            upper_root.Height = 50;
            upper_root.Width = 56;
            upper_root.Visibility = System.Windows.Visibility.Hidden;
            container.Children.Add(upper_root);
            Canvas.SetLeft(upper_root, 152);
            Canvas.SetTop(upper_root, 18);

            lower_root = new Image();
            lower_root.Source = new BitmapImage(new Uri(path + "root.png", UriKind.Relative));
            lower_root.Height = 50;
            lower_root.Width = 56;
            lower_root.Visibility = System.Windows.Visibility.Hidden;
            container.Children.Add(lower_root);
            Canvas.SetLeft(lower_root, 152);
            Canvas.SetTop(lower_root, 156);

            upper_root.SetBinding(Image.VisibilityProperty, binding);
            lower_root.SetBinding(Image.VisibilityProperty, binding);

            Image upper_choice = new Image();
            upper_choice.Source = new BitmapImage(new Uri(path + i1, UriKind.Relative));
            upper_choice.Height = 85;
            upper_choice.Width = 91;
            upper_choice.Visibility = System.Windows.Visibility.Hidden;
            container.Children.Add(upper_choice);
            Canvas.SetLeft(upper_choice, 138);
            Canvas.SetTop(upper_choice, 0);

            upper_choice.TouchDown += this.change_instrument1;
            upper_choice.TouchUp += this.change_instrument1;
            upper_choice.SetBinding(Image.VisibilityProperty, binding);

            Image lower_choice = new Image();
            lower_choice.Source = new BitmapImage(new Uri(path + i2, UriKind.Relative));
            lower_choice.Height = 77;
            lower_choice.Width = 74;
            lower_choice.Visibility = System.Windows.Visibility.Hidden;
            container.Children.Add(lower_choice);
            Canvas.SetLeft(lower_choice, 138);
            Canvas.SetTop(lower_choice, 142);

            lower_choice.TouchDown += this.change_instrument2;
            lower_choice.TouchUp += this.change_instrument2;
            lower_choice.SetBinding(Image.VisibilityProperty, binding);

        }

        private void display_tree(object sender, TouchEventArgs e)
        {
            ((Image)sender).Visibility = System.Windows.Visibility.Hidden;
            root.Visibility = System.Windows.Visibility.Visible;
        }

        private void hide_tree(object sender, TouchEventArgs e)
        {

            root.Visibility = System.Windows.Visibility.Hidden;
            if (default_inst == 0)
            {
                instrument1.Visibility = System.Windows.Visibility.Visible;
                instrument2.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                instrument1.Visibility = System.Windows.Visibility.Hidden;
                instrument2.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void change_instrument1(object sender, TouchEventArgs e)
        {
            default_inst = 0;
            hide_tree(sender, e);
        }

        private void change_instrument2(object sender, TouchEventArgs e)
        {
            default_inst = 1;
            hide_tree(sender, e);
        }
    }
}

